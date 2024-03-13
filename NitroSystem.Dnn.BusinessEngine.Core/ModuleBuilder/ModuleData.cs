using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Common;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Core.Enums;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using NitroSystem.Dnn.BusinessEngine.Utilities;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder
{
    public class ModuleData : JObject, IModuleData
    {
        private readonly IExpressionService _expressionService;

        private string _connectionID { get; set; }
        private Dictionary<string, string> _pageParams { get; set; }
        private bool _isUpdated { get; set; }
        private static IEnumerable<ModuleVariableInfo> _moduleVariables { get; set; }

        public event EventHandler OnChangeModuleData;
        public Guid? ModuleID { get; set; }

        public ModuleData(IExpressionService expressionService)
        {
            this._expressionService = expressionService;
        }

        public void InitModuleData(Guid? moduleID, string connectionID, int userID, IDictionary<string, object> form, IDictionary<string, object> field, string pageUrl, bool clearData = false)
        {
            this.ModuleID = moduleID;
            this._connectionID = connectionID;

            if (!clearData && moduleID!=null)
            {
                var items = GetDbModuleData();
                foreach (var item in items) this.Add(item.Key, item.Value);
            }

            if (this["Field"] == null) this.Add("Field", JObject.Parse("{}"));
            if (this["Form"] == null) this.Add("Form", JObject.Parse("{}"));
            if (this["_CurrentUser"] == null)
                this.Add("_CurrentUser", JObject.Parse(@"{""UserID"":" + userID + "}"));
            else
                this["_CurrentUser"] = JObject.Parse(@"{""UserID"":" + userID + "}");

            if (form != null) this["Form"] = JObject.FromObject(form);

            var variables = _moduleVariables = GetVariables();
            foreach (var item in variables ?? Enumerable.Empty<ModuleVariableInfo>())
            {
                var type = TypeUtil.GetSystemType(item.VariableType);
                var defaultValue = TypeUtil.GetSystemTypeDefaultValue(type);

                if (this[item.VariableName] == null)
                {
                    this.Add(item.VariableName, JToken.FromObject(defaultValue));

                    if (item.VariableType == "viewModel" && item.ViewModelID != null) populateViewModelProperties(this[item.VariableName] as JObject, item.ViewModelID.Value);
                }
            }

            if (field != null) this["Field"] = JObject.FromObject(field);

            SetPageParam(pageUrl);

            UpdateDbModuleData();
        }

        public void SetFieldItem(string fieldName, object field)
        {
            var item = this["Field"] as JObject;

            if (item != null) item.Add(fieldName, JObject.FromObject(field));
        }

        public void AddProperty(string propertyPath)
        {
            var token = this.SelectToken(propertyPath);
            if (token == null) this.Add(propertyPath, new JObject());
        }

        public JObject GetModuleData(string connectionID, Guid? moduleID = null)
        {
            var result = JObject.Parse("{}");

            var variables = GetVariables(moduleID).Where(v => v.Scope == 0).Select(v => v.VariableName);

            var items = GetDbModuleData(connectionID, moduleID);
            foreach (var item in items.Properties())
            {
                if (item.Name == "Form" || item.Name == "Field" || variables.Contains(item.Name)) result.Add(item);
            }
            return result;
        }

        public object GetData(string propertyPath)
        {
            if (this._isUpdated)
            {
                var data = GetDbModuleData();

                this._isUpdated = false;
            }

            try
            {
                var token = this.SelectToken(propertyPath);
                return token != null ? token : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public void SetProperty(JObject target, string propertyPath, object data)
        {
            var parts = propertyPath.Split('.');
            var leftExpr = parts[0];

            if (parts.Length == 1)
            {
                if (target[leftExpr] == null)
                {
                    target.Add(leftExpr, "");
                    var prop = target.SelectToken(leftExpr);
                    SetValue(prop, data);
                }
            }
            else
            {
                var prop = target.SelectToken(leftExpr);
                if (prop == null)
                    target[leftExpr] = JObject.Parse("{}");
                else
                    if (!prop.HasValues) target[leftExpr] = JObject.Parse("{}");

                SetProperty(target[leftExpr] as JObject, string.Join(".", parts.Skip(1)), data);
            }
        }

        public bool SetData(string propertyPath, object data, bool updateDB = true)
        {
            JToken token = this.SelectToken(propertyPath);

            if (token != null)
            {
                SetValue(token, data);
            }
            else
            {
                SetProperty(this, propertyPath, data);
            }

            if (updateDB) UpdateDbModuleData();

            return true;
        }

        private void SetValue(JToken token, object data)
        {
            if (data == null)
                (token.Parent as JProperty).Value = null;
            else
            {
                var dataType = data.GetType();

                if (dataType == typeof(JValue))
                {
                    (token.Parent as JProperty).Value = data as JValue;
                }
                else if (TypeUtil.CheckDataTypeIsNonObject(dataType))
                {
                    (token.Parent as JProperty).Value = new JValue(data);
                }
                else if (dataType == typeof(JArray))
                {
                    (token.Parent as JProperty).Value = JArray.FromObject(data);
                }
                else if (dataType == typeof(JObject) || dataType.BaseType == typeof(object))
                {
                    (token.Parent as JProperty).Value = JObject.FromObject(data);
                }
            }
        }

        private static void UpdateJsonInternal(JToken source, string[] path, int pathIndex, JToken value)
        {
            if (pathIndex == path.Length - 1)
            {
                if (source is JArray)
                {
                    ((JArray)source)[int.Parse(path[pathIndex])] = value;
                }
                else if (source is JObject)
                {
                    ((JObject)source)[path[pathIndex]] = value;
                }
            }
            else if (source is JArray)
            {
                UpdateJsonInternal(((JArray)source)[int.Parse(path[pathIndex])], path, pathIndex + 1, value);
            }
            else if (source is JObject)
            {
                UpdateJsonInternal(((JObject)source)[path[pathIndex]], path, pathIndex + 1, value);
            }
        }

        public string GetPageParam(string paramName)
        {
            var value = this._pageParams != null && this._pageParams.ContainsKey(paramName) ? this._pageParams[paramName] : null;
            return value;
        }

        public void SetPageParam(string pageUrl)
        {
            if (!string.IsNullOrWhiteSpace(pageUrl))
                this._pageParams = new Uri(pageUrl).DecodeQueryParameters();
            //this._pageParams = !string.IsNullOrWhiteSpace(pageUrl) ? new Uri(pageUrl) : null;
        }

        public VariableScope GetScope(string variableName)
        {
            throw new NotImplementedException();
        }

        public bool IsVariable(string variableName)
        {
            return variableName == "_CurrentUser" || variableName == "Form" || variableName == "Field" || _moduleVariables.FirstOrDefault(v => v.VariableName == variableName) != null;
        }

        internal IEnumerable<ModuleVariableInfo> GetVariables(Guid? moduleID = null)
        {
            if (moduleID == null && this.ModuleID == null) return Enumerable.Empty<ModuleVariableInfo>();

            var result = ModuleVariableRepository.Instance.GetVariables(moduleID == null ? this.ModuleID.Value : moduleID.Value);
            return result;
        }

        private void populateViewModelProperties(JObject parent, Guid viewModelID)
        {
            var items = ViewModelPropertyRepository.Instance.GetProperties(viewModelID);
            foreach (var item in items)
            {
                var type = TypeUtil.GetSystemType(item.PropertyType);
                var defaultValue = TypeUtil.GetSystemTypeDefaultValue(type);

                parent.Add(item.PropertyName, JToken.FromObject(defaultValue));

                if (item.PropertyTypeID != null) populateViewModelProperties(parent[item.PropertyName] as JObject, item.PropertyTypeID.Value);
            }
        }

        private void UpdateDbModuleData()
        {
            if (this.ModuleID == null) return;

            var spParams = new Dictionary<string, object>();
            spParams.Add("@ConnectionID", this._connectionID);
            spParams.Add("@ModuleID", this.ModuleID);
            spParams.Add("@Data", this.ToString());

            DbUtil.ExecuteSp("[dbo].[BusinessEngine_SaveModuleData]", spParams);
        }

        private JObject GetDbModuleData(string connectionID = null, Guid? moduleID = null)
        {
            var spParams = new Dictionary<string, object>();
            spParams.Add("@ConnectionID", connectionID != null ? connectionID : this._connectionID);
            spParams.Add("@ModuleID", moduleID != null ? moduleID : this.ModuleID);

            var result = DbUtil.ExecuteScaler<string>("[dbo].[BusinessEngine_GetModuleData]", spParams);
            return JObject.Parse(result ?? "{}");
        }
    }
}


//public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//{
//    JObject jo = JObject.Load(reader);
//    object targetObj = Activator.CreateInstance(objectType);
//    foreach (PropertyInfo prop in objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
//    {
//        JsonPropertyAttribute att = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
//        string jsonPath = (att != null ? att.PropertyName : prop.Name);
//        JToken token = jo.SelectToken(jsonPath);
//        if (token != null && token.Type != JTokenType.Null)
//        {
//            object value = token.ToObject(prop.PropertyType, serializer);
//            prop.SetValue(targetObj, value, null);
//        }
//    }
//    return targetObj;
//}

//public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//{
//    JToken t = JToken.FromObject(value);

//    if (t.Type != JTokenType.Object)
//    {
//        t.WriteTo(writer);
//    }
//    else
//    {
//        JObject o = (JObject)t;
//        IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

//        o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

//        o.WriteTo(writer);
//    }
//}

//    public override void WriteJson(JsonWriter writer, object value,
//JsonSerializer serializer)
//    {
//        var properties = value.GetType().GetRuntimeProperties().Where(p => p.CanRead && p.CanWrite);
//        JObject main = new JObject();
//        foreach (PropertyInfo prop in properties)
//        {
//            JsonPropertyAttribute att = prop.GetCustomAttributes(true)
//                .OfType<JsonPropertyAttribute>()
//                .FirstOrDefault();

//            string jsonPath = (att != null ? att.PropertyName : prop.Name);
//            var nesting = jsonPath.Split(new[] { '.' });
//            JObject lastLevel = main;
//            for (int i = 0; i < nesting.Length; i++)
//            {
//                if (i == nesting.Length - 1)
//                {
//                    lastLevel[nesting[i]] = new JValue(prop.GetValue(value));
//                }
//                else
//                {
//                    if (lastLevel[nesting[i]] == null)
//                    {
//                        lastLevel[nesting[i]] = new JObject();
//                    }
//                    lastLevel = (JObject)lastLevel[nesting[i]];
//                }
//            }

//        }
//        serializer.Serialize(writer, main);
//    }

//public bool SetData(string propertyPath, object data)
//{
//    try
//    {
//        var val = data;
//        if (data.GetType() == typeof(string)) val = this._expressionService.ParseExpression(data.ToString(), this);

//        JObject property = this as JObject;

//        var path = propertyPath.Split('.');
//        var index = 0;
//        while (index < path.Length)
//        {
//            var propName = path[index];
//            if (property[propName] != null)
//            {
//                if (property[propName].Type == JTokenType.Object && index < (path.Length - 1)) property = property[propName] as JObject;

//                if (index == path.Length - 1)
//                {
//                    var dataType = data.GetType();

//                    if (dataType == typeof(JValue))
//                        property[propName] = data as JValue;
//                    else if (TypeUtil.CheckDataTypeIsNonObject(dataType))
//                        property[propName] = new JValue(val);
//                    else if (dataType == typeof(JArray))
//                    {
//                        property[propName] = JArray.FromObject(val);
//                    }
//                    else if (dataType == typeof(JObject) || dataType.BaseType == typeof(object))
//                    {
//                        property[propName] = JObject.FromObject(val);
//                    }
//                }
//            }

//            index++;
//        }

//        UpdateDbModuleData();

//        this._isUpdated = true;

//        return true;
//    }
//    catch (Exception ex)
//    {
//        return false;
//    }
//}
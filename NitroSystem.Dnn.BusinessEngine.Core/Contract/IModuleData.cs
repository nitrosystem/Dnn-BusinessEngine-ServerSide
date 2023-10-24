using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Contract
{
    public interface IModuleData
    {
        Guid ModuleID { get; set; }

        event EventHandler OnChangeModuleData;

        void InitModuleData(Guid moduleID, string connectionID, int userID, IDictionary<string, object> form, IDictionary<string, object> field, string pageUrl, bool clearData = false);

        void SetFieldItem(string fieldName, object field);

        void AddProperty(string propertyPath);

        bool SetData(string propertyPath, object data, bool updateDB = true);

        JObject GetModuleData(string connectionID, Guid? moduleID = null);

        object GetData(string propertyPath);

        string GetPageParam(string paramName);

        void SetPageParam(string pageUrl);

        bool IsVariable(string variableName);

        VariableScope GetScope(string variableName);
    }
}

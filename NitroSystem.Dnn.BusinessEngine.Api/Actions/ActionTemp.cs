using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NitroSystem.Dnn.BusinessEngine.Actions
{
    public class ActionTemp
    {
        public JObject _B { get; set; }

        public ActionTemp(string json)
        {
            this._B = JObject.Parse(json);
        }

        internal object GetPropertyValue(string propertyPath)
        {
            var token = this._B.SelectToken(propertyPath);
            return token.ToString();
        }
    }
}



//FileInfo sourceFile = new FileInfo(sourceName);

//String dllName = String.Format(@"{0}\{1}.dll",
//       System.Environment.CurrentDirectory,
//       sourceFile.Name.Replace(".", "_"));

//FileInfo fi = new FileInfo(dllName);
//CSharpCodeProvider provider = new CSharpCodeProvider();
//CompilerParameters compilerparams = new CompilerParameters();

//compilerparams.OutputAssembly = dllName;
//compilerparams.GenerateExecutable = false; ;

//CompilerResults results = provider.CompileAssemblyFromFile(compilerparams, sourceName);
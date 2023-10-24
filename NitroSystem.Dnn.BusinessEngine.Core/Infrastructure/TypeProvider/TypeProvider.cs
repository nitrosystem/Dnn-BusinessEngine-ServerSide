using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.TypeProvider
{
    public class TypeProvider
    {
        public static bool CompileExecutable(String sourceName)
        {
            FileInfo sourceFile = new FileInfo(sourceName);

            String dllName = String.Format(@"{0}\{1}.dll",
                   System.Environment.CurrentDirectory,
                   sourceFile.Name.Replace(".", "_"));

            FileInfo fi = new FileInfo(dllName);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compilerparams = new CompilerParameters();

            compilerparams.OutputAssembly = dllName;
            compilerparams.GenerateExecutable = false; ;

            CompilerResults results = provider.CompileAssemblyFromFile(compilerparams, sourceName);

            var a = results.Errors;

            return true;
        }
    }
}

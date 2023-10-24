using System;
using System.CodeDom.Compiler;
using System.Data;
using System.Reflection;
using DynamicExpresso;
using Microsoft.JScript;

namespace NitroSystem.Dnn.BusinessEngine.Core.ModuleBuilder
{
    public static class Evaluator
    {
        private static Interpreter interpreter = new Interpreter();
        private static object _evaluator = null;
        private static Type _evaluatorType = null;
        private static readonly string _jscriptSource =
          @"package Evaluator
            {
               class Evaluator
               {
                  public function Eval(expr : String) : String 
                  { 
                     return eval(expr); 
                  }
               }
            }";

        [Obsolete]
        static Evaluator()
        {
            Func<int, int, int> random = (x, y) => new Random().Next(x, y);
            Func<double, double, double> pow = (x, y) => Math.Pow(x, y);
            Func<object, object, object> isNull = (x, y) =>
            {
                if (x == null || x == "")
                    return y;
                else
                    return x;
            };

            interpreter.SetFunction("Random", random);
            interpreter.SetFunction("Pow", pow);
            interpreter.SetFunction("IsNull", isNull);

            ICodeCompiler compiler;
            compiler = new JScriptCodeProvider().CreateCompiler();

            CompilerParameters parameters;
            parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            CompilerResults results;
            results = compiler.CompileAssemblyFromSource(parameters, _jscriptSource);

            Assembly assembly = results.CompiledAssembly;
            _evaluatorType = assembly.GetType("Evaluator.Evaluator");

            _evaluator = Activator.CreateInstance(_evaluatorType);
        }

        public static T Eval<T>(string statement)
        {
            try
            {
                var target = new Interpreter();
                var result = target.Eval<T>(statement);

                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static int EvalToInteger(string statement)
        {
            string s = EvalToString(statement);
            return int.Parse(s.ToString());
        }

        public static double EvalToDouble(string statement)
        {
            string s = EvalToString(statement);
            return double.Parse(s);
        }

        public static string EvalToString(string statement)
        {
            object o = EvalToObject(statement);
            return o == "undefined" ? "" : o.ToString();
        }

        public static object EvalToObject(string statement)
        {
            return _evaluatorType.InvokeMember(
                        "Eval",
                        BindingFlags.InvokeMethod,
                        null,
                        _evaluator,
                        new object[] { statement }
                     );
        }

        public static object EvalCustomMethod(string statement)
        {
            var result = interpreter.Eval(statement);

            return result;
        }

        public static object Eval(string expression)
        {
            DataTable dt = new DataTable();
            var value = dt.Compute(expression, "");
            return value != null ? value : string.Empty;
        }
    }
}

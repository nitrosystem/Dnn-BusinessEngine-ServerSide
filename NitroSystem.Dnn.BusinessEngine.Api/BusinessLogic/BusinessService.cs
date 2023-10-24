using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Common;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.BusinessLogic
{
    internal static class BusinessService
    {
        
    }
}

//class Program
//{
//    static void Main(string[] args)
//    {
//        var p = "2,3,9/25/2021";

//        var DLL = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "ClassLibrarySharepointCSOM.dll");

//        var theType = DLL.GetType("ClassLibrarySharepointCSOM.TaxFraudSpCsom");

//        var c = Activator.CreateInstance(theType,);

//        var parameterTypes = p.Split(',').ToList().ConvertAll(a => a.GetType()).ToArray();

//        var method = theType.GetMethod("Post");//, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, parameterTypes, null);

//        ParameterInfo[] Myarray = method.GetParameters();
//        int index = 0;
//        List<object> pp = new List<object>();
//        var ppp = p.Split(',');
//        foreach (var v in Myarray)
//        {
//            var vv = Convert.ChangeType(ppp[index++], v.ParameterType);
//            pp.Add(vv);
//        }

//        //theType.GetProperty("p1").SetValue(anonymParameter, "");

//        var result = method.Invoke(c, pp.ToArray());

//        Console.WriteLine(result);

//        Console.ReadLine();

//    }
//}



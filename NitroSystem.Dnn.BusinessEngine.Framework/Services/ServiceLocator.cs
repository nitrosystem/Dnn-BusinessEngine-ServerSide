using NitroSystem.Dnn.BusinessEngine.Framework.Contracts;
using NitroSystem.Dnn.BusinessEngine.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Framework.Services
{
    internal static class ServiceLocator<T> where T : class
    {
        private static T _instance;

        internal static void Init(string typeName, params object[] constructor)
        {
            try
            {
                _instance = Activator.CreateInstance(Type.GetType(typeName), constructor) as T;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal static T CreateInstance(string typeName, params object[] constructor)
        {
            if (_instance == null || typeName.IndexOf(_instance.GetType().FullName) == -1) Init(typeName, constructor);

            return _instance;
        }
    }
}

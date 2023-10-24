using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common
{
    public static class TypeUtil
    {
        public static Type GetSystemType(string typeName)
        {
            typeName = typeName ?? string.Empty;

            switch (typeName.ToLower())
            {
                case "object":
                    return typeof(object);
                case "string":
                    return typeof(string);
                case "bool":
                    return typeof(bool);
                case "byte":
                    return typeof(byte);
                case "char":
                    return typeof(char);
                case "decimal":
                    return typeof(decimal);
                case "double":
                    return typeof(double);
                case "short":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "sbyte":
                    return typeof(sbyte);
                case "float":
                    return typeof(float);
                case "ushort":
                    return typeof(ushort);
                case "uint":
                    return typeof(uint);
                case "ulong ":
                    return typeof(ulong);
                case "datetime":
                    return typeof(DateTime);
                case "timespan":
                    return typeof(TimeSpan);
                case "guid":
                    return typeof(Guid);
                default:
                    return typeof(object);
                    //return Type.GetType(typeName);
            }
        }

        public static bool CheckDataTypeIsNonObject(Type dataType)
        {
            if (dataType == typeof(string)) return true;
            else if (dataType == typeof(bool)) return true;
            else if (dataType == typeof(byte)) return true;
            else if (dataType == typeof(char)) return true;
            else if (dataType == typeof(decimal)) return true;
            else if (dataType == typeof(double)) return true;
            else if (dataType == typeof(short)) return true;
            else if (dataType == typeof(int)) return true;
            else if (dataType == typeof(long)) return true;
            else if (dataType == typeof(sbyte)) return true;
            else if (dataType == typeof(float)) return true;
            else if (dataType == typeof(ushort)) return true;
            else if (dataType == typeof(uint)) return true;
            else if (dataType == typeof(ulong)) return true;
            else if (dataType == typeof(DateTime)) return true;
            else if (dataType == typeof(TimeSpan)) return true;
            else if (dataType == typeof(Guid)) return true;
            else return false;
        }

        public static object GetSystemTypeDefaultValue(Type type)
        {
            if (type == null) return new object();

            switch (type.Name.ToLower())
            {
                case "object":
                    return new System.Object();
                case "string":
                    return string.Empty;
                case "bool":
                    return new System.Boolean();
                case "byte":
                    return new System.Byte();
                case "char":
                    return new System.Char();
                case "decimal":
                    return new System.Decimal();
                case "double":
                    return new System.Double();
                case "short":
                    return new System.Int16();
                case "int":
                    return new System.Int32();
                case "long":
                    return new System.Int64();
                case "sbyte":
                    return new System.SByte();
                case "float":
                    return new System.Double();
                case "datetime":
                    return new System.DateTime();
                case "timespan":
                    return new System.TimeSpan();
                case "guid":
                    return new System.Guid();
                default:
                    return Activator.CreateInstance(type);
            }
        }

        public static void FirePublicEvent(object onMe, string invokeMe, params object[] eventParams)
        {
            MulticastDelegate eventDelagate =
                  (MulticastDelegate)onMe.GetType().GetField(invokeMe,
                   System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.Public).GetValue(onMe);

            Delegate[] delegates = eventDelagate.GetInvocationList();

            foreach (Delegate dlg in delegates)
            {
                dlg.Method.Invoke(dlg.Target, eventParams);
            }
        }
    }
}

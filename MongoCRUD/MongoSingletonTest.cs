using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class MongoSingletonTest
    {
        private static readonly Dictionary<Type, object> TypeList = new Dictionary<Type, object>();

        public static void UseSingleton<T>() where T : new()
        {
            var type = typeof(T);
            if (TypeList.ContainsKey(type))
            {
                throw new DuplicateNameException(type.Name);
            }
            TypeList.Add(type, null);
        }

        public static bool IsTypeExists<T>()
        {
            return TypeList.ContainsKey(typeof(T));
        }
        public static void ResetSingleton<T>()
        {
            var type = typeof(T);
            if (TypeList.ContainsKey(type))
            {
                TypeList[type] = null;
            }
            else
            {
                throw new KeyNotFoundException("No registered type found on " + type.Name);
            }
        }
        public static void RemoveSingleton<T>()
        {
            TypeList.Remove(typeof(T));
        }
        public static T GetSingleton<T>()
        {
            var type = typeof(T);
            if (TypeList[type] == null)
            {
                var obj = (T)Activator.CreateInstance(type);
                TypeList[type] = obj;
                return obj;
            }

            return (T)TypeList[type];
        }
    }
}

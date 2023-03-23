using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SuperShortLink.Helpers
{
    internal static class ReflectionHelper
    {
        public static MethodInfo GetPrivateMethod(this Type t, string methodName)
        {
            return t.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        public static MethodInfo GetPrivateStaticMethod(this Type t, string methodName)
        {
            return t.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        public static bool IsSubclassOfTypeByName(this Type t, string typeName)
        {
            while (t != null)
            {
                if (t.Name == typeName)
                    return true;
                t = t.GetTypeInfo().BaseType;
            }

            return false;
        }
    }
}

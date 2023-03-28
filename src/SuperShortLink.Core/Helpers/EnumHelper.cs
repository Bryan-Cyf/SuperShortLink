using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SuperShortLink.Helpers
{
    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumCache = new ConcurrentDictionary<Enum, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> EnumAllCache = new ConcurrentDictionary<Type, Dictionary<int, string>>();

        
        /// <summary>
        /// 把枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>以枚举值为key,枚举文本为value的键值对集合</returns>
        public static Dictionary<int, string> EnumToDictionary(Type enumType)
        {
            if (!EnumAllCache.ContainsKey(enumType))
            {
                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("传入的参数必须是枚举类型！", nameof(enumType));
                }

                Dictionary<int, string> enumDic = new Dictionary<int, string>();
                Array enumValues = Enum.GetValues(enumType);
                foreach (Enum enumValue in enumValues)
                {
                    int key = Convert.ToInt32(enumValue);
                    string value = enumValue.GetDescription();
                    enumDic.Add(key, value);
                }

                EnumAllCache[enumType] = enumDic;
            }
            return EnumAllCache[enumType];
        }

        /// <summary>
        /// 扩展方法,获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute,是否使用枚举名代替,默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, bool nameInstead = true)
        {
            if (!EnumCache.ContainsKey(value))
            {
                Type type = value.GetType();
                string name = Enum.GetName(type, value);
                if (name == null)
                {
                    EnumCache[value] = null;
                }
                else
                {
                    FieldInfo field = type.GetField(name);
                    DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute == null && nameInstead)
                    {
                        EnumCache[value] = name;
                        return name;
                    }
                    EnumCache[value] = attribute?.Description;
                }
            }
            return EnumCache[value];
        }


        /// <summary>
        /// 枚举描述
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static String GetEnumDesc(Enum e)
        {
            FieldInfo fieldInfo = e.GetType().GetField(e.ToString());
            if (fieldInfo != null)
            {
                DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    return EnumAttributes[0].Description;
                }
            }
            return e.ToString();
        }
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="en">枚举</param>
        /// <returns>返回枚举的描述</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();   //获取类型
            MemberInfo[] memberInfos = type.GetMember(en.ToString());   //获取成员
            if (memberInfos != null && memberInfos.Length > 0)
            {
                if (memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attrs && attrs.Length > 0)
                {
                    return attrs[0].Description;    //返回当前描述
                }
            }
            return en.ToString();
        }
        /// <summary>
        /// 枚举的描述和Value转为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> EnumValueAndDescriptionToDictionary<T>()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            if (typeof(T) == typeof(Enum))
            {
                throw new ArgumentOutOfRangeException("T只能是Enum类型");
            }

            Type enumType = typeof(T);

            foreach (string key in Enum.GetNames(enumType))
            {
                int val = (int)enumType.GetField(key).GetValue(null);

                FieldInfo finfo = enumType.GetField(key);
                object[] cAttr = finfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (cAttr.Length > 0)
                {
                    DescriptionAttribute desc = cAttr[0] as DescriptionAttribute;
                    if (desc != null)
                    {
                        dic[val] = desc.Description;
                    }
                }
            }

            return dic;
        }
        public static List<int> EnumValue<T>()
        {
            List<int> dic = new List<int>();

            if (typeof(T) == typeof(Enum))
            {
                throw new ArgumentOutOfRangeException("T只能是Enum类型");
            }

            Type enumType = typeof(T);

            foreach (string key in Enum.GetNames(enumType))
            {
                int val = (int)enumType.GetField(key).GetValue(null);

                dic.Add(val);
            }

            return dic;
        }
    }
}

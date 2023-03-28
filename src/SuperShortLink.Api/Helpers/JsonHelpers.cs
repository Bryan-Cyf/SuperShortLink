using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShortLink.Api.Helpers
{

    internal static class JsonExtension
    {
        public static string ToJsonString(this object obj)
        {
            IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
            isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            if (obj != null)
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented, isoDateTimeConverter);
            }

            return "";
        }

        public static T ToObj<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToEntity<T>(this object json)
        {
            return json.ToJsonString().ToObj<T>();
        }
    }
}

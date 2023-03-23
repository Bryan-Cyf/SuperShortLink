using SuperShortLink.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SuperShortLink.Cache
{
    internal class CacheEntry
    {
        private object _cacheValue;
        private static long _instanceCount;

        public CacheEntry(string key, object value)
        {
            Key = key;
            Value = value;
            LastModifiedTicks = DateTime.Now.Ticks;
            InstanceNumber = Interlocked.Increment(ref _instanceCount);
        }

        internal string Key { get; private set; }
        internal long InstanceNumber { get; private set; }
        internal long LastAccessTicks { get; private set; }
        internal long LastModifiedTicks { get; private set; }

        internal object Value
        {
            get
            {
                LastAccessTicks = DateTime.Now.Ticks;
                return _cacheValue;
            }
            set
            {
                _cacheValue = value;
                LastAccessTicks = DateTime.Now.Ticks;
                LastModifiedTicks = DateTime.Now.Ticks;
            }
        }

        public T GetValue<T>(bool isDeepClone = true)
        {
            object val = Value;

            var t = typeof(T);

            if (t == TypeHelper.BoolType || t == TypeHelper.StringType || t == TypeHelper.CharType || t == TypeHelper.DateTimeType || t.IsNumeric())
                return (T)Convert.ChangeType(val, t);

            if (t == TypeHelper.NullableBoolType || t == TypeHelper.NullableCharType || t == TypeHelper.NullableDateTimeType || t.IsNullableNumeric())
                return val == null ? default(T) : (T)Convert.ChangeType(val, Nullable.GetUnderlyingType(t));

            return isDeepClone
                ? DeepClonerGenerator.CloneObject<T>((T)val)
                : (T)val;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShortLink.Cache
{
    public interface IMemoryCaching
    {
        int GetCount(string prefix = "");

        CacheValue<T> Get<T>(string key);

        bool Set<T>(string key, T value);

        bool Exists(string key);

        int RemoveAll(IEnumerable<string> keys = null);

        bool Remove(string key);
    }
}

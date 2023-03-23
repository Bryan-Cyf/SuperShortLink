using Microsoft.Extensions.Options;
using SuperShortLink.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperShortLink.Cache
{
    public class MemoryCaching : IMemoryCaching
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _memory;
        private ShortLinkOptions _options;

        public MemoryCaching(IOptionsMonitor<ShortLinkOptions> options)
        {
            _options = options.CurrentValue;
            _memory = new ConcurrentDictionary<string, CacheEntry>();
            options.OnChange(x =>
            {
                _options = x;
            });
        }

        private long _cacheSize = 0L;
        private const string _UPTOLIMIT_KEY = "inter_up_to_limit_key";


        public CacheValue<T> Get<T>(string key)
        {

            if (!_memory.TryGetValue(key, out var cacheEntry))
            {
                return CacheValue<T>.NoValue;
            }
            try
            {
                var value = cacheEntry.GetValue<T>();
                return new CacheValue<T>(value, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"some error herer, message = {ex.Message}");
                return CacheValue<T>.NoValue;
            }
        }

        public bool Set<T>(string key, T value)
        {
            var entry = new CacheEntry(key, value);
            CacheEntry deep = null;
            try
            {
                deep = DeepClonerGenerator.CloneObject(entry);
            }
            catch (Exception)
            {
                deep = entry;
            }

            _memory.AddOrUpdate(deep.Key, deep, (k, cacheEntry) => deep);

            StartScanForOutOfScopeItems();

            if (_options.CacheCountLimit.HasValue)
                Interlocked.Increment(ref _cacheSize);

            return true;
        }

        private void StartScanForOutOfScopeItems()
        {
            if (_options.CacheCountLimit.HasValue && Interlocked.Read(ref _cacheSize) >= _options.CacheCountLimit)
            {
                if (_memory.TryAdd(_UPTOLIMIT_KEY, new CacheEntry(_UPTOLIMIT_KEY, 1)))
                {
                    Task.Factory.StartNew(state => ScanForOutOfScopeItems((MemoryCaching)state), this,
                        CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
                }
            }
        }


        private void ScanForOutOfScopeItems(MemoryCaching cache)
        {
            try
            {
                var shouldRemoveCount = 5;

                if (_options.CacheCountLimit.Value >= 10000)
                {
                    shouldRemoveCount = (int)(_options.CacheCountLimit * 0.005d);
                }
                else if (_options.CacheCountLimit.Value >= 1000 && _options.CacheCountLimit.Value < 10000)
                {
                    shouldRemoveCount = (int)(_options.CacheCountLimit * 0.01d);
                }

                var oldestList = cache._memory.ToArray()
                                .OrderBy(kvp => kvp.Value.LastAccessTicks)
                                .ThenBy(kvp => kvp.Value.InstanceNumber)
                                .Take(shouldRemoveCount)
                                .Select(kvp => kvp.Key);

                RemoveAll(oldestList);
            }
            catch (Exception)
            {

            }
            finally
            {
                cache._memory.TryRemove(_UPTOLIMIT_KEY, out _);
            }
        }
        public bool Exists(string key)
        {
            return _memory.TryGetValue(key, out var entry);
        }

        public int RemoveAll(IEnumerable<string> keys = null)
        {
            if (keys == null)
            {
                if (_options.CacheCountLimit.HasValue)
                {
                    int count = (int)Interlocked.Read(ref _cacheSize);
                    Interlocked.Exchange(ref _cacheSize, 0);
                    _memory.Clear();
                    return count;
                }
                else
                {
                    int count = _memory.Count;
                    _memory.Clear();
                    return count;
                }
            }

            int removed = 0;
            foreach (string key in keys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                if (_memory.TryRemove(key, out _))
                {
                    removed++;
                    if (_options.CacheCountLimit.HasValue)
                        Interlocked.Decrement(ref _cacheSize);
                }
            }

            return removed;
        }

        public bool Remove(string key)
        {
            bool flag = _memory.TryRemove(key, out _);

            if (_options.CacheCountLimit.HasValue && !key.Equals(_UPTOLIMIT_KEY) && flag)
            {
                Interlocked.Decrement(ref _cacheSize);
            }

            return flag;
        }

        public int GetCount(string prefix = "")
        {
            return string.IsNullOrWhiteSpace(prefix)
                ? _memory.Count
                : _memory.Count(x => x.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }
    }
}

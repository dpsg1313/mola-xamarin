using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MolaApp
{
    public class ServiceContainer
    {
        ConcurrentDictionary<string, Object> _dict = new ConcurrentDictionary<string, Object>();

        public void Add(string handle, Object service)
        {
            _dict.AddOrUpdate(handle, service, (key, oldValue) => service);
        }

        public T Get<T>(string handle)
        {
            if (_dict.ContainsKey(handle))
            {
                Object service;
                if (_dict.TryGetValue(handle, out service) && service is T)
                {
                    return (T)service;
                }
            }
            throw new Exception("Service not available");
        }
    }
}

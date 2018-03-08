using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCallASPAPI.HelperClass
{
    public static class Mediator
    {
        static IDictionary<string, List<Action<Object>>> Dictionary =
            new Dictionary<string, List<Action<Object>>>();

        static public void Registry(string key, Action<object> CallMethod)
        {
            if (!Dictionary.ContainsKey(key))
            {
                var list = new List<Action<Object>>();
                list.Add(CallMethod);
                Dictionary.Add(key, list);
            }
            else
            {
                bool MethodTrue = false;
                foreach (var item in Dictionary[key])
                    if (item.Method.ToString() == CallMethod.Method.ToString())
                        MethodTrue = true;

                if (!MethodTrue)
                    Dictionary[key].Add(CallMethod);
            }
        }

        static public void UnRegistry(string key, Action<Object> CallMethod)
        {
            if (Dictionary.ContainsKey(key))
                Dictionary[key].Remove(CallMethod);
        }

        static public void Notify(string key, object args)
        {
            if (Dictionary.ContainsKey(key))
                foreach (var item in Dictionary[key])
                    item(args);
        }
    }
}

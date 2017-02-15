using EasyOpen.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyOpen
{
    public class ExecuteCommon
    {
        public object doMethod(string Key, params object[] par)
        {
            object result = false;

            Type[] type = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var item in type)
            {
                if (!item.IsInterface && item.GetMethod(Key) != null)
                {
                    MethodInfo name = item.GetMethod(Key);

                    object obeject = Activator.CreateInstance(item);

                    if (par == null)
                    {
                        result = name.Invoke(obeject, null);
                    }
                    else
                    {
                        result = name.Invoke(obeject, new object[] { par });
                    }
                    break;
                }
            }
            return result;
        }
    }
}

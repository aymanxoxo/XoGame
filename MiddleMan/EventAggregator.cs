using System;
using System.Collections.Generic;
using System.Linq;

namespace MiddleMan
{
    public class EventAggregator {
        private static readonly List<IMyEvent> MyList = new List<IMyEvent>();  
        public static T Resolve<T>() where T : IMyEvent
        {
            lock (MyList)
            {
                if (typeof(T).IsAbstract || typeof(T).IsInterface) return default(T);
                var result = (T)MyList.FirstOrDefault(x => x is T);
                if (result == null)
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                    //MyList.Add(result);
                }
                return result;
            }
        }

        public static void Register(IMyEvent myEvent)
        {
            if (myEvent == null) throw new ArgumentNullException(nameof(myEvent), "Can't register null object.");
            if(!MyList.Contains(myEvent))
                MyList.Add(myEvent);
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiddleMan
{
    public abstract class MyEvent<T> : IMyEvent where T : class
    {
        //private readonly Dictionary<Action<T>, bool> _subscribers = new Dictionary<Action<T>, bool>(); 
        private readonly List<Helper<T>> _subscribers = new List<Helper<T>>();

        protected MyEvent()
        {
            EventAggregator.Register(this);
        }

        public void Publish(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), "Can't publish null objects");
            lock (_subscribers)
            {
                foreach (var subscriber in _subscribers)
                {
                    if (subscriber.Active)
                        subscriber.Action(obj);
                }
                ClearInActiveSubscribers();
            }
        }

        private void ClearInActiveSubscribers()
        {
            _subscribers.RemoveAll(helper => !helper.Active);
        }

        public void Subscribe(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            lock (_subscribers)
            {
                var sub = _subscribers.FirstOrDefault(x => x.Action == action);
                if (sub == null)
                {
                    _subscribers.Add(new Helper<T> { Action = action, Active = true });
                }
            }
        }

        public void UnSubscribe(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            lock (_subscribers)
            {
                var sub = _subscribers.FirstOrDefault(x => x.Action == action);
                if (sub != null)
                    sub.Active = false;
            }
        }
    }

    public class Helper<T>
    {
        public Action<T> Action { get; set; }
        public bool Active { get; set; }
    }
}
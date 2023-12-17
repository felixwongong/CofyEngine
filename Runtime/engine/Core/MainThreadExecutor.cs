using System;
using System.Collections.Generic;
using CofyEngine;

namespace CofyEngine.Core
{
    public class MainThreadExecutor : MonoInstance<MainThreadExecutor>
    {
        public override bool persistent => true;

        private Queue<Action> _actionQueue;
        
        protected override void Awake()
        {
            base.Awake();
            _actionQueue = new Queue<Action>();
        }

        public void OnUpdate()
        {
            while (_actionQueue.Count > 0)
            {
                try
                {
                    var action = _actionQueue.Dequeue();
                    action();
                }
                catch (Exception ex)
                {
                    FLog.LogException(new Exception("Exception occurs in Main Thread", ex));
                }
            }
        }

        public void QueueAction(in Action action)
        {
            _actionQueue.Enqueue(action);
        }
    }

    public static class ThreadExtension
    {
        public static Future<T> executeInMainThread<T>(this Future<T> future)
        {
            Promise<T> promise = new Promise<T>(() => future.progress);
            future.OnCompleted(validation =>
            {
                if (validation.hasException)
                {
                    promise.Reject(validation.target.ex);
                }
                else
                {
                    MainThreadExecutor.instance.QueueAction(() => promise.Resolve(validation.target.result));
                }
            });

            return promise.future;
        }
    }
}
﻿using System;
using System.Collections.Generic;

namespace CofyEngine.Core
{
    public class MainThreadExecutor : MonoInstance<MainThreadExecutor>, IDisposable
    {
        public override bool persistent => true;

        private Queue<Action> _actionQueue;
        private List<Action> _actionPersistent;
        
        protected override void Awake()
        {
            base.Awake();
            _actionQueue = new Queue<Action>();
            _actionPersistent = new List<Action>();
        }

        public void OnUpdate()
        {
            for (var i = 0; i < _actionPersistent.Count; i++)
            {
                _actionPersistent[i]();
            }

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
        
        public void QueueUpdate(in Action action)
        {
            _actionPersistent.Add(action);
        }

        public void Dispose()
        {
            _actionQueue = null;
            _actionPersistent = null;
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
                    promise.Reject(validation.ex);
                }
                else
                {
                    MainThreadExecutor.instance.QueueAction(() => promise.Resolve(validation.result));
                }
            });

            return promise.future;
        }
    }
}
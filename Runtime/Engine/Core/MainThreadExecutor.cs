using System;
using System.Collections.Generic;
using Engine.Util;

namespace CofyEngine.Engine.Core
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

        private void Update()
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

        public void QueueAction(Action action)
        {
            _actionQueue.Enqueue(action);
        }
    }
}
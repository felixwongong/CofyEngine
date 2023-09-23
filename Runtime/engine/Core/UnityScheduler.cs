using System;
using Engine.Util;
using UnityEngine;

namespace CofyEngine.Engine
{
    public class UnityScheduler: MonoInstance<UnityScheduler>
    {
        private SortedSeq<ScheduledTask> _tasks;

        private CtorObjectPool<ScheduledTask> _pool;

        protected override void Awake()
        {
            base.Awake();
            _tasks = new SortedSeq<ScheduledTask>(task => task.endTime);
            _pool = new CtorObjectPool<ScheduledTask>();
        }

        private void Update()
        {
            for (var i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];
                
                if (Time.timeAsDouble * 1000 > task.endTime)
                {
                    task.taskAction();
                    _pool.Recycle(task);
                    _tasks.RemoveAt(i);
                }
            }
        }

        public void AddDelay(double ms, Action task)
        {
            var invokeMS = Time.timeAsDouble * 1000 + ms;
            _tasks.Add(_pool.Get().Set(invokeMS, task));
        }
    }

    public class ScheduledTask
    {
        public double endTime;
        public Action taskAction;

        public ScheduledTask() { }

        public ScheduledTask Set(double endTime, Action taskAction)
        {
            this.endTime = endTime;
            this.taskAction = taskAction;
            return this;
        }
    }
}
using UnityEngine;

namespace Project.Runtime.Entity.Ai.BehaviorTree
{
    public class WaitNode : ActionNode
    {
        public float time = 1f;
        private float startedTime;

        protected override void OnStart()
        {
            startedTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return Time.time - startedTime > time ? State.Success : State.Running;
        }
    }
}
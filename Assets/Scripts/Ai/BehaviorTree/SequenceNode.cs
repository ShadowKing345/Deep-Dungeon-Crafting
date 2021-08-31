using System;

namespace Ai.BehaviorTree
{
    public class SequenceNode : CompositeNode
    {
        public int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        { }

        protected override State OnUpdate()
        {
            var child = children[current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return current == children.Count ? State.Success : State.Running;
        }
    }
}
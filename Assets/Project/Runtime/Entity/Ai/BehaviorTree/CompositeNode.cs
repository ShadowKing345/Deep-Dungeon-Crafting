using System.Collections.Generic;

namespace Project.Runtime.Entity.Ai.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new();
    }
}
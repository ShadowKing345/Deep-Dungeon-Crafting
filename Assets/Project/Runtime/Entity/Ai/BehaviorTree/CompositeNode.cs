using System.Collections.Generic;

namespace Entity.Ai.BehaviorTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> children = new();
    }
}
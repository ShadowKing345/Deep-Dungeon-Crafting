using System.Collections.Generic;

namespace Ai.BehaviorTree
{
    public abstract class CompositeNode: Node
    {
        public List<Node> children = new List<Node>();
    }
}
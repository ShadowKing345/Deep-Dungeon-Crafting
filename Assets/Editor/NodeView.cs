using Ai.BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Node = Ai.BehaviorTree.Node;

namespace Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node node;
        public Port input;
        public Port output;
        
        public NodeView(Node node)
        {
            this.node = node;
            title = node.name;
            viewDataKey = node.guid;
            
            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }
        
        private void CreateInputPorts()
        {
            input = node switch
            {
                ActionNode _ => InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)),
                CompositeNode _ => InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)),
                DecoratorNode _ => InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)),
                _ => input
            };

            if (input == null) return;
            
            input.portName = "";
            inputContainer.Add(input);
        }
        
        private void CreateOutputPorts()
        {
            output = node switch
            {
                CompositeNode _ => InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                    typeof(bool)),
                DecoratorNode _ => InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                    typeof(bool)),
                _ => output
            };
            
            if (output == null) return;
            
            output.portName = "";
            outputContainer.Add(output);
        }
        
        

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.position.x = newPos.x;
            node.position.y = newPos.y;
        }
    }
}
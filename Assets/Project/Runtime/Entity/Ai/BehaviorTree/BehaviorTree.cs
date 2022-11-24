using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.Runtime.Entity.Ai.BehaviorTree
{
    [CreateAssetMenu(fileName = "Behavior Tree", menuName = "SO/Behavior Tree")]
    public class BehaviorTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;

        public List<Node> nodes = new();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
                treeState = rootNode.Update();
            return treeState;
        }

        public Node CreateNode(Type type)
        {
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            switch (parent)
            {
                case DecoratorNode decoratorNode:
                    decoratorNode.child = child;
                    break;
                case CompositeNode compositeNode:
                    compositeNode.children.Add(child);
                    break;
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            switch (parent)
            {
                case DecoratorNode decoratorNode:
                    decoratorNode.child = null;
                    break;
                case CompositeNode compositeNode:
                    compositeNode.children.Remove(child);
                    break;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent switch
            {
                DecoratorNode decoratorNode => new List<Node>(new[] {decoratorNode.child}),
                CompositeNode compositeNode => compositeNode.children,
                _ => new List<Node>()
            };
        }
    }
}
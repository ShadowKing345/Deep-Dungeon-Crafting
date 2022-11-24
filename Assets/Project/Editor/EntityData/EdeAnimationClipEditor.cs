using System;
using Project.Runtime.Entity.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Editor.EntityData
{
    public class EdeAnimationClipEditor : VisualElement
    {
        private static readonly Type AnimationClipType = typeof(AnimationClip);

        private readonly EntityAnimation entityAnimation;

        private readonly VisualElement directional;
        private readonly VisualElement directionless;

        public EdeAnimationClipEditor(EntityAnimation entityAnimation)
        {
            this.entityAnimation = entityAnimation;

            var toggle = new Toggle { label = "Use Direction" };
            toggle.RegisterValueChangedCallback(callback => ToggleDirection(callback.newValue));
            directional = new VisualElement();
            directionless = new VisualElement();

            Add(toggle);
            Add(directionless);
            Add(directional);

            AddElements();

            ToggleDirection(entityAnimation.UseDirection);
        }

        private void AddElements()
        {
            var clip = new ObjectField
                { label = "Directionless Clip", objectType = AnimationClipType, value = entityAnimation.Clip };

            directionless.Add(clip);

            var north = new ObjectField
                { label = "North Clip", objectType = AnimationClipType, value = entityAnimation.North };
            var south = new ObjectField
                { label = "South Clip", objectType = AnimationClipType, value = entityAnimation.South };
            var east = new ObjectField
                { label = "East Clip", objectType = AnimationClipType, value = entityAnimation.East };
            var west = new ObjectField
                { label = "West Clip", objectType = AnimationClipType, value = entityAnimation.West };

            directional.Add(north);
            directional.Add(south);
            directional.Add(east);
            directional.Add(west);
        }

        private void ToggleDirection(bool v)
        {
            directional.style.display = v ? DisplayStyle.Flex : DisplayStyle.None;
            directionless.style.display = !v ? DisplayStyle.Flex : DisplayStyle.None;
            entityAnimation.UseDirection = v;
        }
    }
}
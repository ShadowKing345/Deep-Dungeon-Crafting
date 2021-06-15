using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Room
{
    public class FloorTile : MonoBehaviour, IRoomTile
    {
        public Sprite[] textures;
        public SpriteRenderer spriteRenderer;

        private void Start()
        {
            UpdateLook();
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
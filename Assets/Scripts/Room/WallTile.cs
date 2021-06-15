using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Room
{
    public class WallTile : MonoBehaviour, IRoomTile, IDirectional
    {
        [SerializeField] private DirectionObj<Sprite>[] textures;
        public Direction direction;
        
        public SpriteRenderer spriteRenderer;

        private void Start()
        {
            UpdateLook();
        }


        public void UpdateLook() => spriteRenderer.sprite = textures[Random.Range(0, textures.Length)].GetDirection((int) direction);
        public void SetDirection(Direction direction) => this.direction = direction;
    }
}
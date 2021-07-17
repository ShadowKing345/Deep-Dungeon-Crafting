using Interfaces;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Board.FloorTiles
{
    public class WallTile : MonoBehaviour, IRoomTile, IDirectional
    {
        [SerializeField] private DirectionObj<Sprite>[] textures;
        public Direction direction;
        
        public SpriteRenderer spriteRenderer;

        private void OnEnable() => UpdateLook();
        
        public void UpdateLook() => spriteRenderer.sprite = textures[Random.Range(0, textures.Length)].GetDirection(direction);
        public Direction Direction
        {
            set => direction = value;
        }
    }
}
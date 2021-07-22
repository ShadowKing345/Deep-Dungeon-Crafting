using Interfaces;
using UnityEngine;
using Utils;

namespace Board.Tiles
{
    public class CornerTile: MonoBehaviour, ITile, IDirectional
    {
        public DirectionalObj<Sprite>[] textures;
        public Direction direction;

        public SpriteRenderer spriteRenderer;
        
        private void OnEnable() => UpdateLook();

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)]
                .GetDirection(direction.GetDirectionFromInterCardinal());
        }

        public Direction Direction
        {
            set => direction = value;
        }
    }
}
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Board.Tiles
{
    public class CornerTile : MonoBehaviour, ITile, IDirectional
    {
        public DirectionalObj<Sprite>[] textures;
        public Direction direction;

        public SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            UpdateLook();
        }

        public Direction Direction
        {
            set => direction = value;
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)]
                .GetDirection(direction.GetDirectionFromInterCardinal());
        }
    }
}
using UnityEngine;
using Utils;

namespace Room
{
    public class CornerTile: MonoBehaviour, IRoomTile, IDirectional
    {
        public DirectionObj<Sprite>[] textures;
        public Direction direction;

        public SpriteRenderer spriteRenderer;
        
        private void Start()
        {
            UpdateLook();
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)]
                .GetDirection(direction.GetDirectionFromInterCardinal());
        }

        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }
    }
}
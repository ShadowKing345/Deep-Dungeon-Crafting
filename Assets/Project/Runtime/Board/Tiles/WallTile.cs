using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Board.Tiles
{
    public class WallTile : MonoBehaviour, ITile, IDirectional
    {
        [SerializeField] private OctaDirectionalObj<Sprite>[] textures;
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
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)].GetDirection(direction);
        }
    }
}
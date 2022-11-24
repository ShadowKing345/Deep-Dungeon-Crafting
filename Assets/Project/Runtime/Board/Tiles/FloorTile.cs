using UnityEngine;

namespace Project.Runtime.Board.Tiles
{
    public class FloorTile : MonoBehaviour, ITile
    {
        public Sprite[] textures;
        public SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            UpdateLook();
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
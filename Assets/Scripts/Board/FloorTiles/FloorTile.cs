using UnityEngine;
using Random = UnityEngine.Random;

namespace Board.FloorTiles
{
    public class FloorTile : MonoBehaviour, IRoomTile
    {
        public Sprite[] textures;
        public SpriteRenderer spriteRenderer;

        private void OnEnable() => UpdateLook();

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
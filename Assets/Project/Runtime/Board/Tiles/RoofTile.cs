using UnityEngine;

namespace Project.Runtime.Board.Tiles
{
    public class RoofTile : MonoBehaviour, ITile
    {
        [SerializeField] private Sprite[] textures;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
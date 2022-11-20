using Entity.Player;
using UnityEngine;

namespace Board.Tiles
{
    public class EntranceTile : MonoBehaviour, ITile
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Space] [SerializeField] private Vector2 offset;

        private void OnEnable()
        {
            UpdateLook();
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        public void PlacePlayer()
        {
            var playerEntity = FindObjectOfType<PlayerEntity>();
            if (playerEntity != null) playerEntity.transform.position = (Vector2) transform.position + offset;
        }
    }
}
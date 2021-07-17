using Entity.Player;
using Interfaces;
using UnityEngine;

namespace Board.FloorTiles
{
    public class EntranceTile : MonoBehaviour, IRoomTile
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Space]
        [SerializeField] private Vector2 offset;
        public void PlacePlayer()
        {
            Player player = FindObjectOfType<Player>();
            if (player != null) player.transform.position = (Vector2) transform.position + offset;
        }

        private void OnEnable() => UpdateLook();

        public void UpdateLook() => spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
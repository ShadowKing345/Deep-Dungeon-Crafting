using Managers;
using UnityEngine;
using Utils.Interfaces;

namespace Board.Tiles
{
    public class ExitTile : MonoBehaviour, ITile, IInteractable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] textures;

        private void OnEnable()
        {
            UpdateLook();
        }

        public bool Interact(GameObject target)
        {
            GameManager.Instance.NextFloor();
            return true;
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Board.Tiles
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
            return true;
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = textures[Random.Range(0, textures.Length)];
        }
    }
}
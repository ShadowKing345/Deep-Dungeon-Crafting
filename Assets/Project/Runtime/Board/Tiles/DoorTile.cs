using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Board.Tiles
{
    public class DoorTile : MonoBehaviour, IDirectional, ITile, IInteractable
    {
        private const float PositionOffset = 1f;
        private static readonly Vector2 PlayerCenterOffset = new(0, -0.4f);
        [SerializeField] private DirectionalObj<Sprite>[] doorTextures;
        [SerializeField] private DirectionalObj<Sprite>[] wallTextures;
        [SerializeField] private Direction direction;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private bool isLocked;
        [SerializeField] private Room connectedRoom;
        [SerializeField] private DoorTile connectedDoorTile;
        [SerializeField] private float doorCoolDown = 0.5f;
        private float coolDown;

        public bool Lock
        {
            get => isLocked;
            set => isLocked = value;
        }

        private void OnEnable()
        {
            UpdateLook();
        }

        public Direction Direction
        {
            get => direction;
            set => direction = value;
        }

        public bool Interact(GameObject target)
        {
            if (isLocked) return false;
            if (connectedDoorTile == null) return false;
            if (Time.time < coolDown) return false;

            target.transform.position = (Vector2) connectedDoorTile.transform.position +
                                        connectedDoorTile.CalculatePlayerPlacement();
            coolDown = Time.time + doorCoolDown;
            connectedRoom.OnPlayerEnter();

            return true;
        }

        public void UpdateLook()
        {
            spriteRenderer.sprite = connectedRoom == null
                ? wallTextures[Random.Range(0, doorTextures.Length)].GetDirection(direction)
                : doorTextures[Random.Range(0, doorTextures.Length)].GetDirection(direction);
        }

        public void SetUpDoor(Room connectedRoom, DoorTile connectedDoorTile)
        {
            this.connectedRoom = connectedRoom;
            this.connectedDoorTile = connectedDoorTile;
        }

        private Vector2 CalculatePlayerPlacement()
        {
            return direction switch
            {
                Direction.S => new Vector2(0, PositionOffset),
                Direction.N => new Vector2(0, -PositionOffset),
                Direction.W => new Vector2(PositionOffset, 0),
                Direction.E => new Vector2(-PositionOffset, 0),
                _ => Vector2.zero
            } - PlayerCenterOffset / 2;
        }
    }
}
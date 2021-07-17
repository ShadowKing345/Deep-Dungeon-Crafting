using Interfaces;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Board.FloorTiles
{
    public class DoorTile : MonoBehaviour, IDirectional, IRoomTile, IInteractable
    {
        [SerializeField] private DirectionObj<Sprite>[] textures;
        [SerializeField] private Direction direction;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private bool isLocked;
        [SerializeField] private Room connectedRoom;
        [SerializeField] private DoorTile connectedDoorTile;

        [SerializeField] private float doorCoolDown = 0.5f;
        private float coolDown;

        private const float PositionOffset = 1f;
        private static readonly Vector2 PlayerCenterOffset = new Vector2(0, -0.4f);
        
        private void OnEnable() => UpdateLook();

        public Direction Direction
        {
            set => direction = value;
        }

        public void UpdateLook() => spriteRenderer.sprite = textures[Random.Range(0, textures.Length)].GetDirection(direction);

        public void SetUpDoor(Board.Room connectedRoom, DoorTile connectedDoorTile)
        {
            this.connectedRoom = connectedRoom;
            this.connectedDoorTile = connectedDoorTile;
        }

        public bool Interact(GameObject target)
        {
            if (isLocked) return false;
            if (connectedDoorTile == null) return false;
            if (Time.time < coolDown) return false;

            target.transform.position = (Vector2) connectedDoorTile.transform.position + connectedDoorTile.CalculatePlayerPlacement();
            coolDown = Time.time + doorCoolDown;
            
            return true;
        }

        private Vector2 CalculatePlayerPlacement() =>
            direction switch
            {
                Utils.Direction.S => new Vector2(0, PositionOffset),
                Utils.Direction.N => new Vector2(0, -PositionOffset),
                Utils.Direction.W => new Vector2(PositionOffset, 0),
                Utils.Direction.E => new Vector2(-PositionOffset, 0),
                _ => Vector2.zero
            } - PlayerCenterOffset / 2;
    }
}
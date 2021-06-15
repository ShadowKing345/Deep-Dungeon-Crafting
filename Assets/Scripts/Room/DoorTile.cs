using Interfaces;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Room
{
    public class DoorTile : MonoBehaviour, IDirectional, IRoomTile, IInteractable
    {
        public DirectionObj<Sprite>[] textures;
        public Direction direction;
        public SpriteRenderer spriteRenderer;

        public bool isLocked;
        public Room connectedRoom;
        public DoorTile connectedDoorTile;

        public float doorCoolDown = 0.5f;
        private float coolDown;

        private const float PositionOffset = 1f;
        private static readonly Vector2 PlayerCenterOffset = new Vector2(0, -0.4f);
        
        private void Start()
        {
            UpdateLook();
            coolDown = Time.time;
        }

        public void SetDirection(Direction direction) => this.direction = direction;
        public void UpdateLook() => spriteRenderer.sprite = textures[Random.Range(0, textures.Length)].GetDirection((int) direction);

        public void SetUpDoor(Room connectedRoom, DoorTile connectedDoorTile)
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
                Direction.S => new Vector2(0, PositionOffset),
                Direction.N => new Vector2(0, -PositionOffset),
                Direction.W => new Vector2(PositionOffset, 0),
                Direction.E => new Vector2(-PositionOffset, 0),
                _ => Vector2.zero
            } - PlayerCenterOffset / 2;
    }
}
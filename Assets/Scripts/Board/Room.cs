using Board.Tiles;
using UnityEngine;
using Utils;

namespace Board
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private DirectionalObj<DoorTile> doorTiles;
        [SerializeField] private DirectionalObj<Room> connectedRooms;
        [SerializeField] private EntranceTile entranceTile;
        [SerializeField] private ExitTile exitTile;
        
        public DirectionalObj<DoorTile> DoorTiles
        {
            get => doorTiles;
            set => doorTiles = value;
        }
        public DirectionalObj<Room> ConnectedRooms
        {
            get => connectedRooms;
            set
            {
                connectedRooms = value;
                SetupDoors();
            }
        }
        public EntranceTile EntranceTile
        {
            get => entranceTile;
            set => entranceTile = value;
        }
        public ExitTile ExitTile
        {
            get => exitTile;
            set => exitTile = value;
        }

        private void SetupDoors()
        {
            foreach (Direction direction in DirectionUtils.Cardinals)
            {
                Room connectedRoom = connectedRooms.GetDirection(direction);
                doorTiles.GetDirection(direction)?.SetUpDoor(connectedRoom, connectedRoom != null ? connectedRoom.doorTiles.GetDirection(direction.GetOpposite()): null);
                doorTiles.GetDirection(direction)?.UpdateLook();
            }
        }
    }
}
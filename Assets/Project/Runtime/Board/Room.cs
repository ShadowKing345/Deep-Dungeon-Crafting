using System.Linq;
using Project.Runtime.Board.Tiles;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Board
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private DirectionalObj<DoorTile> doorTiles;
        [SerializeField] private DirectionalObj<Room> connectedRooms;
        [SerializeField] private EntranceTile entranceTile;
        [SerializeField] private ExitTile exitTile;

        [SerializeField] private GameObject[] enemyList;
        [SerializeField] private int livingCount;

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

        private void Awake()
        {
            livingCount = enemyList.Where(e => e.TryGetComponent(out IDamageable _)).ToArray().Length;
        }

        private void SetupDoors()
        {
            foreach (var direction in DirectionUtils.Cardinals)
            {
                var connectedRoom = connectedRooms.GetDirection(direction);
                doorTiles.GetDirection(direction)?.SetUpDoor(connectedRoom,
                    connectedRoom != null ? connectedRoom.doorTiles.GetDirection(direction.GetOpposite()) : null);
                doorTiles.GetDirection(direction)?.UpdateLook();
            }
        }

        public void OnPlayerEnter()
        {
            if (livingCount <= 0) return;
            LockDoors();
            // foreach (GameObject enemy in enemyList)
            // if (enemy != null && enemy.TryGetComponent(out IDamageable damageable))
            // damageable.OnDeath += _ =>
            // {
            //     if (--livingCount <= 0) UnlockDoors();
            // };
        }

        private void LockDoors()
        {
            foreach (var direction in DirectionUtils.Cardinals) doorTiles.GetDirection(direction).Lock = true;
        }

        private void UnlockDoors()
        {
            foreach (var direction in DirectionUtils.Cardinals) doorTiles.GetDirection(direction).Lock = false;
        }
    }
}
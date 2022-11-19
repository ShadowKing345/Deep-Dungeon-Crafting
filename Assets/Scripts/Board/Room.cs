using System;
using System.Linq;
using Board.Tiles;
using Interfaces;
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

        private void SetupDoors()
        {
            foreach (Direction direction in DirectionUtils.Cardinals)
            {
                Room connectedRoom = connectedRooms.GetDirection(direction);
                doorTiles.GetDirection(direction)?.SetUpDoor(connectedRoom, connectedRoom != null ? connectedRoom.doorTiles.GetDirection(direction.GetOpposite()): null);
                doorTiles.GetDirection(direction)?.UpdateLook();
            }
        }

        private void Awake() => livingCount = enemyList.Where(e => e.TryGetComponent(out IDamageable _)).ToArray().Length;

        public void OnPlayerEnter()
        {
            if(livingCount <= 0) return;
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
            foreach (Direction direction in DirectionUtils.Cardinals) doorTiles.GetDirection(direction).Lock = true;
        }

        private void UnlockDoors()
        {
            foreach (Direction direction in DirectionUtils.Cardinals) doorTiles.GetDirection(direction).Lock = false;
        }
    }
}
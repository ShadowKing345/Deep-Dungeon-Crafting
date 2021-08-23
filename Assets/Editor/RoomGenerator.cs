using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Board;
using Board.Tiles;
using Interfaces;
using Managers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor
{
    internal class FloorLayout
    {
        public Dictionary<string, string> keys;
        public string[][] rooms;
    }


    [Serializable]
    internal class FloorScheme
    {
        public GameObject floorTiles;
        public GameObject wallTile;
        public GameObject doorTile;
        public GameObject exitTile;
        public GameObject entranceTile;
        public GameObject obstacles;
        public GameObject roofTile;
    }

    public class RoomGenerator : EditorWindow
    {
        [SerializeField] private FloorScheme floorScheme;

        [MenuItem("Tools/Room Generator")]
        public static void Open()
        {
            RoomGenerator window = GetWindow<RoomGenerator>("Room Generator");
            window.floorScheme = new FloorScheme();
        }

        private void OnGUI()
        {
            floorScheme.floorTiles =
                (GameObject) EditorGUILayout.ObjectField("Floor Tile", floorScheme.floorTiles, typeof(GameObject),
                    true);
            floorScheme.wallTile =
                (GameObject) EditorGUILayout.ObjectField("Wall Tile", floorScheme.wallTile, typeof(GameObject), true);
            floorScheme.doorTile =
                (GameObject) EditorGUILayout.ObjectField("Door Tile", floorScheme.doorTile, typeof(GameObject), true);
            floorScheme.exitTile =
                (GameObject) EditorGUILayout.ObjectField("Exit Tile", floorScheme.exitTile, typeof(GameObject), true);
            floorScheme.entranceTile = (GameObject) EditorGUILayout.ObjectField("Entrance Tile",
                floorScheme.entranceTile, typeof(GameObject), true);
            floorScheme.obstacles =
                (GameObject) EditorGUILayout.ObjectField("Obstacle", floorScheme.obstacles, typeof(GameObject), true);
            floorScheme.roofTile =
                (GameObject) EditorGUILayout.ObjectField("Roof Tile", floorScheme.roofTile, typeof(GameObject), true);

            if (GUILayout.Button("Generate"))
                GenerateRoom();
        }

        public void GenerateRoom()
        {
            FloorLayout floorLayout =
                JsonConvert.DeserializeObject<FloorLayout>(File.ReadAllText("Assets/Editor/FloorLayout.json"));


            foreach (string[] room in floorLayout.rooms)
            {
                GameObject content = new GameObject("Room");

                Dictionary<Vector2Int, GameObject> dictionary = new Dictionary<Vector2Int, GameObject>();

                int width = room.FirstOrDefault().Length - 1;
                int height = room.Length - 1;

                Vector2Int pos = new Vector2Int(0, height);

                foreach (string row in room)
                {
                    foreach (var obj in row.Select(c => floorLayout.keys[c.ToString()]).Select(type => type switch
                    {
                        "wall" => PrefabUtility.InstantiatePrefab(floorScheme.wallTile),
                        "floor" => PrefabUtility.InstantiatePrefab(floorScheme.floorTiles),
                        "door" => PrefabUtility.InstantiatePrefab(floorScheme.doorTile),
                        "enemy" => PrefabUtility.InstantiatePrefab(floorScheme.floorTiles),
                        "entrance" => PrefabUtility.InstantiatePrefab(floorScheme.entranceTile),
                        "exit" => PrefabUtility.InstantiatePrefab(floorScheme.exitTile),
                        "obstacle" => PrefabUtility.InstantiatePrefab(floorScheme.obstacles),
                        "roof" => PrefabUtility.InstantiatePrefab(floorScheme.roofTile),
                        _ => null
                    }))
                    {
                        if (obj != null && obj is GameObject obGameObject)
                        {
                            obGameObject.transform.SetParent(content.transform);
                            obGameObject.transform.position = (Vector2) pos;
                            dictionary.Add(pos, obGameObject);
                        }

                        pos.x++;
                    }

                    pos.y--;
                    pos.x = 0;
                }

                Room roomComponent = content.AddComponent<Room>();

                foreach (KeyValuePair<Vector2Int, GameObject> kvPair in dictionary)
                {
                    Direction direction = Direction.W;

                    pos = kvPair.Key;
                    if (!kvPair.Value.TryGetComponent(out IDirectional directional)) continue;

                    if (pos.x == 0 && pos.y != 0 && pos.y != height) direction = Direction.W;
                    if (pos.x == width && pos.y != 0 && pos.y != height) direction = Direction.E;

                    if (pos.y == height && pos.x != 0 && pos.x != width) direction = Direction.N;
                    if (pos.y == 0 && pos.x != 0 && pos.x != width) direction = Direction.S;

                    if (pos.x == 0 && pos.y == height) direction = Direction.NW;
                    if (pos.x == width && pos.y == height) direction = Direction.NE;

                    if (pos.x == 0 && pos.y == 0) direction = Direction.SW;
                    if (pos.x == width && pos.y == 0) direction = Direction.SE;

                    directional.Direction = direction;
                    if (directional is ITile roomTile) roomTile.UpdateLook();
                }

                roomComponent.DoorTiles = new DirectionalObj<DoorTile>();

                foreach (KeyValuePair<Vector2Int, GameObject> kvPair in dictionary.Where(pair =>
                    pair.Value.GetComponent<DoorTile>() != null))
                {
                    DoorTile doorTile = kvPair.Value.GetComponent<DoorTile>();
                    roomComponent.DoorTiles.SetDirection(doorTile.Direction, doorTile);
                }

                KeyValuePair<Vector2Int, GameObject> entrancePair =
                    dictionary.FirstOrDefault(pair => pair.Value.GetComponent<EntranceTile>() != null);
                KeyValuePair<Vector2Int, GameObject> exitPair =
                    dictionary.FirstOrDefault(pair => pair.Value.GetComponent<ExitTile>() != null);

                roomComponent.EntranceTile =
                    entrancePair.Value == null ? null : entrancePair.Value.GetComponent<EntranceTile>();
                roomComponent.ExitTile = exitPair.Value == null ? null : exitPair.Value.GetComponent<ExitTile>();
            }
        }
    }
}
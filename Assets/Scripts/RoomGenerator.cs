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

class FloorLayout
{
    public Dictionary<string, string> keys;
    public string[] room;
}


[Serializable]
class FloorScheme
{
    public GameObject floorTiles;
    public GameObject wallTile;
    public GameObject doorTile;
    public GameObject exitTile;
    public GameObject entranceTile;
    public GameObject obstacles;
    public GameObject roofTile;
}

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private FloorScheme floorScheme;

    public void GenerateRoom()
    {
        GameObject content = new GameObject("Room");
        content.transform.SetParent(transform);
        FloorLayout floorLayout =
            JsonConvert.DeserializeObject<FloorLayout>(File.ReadAllText("Assets/Editor/FloorLayout.json"));
        Dictionary<Vector2Int, GameObject> dictionary = new Dictionary<Vector2Int, GameObject>();
        int width = floorLayout.room.FirstOrDefault().Length - 1;
        int height = floorLayout.room.Length - 1;

        Vector2Int pos = new Vector2Int(0, height);
        foreach (string row in floorLayout.room)
        {
            foreach (var obj in row.Select(c => floorLayout.keys[c.ToString()]).Select(type => type switch
            {
                "wall" => Instantiate(floorScheme.wallTile),
                "floor" => Instantiate(floorScheme.floorTiles),
                "door" => Instantiate(floorScheme.doorTile),
                "enemy" => Instantiate(floorScheme.floorTiles),
                "entrance" => Instantiate(floorScheme.entranceTile),
                "exit" => Instantiate(floorScheme.exitTile),
                "obstacle" => Instantiate(floorScheme.obstacles),
                "roof" => Instantiate(floorScheme.roofTile),
                _ => null
            }))
            {
                if (obj != null)
                {
                    obj.transform.SetParent(content.transform);
                    obj.transform.position = (Vector2) pos;
                    dictionary.Add(pos, obj);
                }

                pos.x++;
            }

            pos.y--;
            pos.x = 0;
        }

        Room room = content.AddComponent<Room>();

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

        room.DoorTiles = new DirectionalObj<DoorTile>();

        foreach (KeyValuePair<Vector2Int,GameObject> kvPair in dictionary.Where(pair => pair.Value.GetComponent<DoorTile>() != null))
        {
            DoorTile doorTile = kvPair.Value.GetComponent<DoorTile>();
            room.DoorTiles.SetDirection(doorTile.Direction, doorTile);
        }

        KeyValuePair<Vector2Int, GameObject> entrancePair = dictionary.FirstOrDefault(pair => pair.Value.GetComponent<EntranceTile>() != null);
        KeyValuePair<Vector2Int, GameObject> exitPair = dictionary.FirstOrDefault(pair => pair.Value.GetComponent<ExitTile>() != null);

        room.EntranceTile = entrancePair.Value == null ? null : entrancePair.Value.GetComponent<EntranceTile>();
        room.ExitTile = exitPair.Value == null ? null : exitPair.Value.GetComponent<ExitTile>();
    }
}

[CustomEditor(typeof(RoomGenerator))]
public class Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Gen Room")) ((RoomGenerator) target).GenerateRoom();
        base.OnInspectorGUI();
    }
}

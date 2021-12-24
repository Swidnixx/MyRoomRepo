using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    Door doorPrefab;
    [SerializeField]
    Chest chestPrefab;

    [SerializeField]
    List<Room> rooms;

    private void Start()
    {
        foreach(Room room in rooms)
        {
            room.Generate(Vector3.zero, wallPrefab, doorPrefab);
            room.InsertDoor(Random.Range(5, 75), doorPrefab);
            room.InsertChest(chestPrefab);
        }
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem
{
    public int width { get; private set; }
    public int height { get; private set; }
    private node[,] mapArray;

    public MapSystem(int width, int height)
    {
        this.width = width;
        this.height = height;

        mapArray = new node[width, height];
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapArray[i,j] = new node { };
            }
        }
    }
    public void SetRoomType(int x, int y, string roomType)
    {
        Debug.Log(mapArray[x, y].roomType);
        mapArray[x,y].roomType = roomType;
    }
    public string GetRoomType(int x, int y)
    {
        return mapArray[x, y].roomType;
    }
    public void SetNextRoom(int x, int y, List<int> roomList)
    {
        mapArray[x, y].next.Clear();
        foreach (int room in roomList)
        {
            mapArray[x, y].next.Add(room);
        }
    }
    public List<int> GetNextRoom(int x, int y)
    {
        return mapArray[x, y].next;
    }
}

public class node
{
    public int x { get; set; }
    public int y { get; set; }
    public List<int> next { get; set; } = new List<int>();
    public string roomType { get; set; } = "";
}

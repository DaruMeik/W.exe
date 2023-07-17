using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    public static MapSystem map;
    [SerializeField] private GameObject debugNumberTest;
    public int width;
    public int height;
    private GameObject[,] debugNumberMap;

    private void Start()
    {
        Instance = this;
        map = new MapSystem(width, height);
        InitMap();
        GenerateMap();
        UpdateMap();
    }

    private void InitMap()
    {
        debugNumberMap = new GameObject[map.width, map.height];
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                map.SetRoomType(x, y, "");
                debugNumberMap[x, y] = Instantiate(debugNumberTest, gameObject.transform);
                debugNumberMap[x, y].transform.position = new Vector3(x, y, 0f);
            }
        }
        map.SetRoomType(0, Mathf.FloorToInt(map.height / 2f), "Start");
    }

    private void GenerateMap()
    {
        //special first room rule
        List<int> possibleFirstNodes = new List<int>();
        for(int i = 0; i < map.height; i++)
        {
            possibleFirstNodes.Add(i);
        }
        for(int i = 0; i < map.height - 4; i++)
        {
            possibleFirstNodes.RemoveAt(Random.Range(0, possibleFirstNodes.Count));
        }
        foreach(int i in possibleFirstNodes)
        {
            map.SetRoomType(1, i, "!!");
        }
        map.SetNextRoom(0, Mathf.FloorToInt(map.height / 2f), possibleFirstNodes);
        for (int x = 1; x < map.width - 1; x++)
        {
            int minIndex = 0;
            int maxIndex = map.height;
            for (int y = 0; y < map.height; y++)
            {
                if (map.GetRoomType(x, y) != "")
                {
                    int rand = Random.Range(0, 100);
                    if (rand < 40)
                    {
                        int randY = Random.Range(Mathf.Max(minIndex, y - 1), Mathf.Min(maxIndex, y + 2));
                        if (randY > minIndex)
                            minIndex = randY;
                        map.SetRoomType(x + 1, randY, "!!");
                        map.SetNextRoom(x, y, new List<int> { randY });
                    }
                    else
                    {
                        int randY1 = Random.Range((int)Mathf.Max(minIndex, y - 1), (int)Mathf.Min(maxIndex, y + 2));
                        if (randY1 > minIndex)
                            minIndex = randY1;
                        map.SetRoomType(x + 1, randY1, "!!");
                        int randY2 = Random.Range((int)Mathf.Max(minIndex, y - 1), (int)Mathf.Min(maxIndex, y + 2));
                        if (randY2 > minIndex)
                            minIndex = randY2;
                        map.SetRoomType(x + 1, randY2, "!!");
                        if (randY1 != randY2)
                            map.SetNextRoom(x, y, new List<int> { randY1, randY2 });
                        else
                            map.SetNextRoom(x, y, new List<int> { randY1 });
                    }
                }
            }
        }
    }
    public void UpdateMap()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                string destionation = "";
                foreach (int i in map.GetNextRoom(x, y))
                {
                    destionation += i + " ";
                }
                debugNumberMap[x, y].GetComponentInChildren<TextMeshPro>().text = ("( " + destionation + ")");
            }
        }
    }
}

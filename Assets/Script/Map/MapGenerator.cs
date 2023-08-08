using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    public static MapSystem map;
    [SerializeField] private GameObject node;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject playerIcon;
    public Sprite[] nodeImages;
    public int[] currentPos;
    public int width;
    public int height;
    private GameObject[,] nodeMap;
    private List<GameObject> lineList;
    [SerializeField] private EventBroadcast eventBroadcast;

    [Header("Scene List")]
    public List<string> garageFightList = new List<string>();
    public List<string> garageMinibossList = new List<string> ();
    public List<string> officeFightList = new List<string>();
    public List<string> randomFightList = new List<string>();
    public List<string> restList = new List<string>();
    public List<string> upgradeList = new List<string>();
    public List<string> shopList = new List<string>();
    public List<string> garageBossList = new List<string>();

    private void OnEnable()
    {
        eventBroadcast.generateMap += GenerateNode;
    }
    private void OnDisable()
    {
        eventBroadcast.generateMap -= GenerateNode;
    }
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        lineList = new List<GameObject>();
        map = new MapSystem(width, height);
        currentPos = new int[2] { 0, 0 };
        InitMap();
    }

    private void InitMap()
    {
        nodeMap = new GameObject[map.width, map.height];
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                map.SetRoomType(x, y, "");
                nodeMap[x, y] = Instantiate(node, gameObject.transform);
                nodeMap[x, y].transform.position = new Vector3(x, y, 0f);
            }
        }
        map.SetRoomType(0, Mathf.FloorToInt(map.height / 2f), "Start");
    }

    private void GenerateNode()
    {
        // ClearMap
        foreach(GameObject gObj in lineList)
        {
            Destroy(gObj);
        }
        lineList.Clear();
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                map.SetRoomType(x, y, "");
                map.SetNextRoom(x, y, new List<int>());
            }
        }
        //special first room rule
        ChangeCurrentPos(0, Mathf.FloorToInt(map.height / 2f));
        List<int> possibleFirstNodes = new List<int>();
        for (int i = 0; i < map.height; i++)
        {
            possibleFirstNodes.Add(i);
        }
        for (int i = 0; i < map.height - 3; i++)
        {
            possibleFirstNodes.RemoveAt(Random.Range(0, possibleFirstNodes.Count));
        }
        foreach (int i in possibleFirstNodes)
        {
            SetUpRoom(1, i);
        }
        map.SetNextRoom(0, Mathf.FloorToInt(map.height / 2f), possibleFirstNodes);
        for (int x = 1; x < map.width - 1; x++)
        {
            int minIndex = 0;
            int maxIndex = map.height;

            // Check converge
            int availableNode = 0;
            for (int y = 0; y < map.height; y++)
            {
                if (map.GetRoomType(x, y) != "")
                {
                    availableNode++;
                }
            }

            for (int y = 0; y < map.height; y++)
            {
                if (map.GetRoomType(x, y) != "")
                {
                    int rand = Random.Range(0, 100);
                    if (availableNode == 1 || rand >= 60)
                    {
                        int randY1 = Random.Range((int)Mathf.Max(minIndex, y - 1), (int)Mathf.Min(maxIndex, y + 2));
                        if (randY1 > minIndex)
                            minIndex = randY1;
                        SetUpRoom(x + 1, randY1);
                        int randY2 = Random.Range((int)Mathf.Max(minIndex, y - 1), (int)Mathf.Min(maxIndex, y + 2));
                        if (randY2 > minIndex)
                            minIndex = randY2;
                        SetUpRoom(x + 1, randY2);
                        if (randY1 != randY2)
                            map.SetNextRoom(x, y, new List<int> { randY1, randY2 });
                        else
                            map.SetNextRoom(x, y, new List<int> { randY1 });
                    }
                    else
                    {
                        int randY = Random.Range(Mathf.Max(minIndex, y - 1), Mathf.Min(maxIndex, y + 2));
                        if (randY > minIndex)
                            minIndex = randY;
                        SetUpRoom(x + 1, randY);
                        map.SetNextRoom(x, y, new List<int> { randY });
                    }
                }
            }
        }
        GenerateMap();
    }

    private void SetUpRoom(int x, int y)
    {
        if (x == map.width - 1)
        {
            map.SetRoomType(x, y, "Rest");
        }
        else if (x == Mathf.CeilToInt(map.width / 2f))
        {
            map.SetRoomType(x, y, "Upgrade");
        }
        else if (x == Mathf.CeilToInt(map.width / 2f) - 1)
        {
            int dice = Random.Range(0, 100);
            if (dice < 80)
            {
                map.SetRoomType(x, y, "Miniboss");
            }
            else if (dice < 95)
            {
                map.SetRoomType(x, y, "Fight");
            }
            else
            {
                map.SetRoomType(x, y, "Random");
            }
        }
        else if (x == 1 | x == 2)
        {
            int dice = Random.Range(0, 100);
            if (dice < 75)
            {
                map.SetRoomType(x, y, "Fight");
            }
            else
            {
                map.SetRoomType(x, y, "Random");
            }
        }
        else
        {
            int dice = Random.Range(0, 100);
            if (dice < 40)
            {
                map.SetRoomType(x, y, "Fight");
            }
            else if (dice < 70)
            {
                map.SetRoomType(x, y, "Random");
            }
            else if (dice < 82)
            {
                map.SetRoomType(x, y, "Shop");
            }
            else if (dice < 92)
            {
                map.SetRoomType(x, y, "Miniboss");
            }
            else
            {
                map.SetRoomType(x, y, "Rest");
            }
        }

    }
    public void GenerateMap()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                string destionation = "";
                string roomType = map.GetRoomType(x, y);
                foreach (int i in map.GetNextRoom(x, y))
                {
                    destionation += i + " ";
                    GameObject temp = Instantiate(line, nodeMap[x, y].transform);
                    lineList.Add(temp);
                    lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(x, y, 0f), new Vector3(x + 1, i, 0f) });
                }
                switch (roomType)
                {
                    case "Start":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[0];
                        break;
                    case "Fight":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[1];
                        break;
                    case "Random":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[2];
                        break;
                    case "Rest":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[3];
                        break;
                    case "Shop":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[4];
                        break;
                    case "Upgrade":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[5];
                        break;
                    case "Miniboss":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[6];
                        break;
                    default:
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = null;
                        break;
                }
            }
        }
    }
    public void ChangeCurrentPos(int x, int y)
    {
        currentPos[0] = x;
        currentPos[1] = y;
        playerIcon.transform.position = new Vector3(currentPos[0], currentPos[1]);
    }

    public void Travel(string roomType, int nextPos)
    {
        ChangeCurrentPos(currentPos[0] + 1, nextPos);
        switch (roomType)
        {
            case "Fight":
                SceneManager.LoadScene(garageFightList[Random.Range(0, garageFightList.Count)]);
                break;
            case "Random":
                SceneManager.LoadScene(randomFightList[Random.Range(0, randomFightList.Count)]);
                break;
            case "Rest":
                SceneManager.LoadScene(restList[Random.Range(0, restList.Count)]);
                break;
            case "Shop":
                SceneManager.LoadScene(shopList[Random.Range(0, shopList.Count)]);
                break;
            case "Upgrade":
                SceneManager.LoadScene(upgradeList[Random.Range(0, upgradeList.Count)]);
                break;
            case "Miniboss":
                SceneManager.LoadScene(garageMinibossList[Random.Range(0, garageMinibossList.Count)]);
                break;
        }
    }
}

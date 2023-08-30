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
    public Sprite[] bossImages;
    public int currentBossIndex;
    public int[] currentPos;
    public int width;
    public int height;
    public string currentReward;
    private GameObject[,] nodeMap;
    private List<GameObject> lineList;
    [SerializeField] private EventBroadcast eventBroadcast;
    [SerializeField] private PlayerStat playerStat;

    [Header("Scene List")]
    public List<string> garageEasyFightList = new List<string>();
    public List<string> garageHardFightList = new List<string>();
    public List<string> garageMinibossList = new List<string>();
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
        currentBossIndex = Random.Range(0, garageBossList.Count);
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
        map.SetRoomType(0, 1, "Start");
        map.SetNextRoom(0, 1, new List<int> { 0, 2 });
    }

    private void GenerateNode()
    {
        // ClearMap
        foreach (GameObject gObj in lineList)
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
        ChangeCurrentPos(0, 1);

        // Set The Skeleton
        List<string> mapSkeleton = new List<string>();
        int chanceForWeakPrice = 25;
        // Init skeleton
        for (int i = 0; i < map.width; i++)
        {
            mapSkeleton.Add("");
        }
        // Setting the rule
        mapSkeleton[0] = "Start";
        mapSkeleton[1] = "ExpType";
        mapSkeleton[4] = "Miniboss";
        mapSkeleton[5] = "Upgrade";
        if (Random.Range(0, 100) < 20)
        {
            mapSkeleton[7] = "RestType";
        }
        mapSkeleton[map.width - 2] = "RestType";
        mapSkeleton[map.width - 1] = "Boss";
        for (int i = 0; i < map.width; i++)
        {
            if (mapSkeleton[i] == "")
            {
                int dice = Random.Range(0, 100);
                if (dice < chanceForWeakPrice)
                {
                    mapSkeleton[i] = "WeakType";
                    chanceForWeakPrice = Mathf.Max(0, chanceForWeakPrice - 50);
                }
                else
                {
                    mapSkeleton[i] = "StrongType";
                    chanceForWeakPrice += 25;
                }
            }
        }


        map.SetRoomType(0, 1, "Start");
        map.SetNextRoom(0, 1, new List<int> { 0, 2 });
        List<int> nextNode = new List<int>() { 0, 2 };
        for (int x = 1; x < map.width - 1; x++)
        {
            List<string> possibleNodes = new List<string>();
            switch (mapSkeleton[x])
            {
                case "ExpType":
                    possibleNodes = new List<string> { "RedExp", "GreenExp", "BlueExp" };
                    break;
                case "Miniboss":
                    possibleNodes = new List<string> { "Miniboss" };
                    break;
                case "Upgrade":
                    possibleNodes = new List<string>() { "Upgrade" };
                    break;
                case "RestType":
                    possibleNodes = new List<string>() { "Rest", "Shop", "Random" };
                    break;
                case "Boss":
                    possibleNodes = new List<string>() { "Boss" };
                    break;
                case "WeakType":
                    possibleNodes = new List<string>() { "Chip", "Gem" , "Gold"};
                    break;
                case "StrongType":
                    possibleNodes = new List<string>() { "RedExp", "GreenExp", "BlueExp", "Gold", "MaxHP" };
                    break;
            }

            // Set Room
            for (int y = 0; y < map.height; y++)
            {
                if (nextNode.Contains(y))
                {
                    if (Random.Range(0, 100) < 10 && !new List<string> { "Miniboss", "Upgrade", "Boss", "RestType" }.Contains(mapSkeleton[x]))
                        map.SetRoomType(x, y, "Random");
                    else
                    {
                        string nodeType = possibleNodes[Random.Range(0, possibleNodes.Count)];
                        map.SetRoomType(x, y, nodeType);
                        possibleNodes.Remove(nodeType);
                    }
                }
            }

            // Set Next
            nextNode.Clear();
            for (int y = 0; y < map.height; y++)
            {
                if (map.GetRoomType(x, y) != "")
                {
                    if (x == 3 || x == 4 || x == map.width - 2)
                    {
                        map.SetNextRoom(x, y, new List<int> { 1 });
                        nextNode.Add(1);
                    }
                    else if (x == map.width - 3)
                    {
                        map.SetNextRoom(x, y, new List<int> { 0, 1, 2 });
                        nextNode.Add(0);
                        nextNode.Add(1);
                        nextNode.Add(2);
                    }
                    else
                    {
                        List<int> goTo = new List<int> { 0, 1, 2 };
                        goTo.RemoveAt(Random.Range(0, goTo.Count));
                        map.SetNextRoom(x, y, goTo);
                        foreach (int i in goTo)
                        {
                            if (!nextNode.Contains(i))
                                nextNode.Add(i);
                        }
                    }
                }
            }
        }
        map.SetRoomType(map.width - 1, 1, "Boss");
        GenerateMap();
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
                    if (x == map.width - 2)
                        lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(x, y, 0f), new Vector3((x + 2f), i, 0f) });
                    else
                        lineList[lineList.Count - 1].GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(x, y, 0f), new Vector3((x + 1), i, 0f) });
                }
                switch (roomType)
                {
                    case "Start":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[0];
                        break;
                    case "RedExp":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[1];
                        break;
                    case "GreenExp":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[2];
                        break;
                    case "BlueExp":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[3];
                        break;
                    case "Gold":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[4];
                        break;
                    case "MaxHP":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[5];
                        break;
                    case "Gem":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[6];
                        break;
                    case "Chip":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[7];
                        break;
                    case "Random":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[8];
                        break;
                    case "Shop":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[9];
                        break;
                    case "Rest":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[10];
                        break;
                    case "Upgrade":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[11];
                        break;
                    case "Miniboss":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = nodeImages[12];
                        break;
                    case "Boss":
                        nodeMap[x, y].GetComponent<SpriteRenderer>().sprite = bossImages[currentBossIndex];
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
        if (currentPos[0] + 1 != width - 1)
        {
            ChangeCurrentPos(currentPos[0] + 1, nextPos);
        }
        else
        {
            ChangeCurrentPos(currentPos[0] + 1, Mathf.FloorToInt(height / 2f));
        }
        switch (roomType)
        {
            case "RedExp":
                currentReward = "RedExp";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "GreenExp":
                currentReward = "GreenExp";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "BlueExp":
                currentReward = "BlueExp";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "Gold":
                currentReward = "Gold";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "MaxHP":
                currentReward = "MaxHP";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "Gem":
                currentReward = "Gem";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "Chip":
                currentReward = "Chip";
                if (currentPos[0] + 1 < 6)
                    SceneManager.LoadScene(garageEasyFightList[Random.Range(0, garageEasyFightList.Count)]);
                else
                    SceneManager.LoadScene(garageHardFightList[Random.Range(0, garageHardFightList.Count)]);
                break;
            case "Random":
                string sceneName = randomFightList[Random.Range(0, randomFightList.Count)];
                currentReward = "";
                if (garageEasyFightList.Contains(sceneName))
                {
                    List<string> possibleRewards = new List<string> { "RedExp", "GreenExp", "BlueExp", "Gold", "MaxHP", "Gem", "Chip" };
                    currentReward = possibleRewards[Random.Range(0, possibleRewards.Count)];
                }
                SceneManager.LoadScene(sceneName);
                break;
            case "Rest":
                currentReward = "";
                SceneManager.LoadScene(restList[Random.Range(0, restList.Count)]);
                break;
            case "Shop":
                currentReward = "";
                SceneManager.LoadScene(shopList[Random.Range(0, shopList.Count)]);
                break;
            case "Upgrade":
                currentReward = "";
                SceneManager.LoadScene(upgradeList[Random.Range(0, upgradeList.Count)]);
                break;
            case "Miniboss":
                currentReward = "Miniboss";
                SceneManager.LoadScene(garageMinibossList[Random.Range(0, garageMinibossList.Count)]);
                break;
            case "Boss":
                currentReward = "Boss";
                SceneManager.LoadScene(garageBossList[currentBossIndex]);
                break;
        }
    }
}

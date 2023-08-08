using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Range(0, 2)] public int order;
    public SpriteRenderer nextRoomIcon;
    public GameObject[] highlight;
    public int nextRoomIndex;
    public string nextRoomType;

    private MapGenerator mapGenerator;
    private void OnEnable()
    {
        TurnOffHighlight();
        mapGenerator = MapGenerator.Instance;
        if(mapGenerator.currentPos[0] == 0)
        {
            nextRoomIndex = MapGenerator.map.GetNextRoom(mapGenerator.currentPos[0], mapGenerator.currentPos[1])[order];
            nextRoomType = MapGenerator.map.GetRoomType(mapGenerator.currentPos[0]+1, nextRoomIndex);
        }
        else
        {
            List<int> possibleNode = MapGenerator.map.GetNextRoom(mapGenerator.currentPos[0], mapGenerator.currentPos[1]);
            if (possibleNode.Contains(mapGenerator.currentPos[1] + order - 1))
            {
                nextRoomIndex = mapGenerator.currentPos[1] + order - 1;
                nextRoomType = MapGenerator.map.GetRoomType(mapGenerator.currentPos[0]+1, nextRoomIndex);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        switch (nextRoomType)
        {
            case "Start":
                nextRoomIcon.sprite = mapGenerator.nodeImages[0];
                break;
            case "Fight":
                nextRoomIcon.sprite = mapGenerator.nodeImages[1];
                break;
            case "Random":
                nextRoomIcon.sprite = mapGenerator.nodeImages[2];
                break;
            case "Rest":
                nextRoomIcon.sprite = mapGenerator.nodeImages[3];
                break;
            case "Shop":
                nextRoomIcon.sprite = mapGenerator.nodeImages[4];
                break;
            case "Upgrade":
                nextRoomIcon.sprite = mapGenerator.nodeImages[5];
                break;
            case "Miniboss":
                nextRoomIcon.sprite = mapGenerator.nodeImages[6];
                break;
        }
    }

    public void TurnOnHighlight()
    {
        foreach(GameObject gObj in highlight)
        {
            gObj.SetActive(true);
        }
    }
    public void TurnOffHighlight()
    {
        foreach (GameObject gObj in highlight)
        {
            gObj.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Range(0, 2)] public int order;
    public TextAnimation textAnim;
    public SpriteRenderer nextRoomIcon;
    public GameObject[] highlight;
    public int nextRoomIndex;
    public string nextRoomType;

    private MapGenerator mapGenerator;
    private void OnEnable()
    {
        TurnOffHighlight();
        mapGenerator = MapGenerator.Instance;
        
        if(mapGenerator.currentPos[0] == 3)
        {
            if (order == 1)
            {
                nextRoomIndex = 1;
                nextRoomType = "Miniboss";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if(mapGenerator.currentPos[0] == mapGenerator.width - 2)
        {
            if(order == 1)
            {
                nextRoomIndex = 1;
                nextRoomType = "Boss";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            List<int> possibleNode = MapGenerator.map.GetNextRoom(mapGenerator.currentPos[0], mapGenerator.currentPos[1]);
            if (possibleNode.Contains(order))
            {
                nextRoomIndex = order;
                nextRoomType = MapGenerator.map.GetRoomType(mapGenerator.currentPos[0]+1, nextRoomIndex);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if(textAnim == null)
            return;
        textAnim.textBoxList.Clear();
        switch (nextRoomType)
        {
            case "RedExp":
                textAnim.textBoxList.Add("Gains 1 exp in offense.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[1];
                break;
            case "GreenExp":
                textAnim.textBoxList.Add("Gains 1 exp in mobility.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[2];
                break;
            case "BlueExp":
                textAnim.textBoxList.Add("Gains 1 exp in defense.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[3];
                break;
            case "Gold":
                textAnim.textBoxList.Add("Gains some gold.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[4];
                break;
            case "MaxHP":
                textAnim.textBoxList.Add("Increase Max HP.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[5];
                break;
            case "Gem":
                textAnim.textBoxList.Add("Get gems to spend at home.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[6];
                break;
            case "Chip":
                textAnim.textBoxList.Add("Get chips to spend at home.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[7];
                break;
            case "Random":
                textAnim.textBoxList.Add("A random event awaits.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[8];
                break;
            case "Shop":
                textAnim.textBoxList.Add("Spend some gold to heal or get new weapon.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[9];
                break;
            case "Rest":
                textAnim.textBoxList.Add("Rest and recover health.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[10];
                break;
            case "Upgrade":
                textAnim.textBoxList.Add("Obtain a weapon upgrade.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[11];
                break;
            case "Miniboss":
                textAnim.textBoxList.Add("Tough foes await.");
                nextRoomIcon.sprite = mapGenerator.nodeImages[12];
                break;
            case "Boss":
                textAnim.textBoxList.Add("Fearsome foes await. Proceed with caution.");
                nextRoomIcon.sprite = mapGenerator.bossImages[mapGenerator.currentBossIndex];
                break;
            default:
                nextRoomIcon.sprite = null;
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

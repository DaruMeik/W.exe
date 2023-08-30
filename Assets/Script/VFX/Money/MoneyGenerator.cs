using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGenerator : MonoBehaviour
{
    public GameObject[] moneyBox;
    private Transform target;

    private void OnEnable()
    {
        target = FindObjectOfType<PlayerStateManager>().transform;
    }
    public void GenerateMoney(Vector2 spawnPos, int amount)
    {
        int change = amount;
        while(change > 0)
        {
            if(change >= 50)
            {
                GameObject obj = Instantiate(moneyBox[3]);
                obj.transform.position = (Vector2)spawnPos + (Vector2)Random.insideUnitCircle;
                Money temp = obj.GetComponent<Money>();
                temp.target = target;
                temp.waitTime = Time.time + Random.Range(0.25f,1.25f);
                temp.ready = true;
                change -= 50;
            }
            else if (change >= 10)
            {
                GameObject obj = Instantiate(moneyBox[2]);
                obj.transform.position = (Vector2)spawnPos + (Vector2)Random.insideUnitCircle;
                Money temp = obj.GetComponent<Money>();
                temp.target = target;
                temp.waitTime = Time.time + Random.Range(0.25f, 1.25f);
                temp.ready = true;
                change -= 10;
            }
            else if (change >= 5)
            {
                GameObject obj = Instantiate(moneyBox[1]);
                obj.transform.position = (Vector2)spawnPos + (Vector2)Random.insideUnitCircle;
                Money temp = obj.GetComponent<Money>();
                temp.target = target;
                temp.waitTime = Time.time + Random.Range(0.25f, 1.25f);
                temp.ready = true;
                change -= 5;
            }
            else
            {
                GameObject obj = Instantiate(moneyBox[0]);
                obj.transform.position = (Vector2)spawnPos + (Vector2)Random.insideUnitCircle;
                Money temp = obj.GetComponent<Money>();
                temp.target = target;
                temp.waitTime = Time.time + Random.Range(0.25f, 1.25f);
                temp.ready = true;
                change -= 1;
            }
        }
        Destroy(gameObject);
    }
}

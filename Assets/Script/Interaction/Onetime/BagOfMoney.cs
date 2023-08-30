using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagOfMoney : Onetime
{
    public GameObject moneyGenerator;
    public int moneyValue = 40;
    public TextAnimation textAnimation;
    public void Start()
    {
        textAnimation.textBoxList.Clear();
        textAnimation.textBoxList.Add("Gives you " + moneyValue + "G.");
    }
    public override void Interact()
    {
        hasBeenUsed = true;
        Instantiate(moneyGenerator).GetComponent<MoneyGenerator>().GenerateMoney(transform.position, moneyValue);
        Destroy(gameObject);
    }
}

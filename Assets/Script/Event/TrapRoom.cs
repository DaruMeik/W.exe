using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : Onetime
{
    public GameObject moneyGenerator;
    public GameObject enemySpawner;
    public int moneyValue = 60;
    public TextAnimation textAnimation;
    public Animator[] doorAnimator;
    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (Animator anim in doorAnimator)
        {
            anim.SetTrigger("Open");
            anim.SetBool("isOpen", true);
        }
    }
    public void Start()
    {
        textAnimation.textBoxList.Clear();
        textAnimation.textBoxList.Add("Gives you " + moneyValue + "G.");
    }
    public override void Interact()
    {
        hasBeenUsed = true;
        Instantiate(moneyGenerator).GetComponent<MoneyGenerator>().GenerateMoney(transform.position, moneyValue);

        foreach (Animator anim in doorAnimator)
        {
            anim.SetBool("isOpen", false);
        }
        enemySpawner.SetActive(true);
        Destroy(gameObject);
    }
}

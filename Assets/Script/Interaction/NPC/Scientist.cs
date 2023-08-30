using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : NPC
{
    public BoxCollider2D boxCollider;
    private void OnEnable()
    {
        eventBroadcast.finishNewMachine += EffectText;
    }
    private void OnDisable()
    {
        eventBroadcast.finishNewMachine -= EffectText;
    }
    private void EffectText(int curseIndex)
    {
        boxCollider.size *= 2f;
        textAnim.textBoxList.Clear();
        string temp = "Look like you gain:\n";
        switch (curseIndex)
        {

            case 0:
                temp += "Curse Of Offense for the next 3 combats";
                textAnim.textBoxList.Add(temp);
                textAnim.textBoxList.Add("You can only use weapon on the left slot.");
                break;
            case 1:
                temp += "Curse Of Defense for the next 3 combats.";
                textAnim.textBoxList.Add(temp);
                textAnim.textBoxList.Add("You take 10% more damage and possessing no longer heals you.");
                break;
            case 2:
                temp += "Curse Of Mobility for the next 3 combats.";
                textAnim.textBoxList.Add(temp);
                textAnim.textBoxList.Add("Your dash is 50% shorter.");
                break;
        }
        TurnOnText();
    }
}

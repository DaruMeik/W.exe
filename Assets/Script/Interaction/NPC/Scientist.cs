using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : NPC
{
    private void OnEnable()
    {
        eventBroadcast.finishNewMachine += EffectText;
    }
    private void OnDisable()
    {
        eventBroadcast.finishNewMachine -= EffectText;
    }
    private void EffectText(int buff, int nerf)
    {
        textAnim.textBoxList.Clear();
        string temp = "Look like you gain:\n";
        if(buff == nerf)
        {
            temp += "Nothing!?";
        }
        else
        {
            switch (buff)
            {
                case 0:
                    temp += "+10% Atk\n";
                    break;
                case 1:
                    temp += "+10% Def\n";
                    break;
                case 2:
                    temp += "+10 Luck\n";
                    break;
            }
            switch (nerf)
            {

                case 0:
                    temp += "-10% Atk\n";
                    break;
                case 1:
                    temp += "-10% Def\n";
                    break;
                case 2:
                    temp += "-10 Luck\n";
                    break;
            }
        }
        textAnim.textBoxList.Add(temp);
        TurnOnText();
    }
}

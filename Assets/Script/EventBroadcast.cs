using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventBroadcast", menuName = "ScriptableObj/EventBroadcast")]
public class EventBroadcast : ScriptableObject
{
    public delegate void GameEvent();
    public event GameEvent finishPossessionAnimation;
    public event GameEvent updateCardUI;
    public event GameEvent enemyKilled;
    public event GameEvent allDead;
    public event GameEvent generateMap;
    public event GameEvent updateHP;
    public event GameEvent updateMoney;
    public event GameEvent finishText;

    // Random Event
    public delegate void RandomMachine(int buff, int nerf);
    public event RandomMachine finishNewMachine;

    public delegate void GameObjEvent(GameObject gObj);
    public event GameObjEvent possessEvent;

    public void FinishPossessionAnimationNoti()
    {
        if (finishPossessionAnimation != null)
            finishPossessionAnimation.Invoke();
    }
    public void UpdateCardUINoti()
    {
        if(updateCardUI != null)
            updateCardUI.Invoke();
    }
    public void EnemyKilledNoti()
    {
        if(enemyKilled != null)
            enemyKilled.Invoke();
    }
    public void AllDeadNoti()
    {
        if(allDead != null)
            allDead.Invoke();
    }
    public void GenerateMapNoti()
    {
        if(generateMap != null)
            generateMap.Invoke();
    }
    public void UpdateHPNoti()
    {
        if(updateHP != null)
            updateHP.Invoke();
    }
    public void UpdateMoneyNoti()
    {
        if(updateMoney != null)
            updateMoney.Invoke();
    }
    public void FinishTextNoti()
    {
        if(finishText != null)
            finishText.Invoke();
    }
    public void FinishNewMachineNoti(int i, int j)
    {
        if(finishNewMachine != null)
            finishNewMachine.Invoke(i, j);
    }
    public void PossessEventNoti(GameObject gObj)
    {
        if(possessEvent != null)
            possessEvent.Invoke(gObj);
    }
}

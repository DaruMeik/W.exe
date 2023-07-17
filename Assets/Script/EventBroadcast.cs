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
    public void PossessEventNoti(GameObject gObj)
    {
        if(possessEvent != null)
            possessEvent.Invoke(gObj);
    }
}

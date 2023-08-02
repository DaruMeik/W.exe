using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventBroadcast", menuName = "ScriptableObj/EventBroadcast")]
public class EventBroadcast : ScriptableObject
{
    public delegate void GameEvent();
    public event GameEvent finishPossessionAnimation;
    public event GameEvent updateWeaponSprite;
    public event GameEvent updateCardUI;
    public event GameEvent enemyKilled;
    public event GameEvent allDead;
    public event GameEvent generateMap;
    public event GameEvent updateHP;
    public event GameEvent updateLvl;
    public event GameEvent updateWeapon;
    public event GameEvent updateMoney;
    public event GameEvent finishText;
    public event GameEvent enterUI;
    public event GameEvent exitUI;

    // Random Event
    public delegate void RandomMachine(int buff, int nerf);
    public event RandomMachine finishNewMachine;

    public delegate void EnemyEvent(EnemyStateManager enemy);
    public event EnemyEvent possessEvent;

    public void FinishPossessionAnimationNoti()
    {
        if (finishPossessionAnimation != null)
            finishPossessionAnimation.Invoke();
    }
    public void UpdateWeaponSpriteNoti()
    {
        if (updateWeaponSprite != null)
            updateWeaponSprite.Invoke();
    }
    public void UpdateCardUINoti()
    {
        if (updateCardUI != null)
            updateCardUI.Invoke();
    }
    public void EnemyKilledNoti()
    {
        if (enemyKilled != null)
            enemyKilled.Invoke();
    }
    public void AllDeadNoti()
    {
        if (allDead != null)
            allDead.Invoke();
    }
    public void GenerateMapNoti()
    {
        if (generateMap != null)
            generateMap.Invoke();
    }
    public void UpdateHPNoti()
    {
        if (updateHP != null)
            updateHP.Invoke();
    }
    public void UpdateLvlNoti()
    {
        if(updateLvl != null)
            updateLvl.Invoke();
    }
    public void UpdateWeaponNoti()
    {
        if(updateWeapon != null)
            updateWeapon.Invoke();
    }
    public void UpdateMoneyNoti()
    {
        if (updateMoney != null)
            updateMoney.Invoke();
    }
    public void FinishTextNoti()
    {
        if (finishText != null)
            finishText.Invoke();
    }
    public void EnterUINoti()
    {
        if (enterUI != null)
            enterUI.Invoke();
    }
    public void ExitUINoti()
    {
        if (exitUI != null)
            exitUI.Invoke();
    }
    public void FinishNewMachineNoti(int i, int j)
    {
        if (finishNewMachine != null)
            finishNewMachine.Invoke(i, j);
    }
    public void PossessEventNoti(EnemyStateManager enemy)
    {
        if (possessEvent != null)
            possessEvent.Invoke(enemy);
    }
}

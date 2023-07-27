using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    public TextMeshProUGUI[] upgradeInfo;

    private List<int> upgradeList = new List<int>();
    private float previousTimeScale;
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        GenerateReward();
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
        Time.timeScale = previousTimeScale;
    }
    private void GenerateReward()
    {
        // Adding possible reward

        upgradeList.Clear();

        int randVal = 0;
        int rewardTier = 0;
        List<int> possibleReward = new List<int>();

        bool firstTime = true;
        bool startIncrease = false;


        while (upgradeList.Count < 3)
        {
            if (possibleReward.Count > 0 || firstTime)
            {
                firstTime = false;
                randVal = Random.Range(0, 100) + playerStat.luck;
                if (randVal < 60)
                {
                    rewardTier = 1;
                }
                else if (randVal < 90)
                {
                    rewardTier = 2;
                }
                else if (randVal < 100)
                {
                    rewardTier = 3;
                }
                else
                {
                    rewardTier = 4;
                }
            }

            possibleReward.Clear();


            foreach (Upgrade upgrade in UpgradeDatabase.upgradeList)
            {
                if (upgrade.tier == rewardTier && !playerStat.upgradeRegister.Any(r => r == upgrade.id))
                {
                    possibleReward.Add(upgrade.id);
                }
            }


            // Remove stuffs that has been picked
            foreach (int temp in upgradeList)
            {
                possibleReward.Remove(temp);
            }

            if (possibleReward.Count == 0)
            {
                if (!startIncrease)
                {
                    startIncrease = true;
                    rewardTier = 1;
                }
                else if (rewardTier < 3)
                    rewardTier++;
                else
                {
                    upgradeList.Add(0);
                }
            }
            else
            {
                upgradeList.Add(possibleReward[Random.Range(0, possibleReward.Count)]);
            }
        }

        for (int inde = 0; inde < upgradeInfo.Count(); inde++)
        {
            upgradeInfo[inde].text = UpgradeDatabase.upgradeList[upgradeList[inde]].upgradeName + ": "
                + UpgradeDatabase.upgradeList[upgradeList[inde]].upgradeDescription + "(" + UpgradeDatabase.upgradeList[upgradeList[inde]].tier + "*)";
        }
    }
    public void ChooseReward(int buttonIndex)
    {
        Debug.Log(UpgradeDatabase.upgradeList[upgradeList[buttonIndex]].upgradeName);
        if (UpgradeDatabase.upgradeList[upgradeList[buttonIndex]].upgradeBaseEffect != null)
            UpgradeDatabase.upgradeList[upgradeList[buttonIndex]].upgradeBaseEffect.ApplyEffect(playerStat);
        playerStat.upgradeRegister.Add(upgradeList[buttonIndex]);
        gameObject.SetActive(false);
    }
}

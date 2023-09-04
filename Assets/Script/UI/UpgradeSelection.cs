using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private EventBroadcast eventBroadcast;
    public TextMeshProUGUI[] upgradeInfo;
    public Image[] upgradeImage;
    public string rewardType;

    private List<int> upgradeList = new List<int>();
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
        Time.timeScale = Mathf.Max(0, Time.timeScale - 1f);
        GenerateReward();
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
        Time.timeScale = Mathf.Min(1f, Time.timeScale + 1f);
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

        List<Upgrade> colorUpgradeList;
        switch (rewardType)
        {
            case "Red":
            default:
                colorUpgradeList = UpgradeDatabase.redUpgradeList;
                break;
            case "Green":
                colorUpgradeList = UpgradeDatabase.greenUpgradeList;
                break;
            case "Blue":
                colorUpgradeList = UpgradeDatabase.blueUpgradeList;
                break;
        }

        while (upgradeList.Count < 3)
        {
            if (possibleReward.Count > 0 || firstTime)
            {
                firstTime = false;
                randVal = Random.Range(0, 100) + playerStat.luck;
                switch (rewardType)
                {
                    case "Red":
                    default:
                        randVal += 2*playerStat.redLevel * (playerStat.redLevel - 1);
                        break;
                    case "Green":
                        randVal += 2 * playerStat.greenLevel * (playerStat.greenLevel - 1);
                        break;
                    case "Blue":
                        randVal += 2 * playerStat.blueLevel * (playerStat.blueLevel - 1);
                        break;
                }
                Debug.Log("Roll the dice: ");
                Debug.Log(randVal);
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


            foreach (Upgrade upgrade in colorUpgradeList)
            {
                switch (rewardType)
                {
                    case "Red":
                    default:
                        if (upgrade.tier == rewardTier && !playerStat.redUpgradeRegister.Any(r => r == upgrade.id))
                        {
                            possibleReward.Add(upgrade.id);
                        }
                        break;
                    case "Green":
                        if (upgrade.tier == rewardTier && !playerStat.greenUpgradeRegister.Any(r => r == upgrade.id))
                        {
                            possibleReward.Add(upgrade.id);
                        }
                        break;
                    case "Blue":
                        if (upgrade.tier == rewardTier && !playerStat.blueUpgradeRegister.Any(r => r == upgrade.id))
                        {
                            possibleReward.Add(upgrade.id);
                        }
                        break;
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
            upgradeInfo[inde].text = colorUpgradeList[upgradeList[inde]].upgradeName + ": "
                + colorUpgradeList[upgradeList[inde]].upgradeDescription + "(" + colorUpgradeList[upgradeList[inde]].tier + "*)";
            upgradeImage[inde].sprite = colorUpgradeList[upgradeList[inde]].upgradeSprite;
        }
    }
    public void ChooseReward(int buttonIndex)
    {
        List<Upgrade> colorUpgradeList;
        switch (rewardType)
        {
            case "Red":
            default:
                colorUpgradeList = UpgradeDatabase.redUpgradeList;
                playerStat.redUpgradeRegister.Add(upgradeList[buttonIndex]);
                break;
            case "Green":
                colorUpgradeList = UpgradeDatabase.greenUpgradeList;
                playerStat.greenUpgradeRegister.Add(upgradeList[buttonIndex]);
                break;
            case "Blue":
                colorUpgradeList = UpgradeDatabase.blueUpgradeList;
                playerStat.blueUpgradeRegister.Add(upgradeList[buttonIndex]);
                break;
        }
        if (colorUpgradeList[upgradeList[buttonIndex]].upgradeBaseEffect != null)
            colorUpgradeList[upgradeList[buttonIndex]].upgradeBaseEffect.ApplyEffect(playerStat);
        gameObject.SetActive(false);
    }
}

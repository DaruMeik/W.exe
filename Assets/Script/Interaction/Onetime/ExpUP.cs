using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpUP : Onetime
{
    public string rewardType;
    public SpriteRenderer[] sprites;
    public Sprite[] spriteChoices;
    protected override void OnEnable()
    {
        base.OnEnable();
        foreach(SpriteRenderer sr in sprites)
        {
            switch (rewardType)
            {
                case "Red":
                    sr.sprite = spriteChoices[0];
                    break;
                case "Green":
                    sr.sprite = spriteChoices[1];
                    break;
                case "Blue":
                    sr.sprite = spriteChoices[2];
                    break;
                case "White":
                default:
                    sr.sprite = spriteChoices[3];
                    break;
            }
        }
    }
    public override void Interact()
    {
        hasBeenUsed = true;
        switch (rewardType)
        {
            case "Red":
                eventBroadcast.GainExpNoti(1, "Red");
                break;
            case "Green":
                eventBroadcast.GainExpNoti(1, "Green");
                break;
            case "Blue":
                eventBroadcast.GainExpNoti(1, "Blue");
                break;
            case "White":
            default:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        eventBroadcast.GainExpNoti(1, "Red");
                        break;
                    case 1:
                        eventBroadcast.GainExpNoti(1, "Green");
                        break;
                    case 2:
                        eventBroadcast.GainExpNoti(1, "Blue");
                        break;
                }
                break;
        }
        Destroy(gameObject);
    }
}
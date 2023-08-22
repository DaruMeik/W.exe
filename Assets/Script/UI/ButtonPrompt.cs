
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPrompt : MonoBehaviour
{
    public string Type;
    public TextMeshPro text;

    private void OnEnable()
    {
        switch (Type)
        {
            default:
                text.text =  PlayerControl.Instance.pInput.Player.Interact.bindings[0].path.Replace("<Keyboard>/","").ToUpper();
                break;
        }
    }
}

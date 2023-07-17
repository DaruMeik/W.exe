using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance { get; private set; }
    public PlayerInput pInput;

    private void OnEnable()
    {
        Instance = this;
        pInput = new PlayerInput();
        pInput.Enable();
        pInput.Player.Enable();
        pInput.UI.Disable();
        Debug.Log(pInput);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance { get; private set; }
    public PlayerInput pInput;

    private void OnEnable()
    {
        if(Instance == null)
            Instance = this;
        pInput = new PlayerInput();
        pInput.Enable();
        pInput.Player.Enable();
        pInput.UI.Enable();
    }
}

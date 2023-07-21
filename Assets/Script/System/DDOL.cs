using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    public GameObject[] foreverApp;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        foreach (GameObject obj in foreverApp)
        {
            DontDestroyOnLoad(obj);
        }
        SceneManager.LoadScene("MainMenu");
    }
}

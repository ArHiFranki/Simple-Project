using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public event UnityAction<bool> GamePause;

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        GamePause?.Invoke(true);
    }

    public void ClosePlanel(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        GamePause?.Invoke(false);
    }

    public void Exit()
    {
        Debug.Log("Application quit");
        Application.Quit();
    }
}

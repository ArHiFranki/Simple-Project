using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject _WinScreen;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameController _gameController;

    private void OnEnable()
    {
        _gameController.GameWin += OnGameWin;
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDisable()
    {
        _gameController.GameWin += OnGameWin;
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void Start()
    {
        _WinScreen.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnGameWin()
    {
        Time.timeScale = 0;
        _WinScreen.SetActive(true);
    }

    private void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void OnExitButtonClick()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}

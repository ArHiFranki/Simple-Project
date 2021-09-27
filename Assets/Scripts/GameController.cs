using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private bool _isGamePause;

    public bool IsGamePause => _isGamePause;

    private void OnEnable()
    {
        _menu.GamePause += SetGamePauseCondition;
    }

    private void OnDisable()
    {
        _menu.GamePause -= SetGamePauseCondition;
    }

    public void SetGamePauseCondition(bool condition)
    {
        _isGamePause = condition;
    }
}

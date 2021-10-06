using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUnitEnableChecker : MonoBehaviour
{
    [SerializeField] private Button _unitButton;
    [SerializeField] private GameObject _disablePanel;
    [SerializeField] private PlayerUnit _playerUnit;
    [SerializeField] private GameController _gameController;

    private bool _isUnitBuild;

    private void Start()
    {
        OnGoldChange(0);
    }

    private void OnEnable()
    {
        _gameController.GoldChanged += OnGoldChange;
        _gameController.NewUnitBuild += OnNewUnitBuild;
        _gameController.StyleSelected += OnStyleSelected;
    }

    private void OnDisable()
    {
        _gameController.GoldChanged -= OnGoldChange;
        _gameController.NewUnitBuild -= OnNewUnitBuild;
        _gameController.StyleSelected -= OnStyleSelected;
    }

    private void OnGoldChange(int gold)
    {
        if (_gameController.Gold >= _playerUnit.unitPrice)
        {
            if(!_isUnitBuild)
            {
                SetButtonEnable();
            }
        }
        else
        {
            SetButtonDisable();
        }
    }

    private void OnNewUnitBuild()
    {
        SetButtonDisable();
        _isUnitBuild = true;
    }

    private void OnStyleSelected()
    {
        if (_gameController.Gold >= _playerUnit.unitPrice)
        {
            SetButtonEnable();
        }
        _isUnitBuild = false;
    }

    private void SetButtonEnable()
    {
        _unitButton.enabled = true;
        _disablePanel.SetActive(false);
    }

    private void SetButtonDisable()
    {
        _unitButton.enabled = false;
        _disablePanel.SetActive(true);
    }
}

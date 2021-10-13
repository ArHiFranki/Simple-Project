using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionChecker : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Button _constructionButton;
    [SerializeField] private GameObject _disablePanel;
    [SerializeField] private ProtectiveConstruction _protectiveConstruction;

    private bool _isUnitBuild;

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

    private void Start()
    {
        OnGoldChange();
    }

    private void OnGoldChange()
    {
        if (_gameController.Gold >= _protectiveConstruction.costProtectiveConstruction)
        {
            if (!_isUnitBuild)
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
        if (_gameController.Gold >= _protectiveConstruction.costProtectiveConstruction)
        {
            SetButtonEnable();
        }
        _isUnitBuild = false;
    }

    private void SetButtonEnable()
    {
        _constructionButton.enabled = true;
        _disablePanel.SetActive(false);
    }

    private void SetButtonDisable()
    {
        _constructionButton.enabled = false;
        _disablePanel.SetActive(true);
    }
}

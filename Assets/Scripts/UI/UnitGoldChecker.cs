using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitGoldChecker : MonoBehaviour
{
    [SerializeField] private Button _unitButton;
    [SerializeField] private GameObject _disablePanel;
    [SerializeField] private PlayerUnit _playerUnit;
    [SerializeField] private GameController _gameController;

    private void Start()
    {
        OnGoldChange(0);
    }

    private void OnEnable()
    {
        _gameController.GoldChanged += OnGoldChange;
    }

    private void OnDisable()
    {
        _gameController.GoldChanged -= OnGoldChange;
    }

    private void OnGoldChange(int gold)
    {
        if (_gameController.Gold >= _playerUnit.unitPrice)
        {
            _unitButton.enabled = true;
            _disablePanel.SetActive(false);
        }
        else
        {
            _disablePanel.SetActive(true);
            _unitButton.enabled = false;
        }
    }
}

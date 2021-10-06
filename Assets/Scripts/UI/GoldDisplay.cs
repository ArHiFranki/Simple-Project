using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private TMP_Text _goldCount;

    private void OnEnable()
    {
        _goldCount.text = "Gold: " + _gameController.Gold.ToString();
        _gameController.GoldChanged += OnGoldChanged;
    }

    private void OnDisable()
    {
        _gameController.GoldChanged -= OnGoldChanged;
    }

    private void OnGoldChanged()
    {
        _goldCount.text = "Gold: " + _gameController.Gold.ToString();
    }
}

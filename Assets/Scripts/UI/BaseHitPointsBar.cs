using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitPointsBar : Bar
{
    [SerializeField] private GameController _gameController;

    private void OnEnable()
    {
        _gameController.BaseHitPointsChanged += OnValueChanged;
        Slider.value = 1;
    }

    private void OnDisable()
    {
        _gameController.BaseHitPointsChanged -= OnValueChanged;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWave : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Button _nextWaveButton;
    [SerializeField] private GameObject _disablePanel;

    private void OnEnable()
    {
        _gameController.WaveClear += OnWaveClear;
        _gameController.NewUnitBuild += OnNewUnitBuild;
        _gameController.StyleSelected += OnStyleSelected;
        _nextWaveButton.onClick.AddListener(OnNextWaveButtonClick);
    }

    private void OnDisable()
    {
        _gameController.WaveClear -= OnWaveClear;
        _gameController.NewUnitBuild -= OnNewUnitBuild;
        _gameController.StyleSelected -= OnStyleSelected;
        _nextWaveButton.onClick.RemoveListener(OnNextWaveButtonClick);
    }

    public void OnWaveClear()
    {
        _nextWaveButton.gameObject.SetActive(true);
    }

    public void OnNextWaveButtonClick()
    {
        _gameController.SetFightPhase();
        _spawner.StartNextWave();
        _nextWaveButton.gameObject.SetActive(false);
    }

    private void OnNewUnitBuild()
    {
        _nextWaveButton.enabled = false;
        _disablePanel.SetActive(true);
    }

    private void OnStyleSelected()
    {
        _nextWaveButton.enabled = true;
        _disablePanel.SetActive(false);
    }
}

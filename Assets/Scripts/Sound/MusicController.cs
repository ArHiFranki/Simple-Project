using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _preparationPhaseMusic;
    [SerializeField] private AudioClip _fightPhaseMusic;

    private SettingsController _settingsController;
    private AudioSource _backgroundMusic;
    private const string _menuScene = "MenuScene";
    private const string _gameScene = "GameScene";
    private const string _settingsControllerName = "SettingsController";

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == _gameScene)
        {
            _gameController.StartPreparationPhase += OnPreparationPhase;
            _gameController.StartFightPhase += OnFightPhase;
        }
    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == _gameScene)
        {
            _gameController.StartPreparationPhase -= OnPreparationPhase;
            _gameController.StartFightPhase -= OnFightPhase;
        }
    }

    private void Awake()
    {
        _backgroundMusic = GetComponent<AudioSource>();
        _settingsController = GameObject.Find(_settingsControllerName).GetComponent<SettingsController>();
        _backgroundMusic.Stop();

        if (SceneManager.GetActiveScene().name == _menuScene)
        {
            PlayBackgroundMusic(_menuMusic, _settingsController.MusicVolume);
        }
    }

    private void PlayBackgroundMusic(AudioClip musicTheme, float volume = 0.5f)
    {
        _backgroundMusic.loop = true;
        _backgroundMusic.clip = musicTheme;
        _backgroundMusic.volume = volume;
        _backgroundMusic.Play();
    }

    public void PlayBackgroundMusic()
    {
        _backgroundMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        _backgroundMusic.Stop();
    }

    private void OnPreparationPhase()
    {
        PlayBackgroundMusic(_preparationPhaseMusic, _settingsController.MusicVolume);
    }

    private void OnFightPhase()
    {
        PlayBackgroundMusic(_fightPhaseMusic, _settingsController.MusicVolume);
    }
}

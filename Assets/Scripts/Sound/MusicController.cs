using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip _menuThemeSound;
    [SerializeField] private AudioClip _gameThemeSound;
    [SerializeField] private SettingsController _settingsController;

    private AudioSource _backgroundMusic;
    private const string _menuScene = "MenuScene";
    private const string _gameScene = "GameScene";
    private const string _settingsControllerName = "SettingsController";

    private void Awake()
    {
        _backgroundMusic = GetComponent<AudioSource>();
        _settingsController = GameObject.Find(_settingsControllerName).GetComponent<SettingsController>();
        _backgroundMusic.Stop();

        if (SceneManager.GetActiveScene().name == _menuScene)
        {
            PlayBackgroundMusic(_menuThemeSound, _settingsController.MusicVolume);
        }
        else if (SceneManager.GetActiveScene().name == _gameScene)
        {
            PlayBackgroundMusic(_gameThemeSound, _settingsController.MusicVolume);
        }
    }

    private void PlayBackgroundMusic(AudioClip musicTheme, float volume = 0.5f)
    {
        _backgroundMusic.loop = true;
        _backgroundMusic.clip = musicTheme;
        _backgroundMusic.volume = volume;
        _backgroundMusic.Play();
    }
}

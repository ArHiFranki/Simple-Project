using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFXController : MonoBehaviour
{
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _onMouseClickUISound;
    [SerializeField] private AudioClip _onMouseOverUISound;
    [SerializeField] private AudioClip _levelUpSound;

    private SettingsController _settingsController;
    private AudioSource _soundFX;

    private const string _settingsControllerName = "SettingsController";

    private void Awake()
    {
        _soundFX = GetComponent<AudioSource>();
        _settingsController = GameObject.Find(_settingsControllerName).GetComponent<SettingsController>();
    }

    public void PlayGameOverSound()
    {
        _soundFX.PlayOneShot(_gameOverSound, _settingsController.EffectsVolume);
    }

    public void PlayOnMouseClickUISound()
    {
        _soundFX.PlayOneShot(_onMouseClickUISound, _settingsController.EffectsVolume);
    }

    public void PlayOnMouseOverUISound()
    {
        _soundFX.PlayOneShot(_onMouseOverUISound, _settingsController.EffectsVolume);
    }

    public void PlayLevelUpSound()
    {
        _soundFX.PlayOneShot(_levelUpSound, _settingsController.EffectsVolume);
    }
}

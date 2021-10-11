using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _effectsVolume;

    public float MusicVolume => _musicVolume;
    public float EffectsVolume => _effectsVolume;

    private void Awake()
    {
        int objectsCount = FindObjectsOfType<SettingsController>().Length;

        if (objectsCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;
    }

    public void SetEffectsVolume(float value)
    {
        _effectsVolume = value;
    }
}

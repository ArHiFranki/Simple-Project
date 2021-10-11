using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] SoundFXController _soundFXController;

    public void OnPointerDown(PointerEventData eventData)
    {
        _soundFXController.PlayOnMouseClickUISound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _soundFXController.PlayOnMouseOverUISound();
    }
}

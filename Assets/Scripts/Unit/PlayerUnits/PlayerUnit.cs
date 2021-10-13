using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public bool isUnitAtack;
    public bool isUnitDefend;

    [Header("Меню выбора состояния")]
    public GameObject _statusSelectionMenu;

    [HideInInspector] public Transform _defendPoint;

    private Vector3 startPoint;


    public float distToDefendPoint;

    public int unitPrice;

    public override void Death()
    {
        Destroy(gameObject);
    }

    public void IsUnitAtack()
    {
        isUnitAtack = true;
        isUnitDefend = false;
        _statusSelectionMenu.SetActive(false);
        gameController.StartStyleSelectedEvent();
    }

    public void IsUnitDefend()
    {
        isUnitAtack = false;
        isUnitDefend = true;
        _statusSelectionMenu.SetActive(false);
    }

    public void ReturnToStartingPoint()
    {
        transform.position = startPoint;
    }

    public void PlaceUnit()
    {
        gameController.WaveClear += ReturnToStartingPoint;
        startPoint = transform.position;
    }
}

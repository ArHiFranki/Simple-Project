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
}

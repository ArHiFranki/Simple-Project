using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUnit : Unit
{
    public bool _isUnitAtack;
    public bool _isUnitDefend;

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
        _isUnitAtack = true;
        _isUnitDefend = false;
        _statusSelectionMenu.SetActive(false);
    }

    public void IsUnitDefend()
    {
        _isUnitAtack = false;
        _isUnitDefend = true;
        _statusSelectionMenu.SetActive(false);
    }
}

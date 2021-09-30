using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerUnit : Unit
{
    public bool isUnitAtack;
    public bool isUnitDefend;

    [Header("Меню выбора состояния")]
    public GameObject statusSelectionMenu;

    [Header("Точка которую нужно защищать")]
    public Transform defendPoint;

    public override void Death()
    {
        Debug.Log("PlayerUnit Death");
    }

    public void IsUnitAtack()
    {
        isUnitAtack = true;
        isUnitDefend = false;
        statusSelectionMenu.SetActive(false);
    }

    public void IsUnitDefend()
    {
        isUnitAtack = false;
        isUnitDefend = true;
        statusSelectionMenu.SetActive(false);
    }
}

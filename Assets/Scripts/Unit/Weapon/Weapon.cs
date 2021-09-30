using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] [Header("Воин у которого в руках оружие")] private Transform Warrior;

    private void OnTriggerEnter(Collider other)
    {
        

        if (Warrior.GetComponent<PlayerUnit>() && other.gameObject.TryGetComponent(out Enemy enemy))
        {
            //enemy.ApplyDamage(Warrior.GetComponent<PlayerUnit>()._damage);
            //enemy.ApplyDamage(10);
        }

        if (Warrior.GetComponent<Enemy>() && other.gameObject.TryGetComponent(out PlayerUnit playerUnit))
        {
            //playerUnit.ApplyDamage(Warrior.GetComponent<Enemy>()._damage);
            //playerUnit.ApplyDamage(10);
        }
    }
}

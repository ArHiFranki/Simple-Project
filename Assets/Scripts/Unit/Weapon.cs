using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Transform Warrior;

    private void Start()
    {
        Warrior = transform.root;
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (Warrior.GetComponent<PlayerUnit>() && other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage(Warrior.GetComponent<PlayerUnit>()._damage);
        }

        if (Warrior.GetComponent<Enemy>() && other.gameObject.TryGetComponent(out PlayerUnit playerUnit))
        {
            playerUnit.ApplyDamage(Warrior.GetComponent<Enemy>()._damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] [Header("Воин у которого в руках оружие")] private Transform _warrior;

    private void OnTriggerEnter(Collider other)
    {
        if (_warrior.GetComponent<PlayerUnit>() && other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage(_warrior.GetComponent<PlayerUnit>().damage);
        }

        if (_warrior.GetComponent<Enemy>() && other.gameObject.TryGetComponent(out PlayerUnit playerUnit))
        {
            playerUnit.ApplyDamage(_warrior.GetComponent<Enemy>().damage);
        }

        if (_warrior.GetComponent<Enemy>() && other.gameObject.TryGetComponent(out Base baseObject))
        {
            GameController tmpController = baseObject.GetComponent<Base>().gameController;
            tmpController.ApplyDamage(_warrior.GetComponent<Enemy>().damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private int _healthMax;
    [SerializeField] private int _currentHealth;
    [SerializeField] public int _damage;

    protected Animator animator;

    public abstract void Death();

    private void Awake()
    {
        _currentHealth = _healthMax;
        animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int damage)
    {        
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    public void AddHealth(int value)
    {
        if (_currentHealth < _healthMax)
        {
            _currentHealth += value;

            if (_currentHealth > _healthMax)
            {
                _currentHealth = _healthMax;
            }
        }
    }

    public Transform FindClosetUnit(PlayerUnit[] PlayerUnits)//Поиск и возвращение ближайшего юнита игрока
    {
        float distance = Mathf.Infinity;
        Transform closetUnit = null; //Ближайший 

        foreach (PlayerUnit Unit in PlayerUnits) //перебор всех юнитов
        {
            if (Vector3.Distance(Unit.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(Unit.transform.position, transform.position);
                closetUnit = Unit.transform;
            }
        }
        return closetUnit;
    }

    public Transform FindClosetUnit(Enemy[] PlayerUnits)//Поиск и возвращение ближайшего юнита врага
    {

        float distance = Mathf.Infinity;
        Transform closetUnit = null; //Ближайший 

        foreach (Enemy Unit in PlayerUnits) //перебор всех юнитов
        {
            if (Vector3.Distance(Unit.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(Unit.transform.position, transform.position);
                closetUnit = Unit.transform;
            }
        }
        return closetUnit;
    }

    public Transform FindClosetBace(Base[] baseies)//Поиск и возвращение ближайшой базы
    {

        float distance = Mathf.Infinity;
        Transform ClosetBase = null; //Ближайший 

        foreach (Base _base in baseies) //перебор всех баз
        {
            if (Vector3.Distance(_base.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(_base.transform.position, transform.position);
                ClosetBase = _base.transform;
            }
        }
        return ClosetBase;
    }
}

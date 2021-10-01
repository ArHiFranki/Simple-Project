using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private int _healthMax;
    [SerializeField] private int _currentHealth;

    public int damage;

    protected Animator _animator;

    public abstract void Death();

    private void Awake()
    {
        _currentHealth = _healthMax;
        _animator = GetComponent<Animator>();
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

    public Transform FindNearestObject(MonoBehaviour[] gameObjects)
    {
        float distance = Mathf.Infinity;
        Transform nearestObject = null;

        foreach (MonoBehaviour gameObject in gameObjects)
        {
            if (Vector3.Distance(gameObject.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(gameObject.transform.position, transform.position);
                nearestObject = gameObject.transform;
            }
        }
        return nearestObject;
    }
}

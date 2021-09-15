using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] private int _healthMax;
    [SerializeField] private int _damage;
    [SerializeField] private float _deahtAnimationDuration;

    private int _currentHealth;
    private Animator _animator;

    private const string _deathAnimationTrigger = "Die";

    private void Start()
    {
        _currentHealth = _healthMax;
        _animator = GetComponent<Animator>();
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    public void AddHealth(int value)
    {
        if (_currentHealth < _healthMax)
        {
            _currentHealth += value;

            if(_currentHealth > _healthMax)
            {
                _currentHealth = _healthMax;
            }
        }
    }

    IEnumerator Death()
    {
        _animator.SetTrigger(_deathAnimationTrigger);
        yield return new WaitForSeconds(_deahtAnimationDuration);
        Destroy(gameObject);
    }
}

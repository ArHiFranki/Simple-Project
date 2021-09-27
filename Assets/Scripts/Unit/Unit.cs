using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField][Header("������������ ������� HP")] private int _healthMax;
    [SerializeField] [Header("������� ������� HP")] private int _currentHealth;
    [SerializeField] [Header("����")] public int _damage;
    [SerializeField] private float _deahtAnimationDuration;

    [HideInInspector]public Animator _animator;

    private const string _deathAnimationTrigger = "Die";

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
            //StartCoroutine(Death());
           // Destroy(gameObject);
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

    IEnumerator Death()
    {
       // _animator.SetTrigger(_deathAnimationTrigger);
        yield return new WaitForSeconds(_deahtAnimationDuration);
        Destroy(gameObject);
    }

    public Transform FindClosetUnit(PlayerUnit[] PlayerUnits)//����� � ����������� ���������� ����� ������
    {
        float distance = Mathf.Infinity;
        Transform closetUnit = null; //��������� 

        foreach (PlayerUnit Unit in PlayerUnits) //������� ���� ������
        {
            if (Vector3.Distance(Unit.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(Unit.transform.position, transform.position);
                closetUnit = Unit.transform;
            }
        }
        return closetUnit;
    }

    public Transform FindClosetUnit(Enemy[] PlayerUnits)//����� � ����������� ���������� ����� �����
    {

        float distance = Mathf.Infinity;
        Transform closetUnit = null; //��������� 

        foreach (Enemy Unit in PlayerUnits) //������� ���� ������
        {
            if (Vector3.Distance(Unit.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(Unit.transform.position, transform.position);
                closetUnit = Unit.transform;
            }
        }
        return closetUnit;
    }

    public Transform FindClosetBace(Base[] baseies)//����� � ����������� ��������� ����
    {

        float distance = Mathf.Infinity;
        Transform ClosetBase = null; //��������� 

        foreach (Base _base in baseies) //������� ���� ���
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

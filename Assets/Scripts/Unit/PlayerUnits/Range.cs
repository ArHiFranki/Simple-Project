using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    [Header("������ �������")]
    public GameObject bulletPrefab;

    [Header("������")]
    public GameObject bulletStartPosition;

    [SerializeField] [Header("����������� ��������� �� �������� ����� �������")]
    private float _distanceMin;

    [SerializeField] [Header("����������")]
    private float _radius;

    [SerializeField] [Header("����������������")]
    private float _fireRate;

    [SerializeField] [Header("�������� ����")] 
    private float _bulletSpeed;

    private NavMeshAgent _navMeshAgent;

    private float _nearestEnemyDistance;

    private float _timeAfterLastShot;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        // ����������� �� ������
        _statusSelectionMenu.transform.LookAt(Camera.main.transform);
        _timeAfterLastShot += Time.deltaTime;

        if (!gameController.IsPreparationPhase) {
            if (_isUnitAtack) AtackStyle();
            if (_isUnitDefend && _defendPoint != null) DefenseStyle();
        }
    }

    private void AtackStyle()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closedEnemy = FindNearestObject(enemies);

        if (closedEnemy != null)
        {
            _nearestEnemyDistance = Vector3.Distance(closedEnemy.position, transform.position);
        }
        else return;

        // ���� ���� � ��������
        if (_nearestEnemyDistance > _distanceMin) 
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(closedEnemy.position);
        }
        // ������� ��������� �����
        if (_nearestEnemyDistance < _distanceMin)
        {
            transform.LookAt(closedEnemy);
            _navMeshAgent.enabled = false;
            // _animator.SetTrigger("Atack");
            arrowCreate(closedEnemy);
        }
    }

    private void DefenseStyle()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closedEnemy = FindNearestObject(enemies);
         
        if(closedEnemy != null)
        {
          _nearestEnemyDistance = Vector3.Distance(closedEnemy.position, transform.position);
        }

        float distToDefendPoint = Vector3.Distance(_defendPoint.position, transform.position);

        if (distToDefendPoint > 1 && closedEnemy != null)
        {
            // ���� � ����� ������
            if (_nearestEnemyDistance > _radius)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(_defendPoint.position);
            }
            // ���� � ���������� �����
            else if (_nearestEnemyDistance < _radius)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(closedEnemy.position);
            }

            // ������� ���������� �����
            if (_nearestEnemyDistance < _distanceMin)
            {
                transform.LookAt(closedEnemy);
                _navMeshAgent.enabled = false;
                // _animator.SetTrigger("Atack");
                arrowCreate(closedEnemy);
            }
        }
        else if (distToDefendPoint > 1 && closedEnemy == null)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_defendPoint.position);
        }
        else if (distToDefendPoint < 1 && closedEnemy != null && _nearestEnemyDistance < _distanceMin)
        {
            transform.LookAt(closedEnemy);
            _navMeshAgent.enabled = false;
            // _animator.SetTrigger("Atack");
            arrowCreate(closedEnemy);
        }
    }

    private void arrowCreate(Transform closedEnemy)
    {
        if (_timeAfterLastShot >= _fireRate) {
            GameObject newArrow = Instantiate(bulletPrefab, bulletStartPosition.transform.position, transform.rotation);
            Arrow arrow = newArrow.GetComponent<Arrow>();
            arrow.targetOfArrow = closedEnemy;
            arrow.arrowSpeed = _bulletSpeed;
            arrow.damage = damage;
            _timeAfterLastShot = 0;
        }
    }
}

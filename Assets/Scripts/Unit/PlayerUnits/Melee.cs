using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Melee : PlayerUnit
{
    private NavMeshAgent _navMeshAgent;
    private float _nearestEnemyDistance;

    [SerializeField] [Header("����������� ��������� �� �������� ����� �������")]
    private float _minimalDistance;

    [SerializeField] [Header("����������")] 
    private float _radius;
   
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _statusSelectionMenu.transform.LookAt(Camera.main.transform);

        if (gameController.IsPreparationPhase != true)
        {
            if (isUnitAtack) AtackStyle();
            if (isUnitDefend) DefenseStyle();
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
        if (_nearestEnemyDistance > _minimalDistance)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(closedEnemy.position);
        }

        // ������� ��������� �����
        if (_nearestEnemyDistance < _minimalDistance)
        {
            transform.LookAt(closedEnemy); 
            _navMeshAgent.enabled = false;
            _animator.SetTrigger("Atack");
            // _arrowCreate(ClosedEnemy);
        }
    }

    private void DefenseStyle()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closedEnemy = FindNearestObject(enemies);

        if (closedEnemy != null)
        {
            _nearestEnemyDistance = Vector3.Distance(closedEnemy.position, transform.position);
        }

        float distToDefendPoint = Vector3.Distance(_defendPoint.position, transform.position);

        if (distToDefendPoint > this.distToDefendPoint && closedEnemy != null)
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
            if (_nearestEnemyDistance < _minimalDistance)
            {
                transform.LookAt(closedEnemy);
                _navMeshAgent.enabled = false;
                _animator.SetTrigger("Atack");
                // _arrowCreate(ClosedEnemy);
            }
        }
        else if (distToDefendPoint > this.distToDefendPoint && closedEnemy == null)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_defendPoint.position);
        }
        else if (distToDefendPoint < this.distToDefendPoint && closedEnemy != null && _nearestEnemyDistance < _minimalDistance)
        {
            transform.LookAt(closedEnemy);
            _navMeshAgent.enabled = false;
            _animator.SetTrigger("Atack");
            // _arrowCreate(ClosedEnemy);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Melee : PlayerUnit
{
    private NavMeshAgent _navMeshAgent;
    private float _nearestEnemyDistance;

    [SerializeField] [Header("Минимальная дистанция до ктоторой можно подойти")]
    private float _minimalDistance;

    [SerializeField] [Header("Агрорадиус")] 
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

        // Если юнит в агрозоне
        if (_nearestEnemyDistance > _minimalDistance)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(closedEnemy.position);
        }

        // Условия остановки врага
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
            // Идет к точке защиты
            if (_nearestEnemyDistance > _radius)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(_defendPoint.position);
            }
            // Идет к вражескому юниту
            else if (_nearestEnemyDistance < _radius)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(closedEnemy.position);
            }

            // Атакует вражеского юнита
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

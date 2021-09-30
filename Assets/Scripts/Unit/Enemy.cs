using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    [SerializeField] [Header("Награда")] private int _reward;
    [SerializeField] private float _distanceMin;
    [SerializeField] private float _visibilityRadius;

    private GameController _gameController;
    private NavMeshAgent _navMeshAgent;
    private float _nearestPlayerUnitDistance;
    private float _nearestBaseDistance;

    private const string _attackTriggerName = "Atack";

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyMovement();
    }

    public override void Death()
    {
        _gameController.ChangeGold(_reward);
    }

    private void EnemyMovement()
    {
        PlayerUnit[] playerUnits = FindObjectsOfType<PlayerUnit>();
        Base[] baseObjects = FindObjectsOfType<Base>();

        Transform nearestUnit = FindNearestObject(playerUnits);
        Transform nearestBase = FindNearestObject(baseObjects);

        if (nearestUnit != null) 
        {
            // расчет дистанции до ближайшего юнита
            _nearestPlayerUnitDistance = Vector3.Distance(nearestUnit.position, transform.position);
        }

        // расчет дистанции до базы
        _nearestBaseDistance = Vector3.Distance(nearestBase.position, transform.position);

        // Если юнит игрока вне агрозоны или его нет, то он идёт к базе, иначе идёт к юниту игрока
        if (nearestUnit == null || _nearestPlayerUnitDistance > _visibilityRadius) 
        {
           _navMeshAgent.enabled = true;
           _navMeshAgent.SetDestination(nearestBase.position);
        }
        else if (_nearestPlayerUnitDistance < _visibilityRadius && _nearestPlayerUnitDistance > _distanceMin)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(nearestUnit.position);
        }

        //Условия остановки врага
        if (_nearestPlayerUnitDistance < _distanceMin && nearestUnit != null)
        {
            transform.LookAt(nearestUnit);
            _navMeshAgent.enabled = false;
            animator.SetTrigger(_attackTriggerName);
        }
        if (_nearestBaseDistance < _distanceMin && (nearestUnit == null || _nearestPlayerUnitDistance > _visibilityRadius))
        {
            transform.LookAt(nearestBase);
            _navMeshAgent.enabled = false;
            animator.SetTrigger(_attackTriggerName);
        }
    }

    public void InitEnemy(GameController gameController)
    {
        _gameController = gameController;
    }
}

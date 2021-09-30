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

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        EnemyMovment();
    }

    public override void Death()
    {
        _gameController.ChangeGold(_reward);
    }

    private void EnemyMovment() //Перемещение врага
    {
        //Поиск ближайшего юнита
        PlayerUnit[] PlayerUnits = FindObjectsOfType<PlayerUnit>();//Поиск всех объектов с компонентом PlayerUnit

        Transform ClosedUnit = FindNearestObject(PlayerUnits);//ближайший юнит

        Base[] _base = FindObjectsOfType<Base>();//Поиск всех объектов с компонентом Base

        Transform ClosetBase = FindNearestObject(_base);

        if (ClosedUnit != null) //если на сцене есть юниты игрока
        {
            _nearestPlayerUnitDistance = Vector3.Distance(ClosedUnit.position, transform.position); //расчет дистанции до ближайшего юнита
        }

        _nearestBaseDistance = Vector3.Distance(ClosetBase.position, transform.position); //расчет дистанции до базы

        if (ClosedUnit == null || _nearestPlayerUnitDistance > _visibilityRadius) //Если юнит игрока вне агрозоны или его нет
        {
           _navMeshAgent.enabled = true;
           _navMeshAgent.SetDestination(ClosetBase.position); //идет к базе 
        }
        else if (_nearestPlayerUnitDistance < _visibilityRadius && _nearestPlayerUnitDistance > _distanceMin) //Если юнит в агрозоне
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(ClosedUnit.position);//Идет к юниту игрока
          
        }

        //Условия остановки врага
        if (_nearestPlayerUnitDistance < _distanceMin && ClosedUnit != null)
        {
            transform.LookAt(ClosedUnit); //Смотреть на Юнита
            _navMeshAgent.enabled = false;
            animator.SetTrigger("Atack");


        }
        if (_nearestBaseDistance < 2f && (ClosedUnit == null || _nearestPlayerUnitDistance > _visibilityRadius))
        {
            transform.LookAt(ClosetBase); //Смотреть на базу
            _navMeshAgent.enabled = false;
            animator.SetTrigger("Atack");
        }
    }

    public void InitEnemy(GameController gameController)
    {
        _gameController = gameController;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    [Header("Префаб снаряда")]
    public GameObject bulletPrefab;

    [Header("Оружие")]
    public GameObject bulletStartPosition;

    [SerializeField] [Header("Минимальная дистанция до ктоторой можно подойти")]
    private float _distanceMin;

    [SerializeField] [Header("Агрорадиус")]
    private float _radius;

    [SerializeField] [Header("Скорострельность")]
    private float _fireRate;

    [SerializeField] [Header("Скорость пули")] 
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
        // Направление на камеру
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

        // Если юнит в агрозоне
        if (_nearestEnemyDistance > _distanceMin) 
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(closedEnemy.position);
        }
        // Условия остановки врага
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

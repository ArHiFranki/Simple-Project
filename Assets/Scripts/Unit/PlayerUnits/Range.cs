using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    [SerializeField]
    [Header("Минимальная дистанция до ктоторой можно подойти")]
    private float _distanceMin;

    [SerializeField]
    [Header("Агрорадиус")]
    float _radius;

    [SerializeField] [Header("Скорострельность")]
    float _fireRate;

    [Header("Скорость пули")] 
    public float _bulletSpeed;

    private NavMeshAgent _navMeshAgent;
    private float _nearestEnemyDistance;
    
    [Header("Префаб снаряда")]
    public GameObject _bulletPrefab;

    [Header("Оружие")]
    public GameObject _bulletStartPosition;
   
    //время после последнего выстрела
    private float _timeAfterLastShot; 
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }



    void Update()
    {
        //направление на камеру
        _statusSelectionMenu.transform.LookAt(Camera.main.transform);

        _timeAfterLastShot += Time.deltaTime;
        if (_isUnitAtack) AtackStyle();
        if (_isUnitDefend && _defendPoint != null) DefenseStyle();
    }

    //Атака
    private void AtackStyle()
    {
        //Поиск ближайшего юнита
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        Transform ClosedEnemy = FindNearestObject(enemies);//ближайший враг

        if (ClosedEnemy != null) //если на сцене есть юниты игрока
        {
            _nearestEnemyDistance = Vector3.Distance(ClosedEnemy.position, transform.position); //расчет дистанции до ближайшего врага
        }
        else return;

        if (_nearestEnemyDistance > _distanceMin) //Если юнит в агрозоне
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(ClosedEnemy.position);//Идет к вражескому юниту
        }
        //Условия остановки врага
        if (_nearestEnemyDistance < _distanceMin)
        {
            transform.LookAt(ClosedEnemy);
            _navMeshAgent.enabled = false;
            //_animator.SetTrigger("Atack");
            arrowCreate(ClosedEnemy);

        }
    }

    private void DefenseStyle()//Защита
    {

        Enemy[] Enemies = FindObjectsOfType<Enemy>();//Поиск всех объектов с компонентом Enemy

        Transform ClosedEnemy = FindNearestObject(Enemies);//ближайший враг
         
        if(ClosedEnemy != null)
        {
          _nearestEnemyDistance = Vector3.Distance(ClosedEnemy.position, transform.position); //расчет дистанции до ближайшего врага
        }

        float distToDefendPoint = Vector3.Distance(_defendPoint.position, transform.position); //дистанция дозащишаемой точки

        if (distToDefendPoint > 1 && ClosedEnemy != null)
        {
            if (_nearestEnemyDistance > _radius)//Идет к точке защиты
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(_defendPoint.position);
            }
            else if (_nearestEnemyDistance < _radius)//Идет к вражескому юниту
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(ClosedEnemy.position);
            }

            if (_nearestEnemyDistance < _distanceMin)//Атакует вражеского юнита
            {
                transform.LookAt(ClosedEnemy);
                _navMeshAgent.enabled = false;
                //_animator.SetTrigger("Atack");
                arrowCreate(ClosedEnemy);
            }
        }
        else if (distToDefendPoint > 1 && ClosedEnemy == null)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_defendPoint.position);
        }
        else if (distToDefendPoint < 1 && ClosedEnemy != null && _nearestEnemyDistance < _distanceMin)
        {
            transform.LookAt(ClosedEnemy);
            _navMeshAgent.enabled = false;
            //_animator.SetTrigger("Atack");
            arrowCreate(ClosedEnemy);
        }
    }

    private void arrowCreate(Transform closedEnemy)
    {
        if (_timeAfterLastShot >= _fireRate) {
            GameObject NewArrow = Instantiate(_bulletPrefab, _bulletStartPosition.transform.position, transform.rotation); //создать 
            Arrow arrow = NewArrow.GetComponent<Arrow>();
            arrow.Target = closedEnemy;
            arrow._speed = _bulletSpeed;
            arrow.Damage = _damage;
           
            _timeAfterLastShot = 0;
        }
    }
}

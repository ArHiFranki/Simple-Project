using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    

    private NavMeshAgent nav;//Навмеш
    private float distToEnemy; //дистанция до Ближайшего врага

    [SerializeField] [Header("Минимальная дистанция до ктоторой можно подойти")] private float minimalDistance;
    [SerializeField] [Header("Агрорадиус")] float radius;
    
    [Header("Префаб снаряда")] public GameObject BulletPrefab;
    [Header("Оружие")] public GameObject BulletStartPosition;
    [Header("Скорость пули")] public float bulletSpeed;
    [SerializeField] [Header("Скорострельность")] float fireRate;
    private float timeAfterLastShot; //время после последнего выстрела
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
       // _animator = GetComponent<Animator>();
    }



    void Update()
    {
        statusSelectionMenu.transform.LookAt(Camera.main.transform);//направление на камеру

        timeAfterLastShot += Time.deltaTime;
        if (isUnitAtack) AtackStyle();
        if (isUnitDefend && defendPoint != null) DefenseStyle();
    }

    private void AtackStyle() //Атака
    {
        //Поиск ближайшего юнита
        Enemy[] Enemies = FindObjectsOfType<Enemy>();//Поиск всех объектов с компонентом Enemy

        Transform ClosedEnemy = FindNearestObject(Enemies);//ближайший враг

        if (ClosedEnemy != null) //если на сцене есть юниты игрока
        {
            distToEnemy = Vector3.Distance(ClosedEnemy.position, transform.position); //расчет дистанции до ближайшего врага
        }
        else return;

        if (distToEnemy > minimalDistance) //Если юнит в агрозоне
        {
            nav.enabled = true;
            nav.SetDestination(ClosedEnemy.position);//Идет к вражескому юниту
        }
        //Условия остановки врага
        if (distToEnemy < minimalDistance)
        {
            transform.LookAt(ClosedEnemy);
            nav.enabled = false;
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
          distToEnemy = Vector3.Distance(ClosedEnemy.position, transform.position); //расчет дистанции до ближайшего врага
        }

        float distToDefendPoint = Vector3.Distance(defendPoint.position, transform.position); //дистанция дозащишаемой точки

        if (distToDefendPoint > 1 && ClosedEnemy != null)
        {
            if (distToEnemy > radius)//Идет к точке защиты
            {
                nav.enabled = true;
                nav.SetDestination(defendPoint.position);
            }
            else if (distToEnemy < radius)//Идет к вражескому юниту
            {
                nav.enabled = true;
                nav.SetDestination(ClosedEnemy.position);
            }

            if (distToEnemy < minimalDistance)//Атакует вражеского юнита
            {
                transform.LookAt(ClosedEnemy);
                nav.enabled = false;
                //_animator.SetTrigger("Atack");
                arrowCreate(ClosedEnemy);
            }
        }
        else if (distToDefendPoint > 1 && ClosedEnemy == null)
        {
            nav.enabled = true;
            nav.SetDestination(defendPoint.position);
        }
        else if (distToDefendPoint < 1 && ClosedEnemy != null && distToEnemy < minimalDistance)
        {
            transform.LookAt(ClosedEnemy);
            nav.enabled = false;
            //_animator.SetTrigger("Atack");
            arrowCreate(ClosedEnemy);
        }
    }

    private void arrowCreate(Transform closedEnemy)
    {
        if (timeAfterLastShot >= fireRate) {
            GameObject NewArrow = Instantiate(BulletPrefab, BulletStartPosition.transform.position, transform.rotation); //создать 
            Arrow arrow = NewArrow.GetComponent<Arrow>();
            arrow.Target = closedEnemy;
            arrow._speed = bulletSpeed;
            arrow.Damage = _damage;
           
            timeAfterLastShot = 0;
        }
    }
}

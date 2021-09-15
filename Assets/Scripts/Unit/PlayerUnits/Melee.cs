using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Melee : PlayerUnit
{
    private NavMeshAgent nav;//Навмеш
    private float distToEnemy; //дистанция до Ближайшего врага

    [SerializeField] [Header("Минимальная дистанция до ктоторой можно подойти")]  private float minimalDistance;
    [SerializeField] [Header("Агрорадиус")] float radius;
    [SerializeField] bool isUnitAtack;
    [SerializeField] bool isUnitDefend;
    [SerializeField] [Header("Точка которую нужно защищать")] Transform defendPoint;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }



    void Update()
    {
        if(isUnitAtack) AtackStyle();
        if(isUnitDefend) DefenseStyle();
    }

    private void AtackStyle() //Атака
    {
        //Поиск ближайшего юнита
        Enemy[] Enemies = FindObjectsOfType<Enemy>();//Поиск всех объектов с компонентом Enemy

        Transform ClosedEnemy = FindClosetUnit(Enemies);//ближайший враг

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
            _animator.SetTrigger("Atack");
        }
    }

    private void DefenseStyle()//Защита
    {
       
        Enemy[] Enemies = FindObjectsOfType<Enemy>();//Поиск всех объектов с компонентом Enemy
        Transform ClosedEnemy = FindClosetUnit(Enemies);//ближайший враг
        distToEnemy = Vector3.Distance(ClosedEnemy.position, transform.position); //расчет дистанции до ближайшего врага
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
                _animator.SetTrigger("Atack");
            }
        }else if(distToDefendPoint < 1 && distToEnemy < minimalDistance)
        {
            transform.LookAt(ClosedEnemy);
            nav.enabled = false;
            _animator.SetTrigger("Atack");
        }
    }
}

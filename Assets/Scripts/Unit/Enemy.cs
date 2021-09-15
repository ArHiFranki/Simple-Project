using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    private NavMeshAgent nav;//Навмеш
    private GameObject Base; //Цель которую нужно атаковать
    private float distToPlayerUnit; //дистанция до Ближайшего Юнита игрока
    private float distToBase;//Дистанция до базы

    [Header("Минимальная дистанция до ктоторой можно подойти")] [SerializeField] private float minimalDistance;
    [SerializeField] [Header("Агрозона врага")] private float radius; //радиус видимости врага


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        Base = GameObject.FindGameObjectWithTag("Finish");
        _animator = GetComponent<Animator>();

    }


    void Update()
    {
        EnemyMovment();
    }

    private void EnemyMovment() //Перемещение врага
    {
        //Поиск ближайшего юнита
        PlayerUnit[] PlayerUnits = FindObjectsOfType<PlayerUnit>();//Поиск всех объектов с компонентом PlayerUnit

        Transform ClosedUnit = FindClosetUnit(PlayerUnits);//ближайший юнит

        if (ClosedUnit != null) //если на сцене есть юниты игрока
        {
            distToPlayerUnit = Vector3.Distance(ClosedUnit.position, transform.position); //расчет дистанции до ближайшего юнита
        }

        distToBase = Vector3.Distance(Base.transform.position, transform.position); //расчет дистанции до базы

        if (ClosedUnit == null || distToPlayerUnit > radius) //Если юнит игрока вне агрозоны или его нет
        {
           nav.enabled = true;
           nav.SetDestination(Base.transform.position); //идет к базе 
        }
        else if (distToPlayerUnit < radius && distToPlayerUnit > minimalDistance) //Если юнит в агрозоне
        {
            nav.enabled = true;
            nav.SetDestination(ClosedUnit.position);//Идет к юниту игрока
          
        }

        //Условия остановки врага
        if (distToPlayerUnit < minimalDistance && ClosedUnit != null)
        {
            transform.LookAt(ClosedUnit); //Смотреть на Юнита
            nav.enabled = false;
            _animator.SetTrigger("Atack");


        }
        if (distToBase < 2f && (ClosedUnit == null || distToPlayerUnit > radius))
        {
            transform.LookAt(Base.transform); //Смотреть на базу
            nav.enabled = false;
            _animator.SetTrigger("Atack");
        }
    }



}

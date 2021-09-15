using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    private NavMeshAgent nav;//������
    private GameObject Base; //���� ������� ����� ���������
    private float distToPlayerUnit; //��������� �� ���������� ����� ������
    private float distToBase;//��������� �� ����

    [Header("����������� ��������� �� �������� ����� �������")] [SerializeField] private float minimalDistance;
    [SerializeField] [Header("�������� �����")] private float radius; //������ ��������� �����


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

    private void EnemyMovment() //����������� �����
    {
        //����� ���������� �����
        PlayerUnit[] PlayerUnits = FindObjectsOfType<PlayerUnit>();//����� ���� �������� � ����������� PlayerUnit

        Transform ClosedUnit = FindClosetUnit(PlayerUnits);//��������� ����

        if (ClosedUnit != null) //���� �� ����� ���� ����� ������
        {
            distToPlayerUnit = Vector3.Distance(ClosedUnit.position, transform.position); //������ ��������� �� ���������� �����
        }

        distToBase = Vector3.Distance(Base.transform.position, transform.position); //������ ��������� �� ����

        if (ClosedUnit == null || distToPlayerUnit > radius) //���� ���� ������ ��� �������� ��� ��� ���
        {
           nav.enabled = true;
           nav.SetDestination(Base.transform.position); //���� � ���� 
        }
        else if (distToPlayerUnit < radius && distToPlayerUnit > minimalDistance) //���� ���� � ��������
        {
            nav.enabled = true;
            nav.SetDestination(ClosedUnit.position);//���� � ����� ������
          
        }

        //������� ��������� �����
        if (distToPlayerUnit < minimalDistance && ClosedUnit != null)
        {
            transform.LookAt(ClosedUnit); //�������� �� �����
            nav.enabled = false;
            _animator.SetTrigger("Atack");


        }
        if (distToBase < 2f && (ClosedUnit == null || distToPlayerUnit > radius))
        {
            transform.LookAt(Base.transform); //�������� �� ����
            nav.enabled = false;
            _animator.SetTrigger("Atack");
        }
    }



}

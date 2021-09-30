using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    private NavMeshAgent nav;//������
    //private GameObject Base; //���� ������� ����� ���������
    private float distToPlayerUnit; //��������� �� ���������� ����� ������
    private float distToBase;//��������� �� ����

    [Header("����������� ��������� �� �������� ����� �������")] [SerializeField] private float minimalDistance;
    [SerializeField] [Header("�������� �����")] private float radius; //������ ��������� �����
    [SerializeField] private int _reward;
    [SerializeField] private GameController _gameController;


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }


    void Update()
    {
        EnemyMovment();
    }

    public override void Death()
    {
        _gameController.ChangeGold(_reward);
    }

    private void EnemyMovment() //����������� �����
    {
        //����� ���������� �����
        PlayerUnit[] PlayerUnits = FindObjectsOfType<PlayerUnit>();//����� ���� �������� � ����������� PlayerUnit

        Transform ClosedUnit = FindClosetUnit(PlayerUnits);//��������� ����

        Base[] _base = FindObjectsOfType<Base>();//����� ���� �������� � ����������� Base

        Transform ClosetBase = FindClosetBace(_base);

        if (ClosedUnit != null) //���� �� ����� ���� ����� ������
        {
            distToPlayerUnit = Vector3.Distance(ClosedUnit.position, transform.position); //������ ��������� �� ���������� �����
        }

        distToBase = Vector3.Distance(ClosetBase.position, transform.position); //������ ��������� �� ����

        if (ClosedUnit == null || distToPlayerUnit > radius) //���� ���� ������ ��� �������� ��� ��� ���
        {
           nav.enabled = true;
           nav.SetDestination(ClosetBase.position); //���� � ���� 
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
            transform.LookAt(ClosetBase); //�������� �� ����
            nav.enabled = false;
            _animator.SetTrigger("Atack");
        }
    }

    public void InitEnemy(GameController gameController)
    {
        _gameController = gameController;
    }
}

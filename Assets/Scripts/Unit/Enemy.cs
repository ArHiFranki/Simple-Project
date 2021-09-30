using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{
    [SerializeField] [Header("�������")] private int _reward;
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

    private void EnemyMovment() //����������� �����
    {
        //����� ���������� �����
        PlayerUnit[] PlayerUnits = FindObjectsOfType<PlayerUnit>();//����� ���� �������� � ����������� PlayerUnit

        Transform ClosedUnit = FindNearestObject(PlayerUnits);//��������� ����

        Base[] _base = FindObjectsOfType<Base>();//����� ���� �������� � ����������� Base

        Transform ClosetBase = FindNearestObject(_base);

        if (ClosedUnit != null) //���� �� ����� ���� ����� ������
        {
            _nearestPlayerUnitDistance = Vector3.Distance(ClosedUnit.position, transform.position); //������ ��������� �� ���������� �����
        }

        _nearestBaseDistance = Vector3.Distance(ClosetBase.position, transform.position); //������ ��������� �� ����

        if (ClosedUnit == null || _nearestPlayerUnitDistance > _visibilityRadius) //���� ���� ������ ��� �������� ��� ��� ���
        {
           _navMeshAgent.enabled = true;
           _navMeshAgent.SetDestination(ClosetBase.position); //���� � ���� 
        }
        else if (_nearestPlayerUnitDistance < _visibilityRadius && _nearestPlayerUnitDistance > _distanceMin) //���� ���� � ��������
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(ClosedUnit.position);//���� � ����� ������
          
        }

        //������� ��������� �����
        if (_nearestPlayerUnitDistance < _distanceMin && ClosedUnit != null)
        {
            transform.LookAt(ClosedUnit); //�������� �� �����
            _navMeshAgent.enabled = false;
            animator.SetTrigger("Atack");


        }
        if (_nearestBaseDistance < 2f && (ClosedUnit == null || _nearestPlayerUnitDistance > _visibilityRadius))
        {
            transform.LookAt(ClosetBase); //�������� �� ����
            _navMeshAgent.enabled = false;
            animator.SetTrigger("Atack");
        }
    }

    public void InitEnemy(GameController gameController)
    {
        _gameController = gameController;
    }
}

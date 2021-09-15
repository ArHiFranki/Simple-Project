using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Melee : PlayerUnit
{
    private NavMeshAgent nav;//������
    private float distToEnemy; //��������� �� ���������� �����

    [SerializeField] [Header("����������� ��������� �� �������� ����� �������")]  private float minimalDistance;
    [SerializeField] [Header("����������")] float radius;
    [SerializeField] bool isUnitAtack;
    [SerializeField] bool isUnitDefend;
    [SerializeField] [Header("����� ������� ����� ��������")] Transform defendPoint;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }



    void Update()
    {
        if(isUnitAtack) AtackStyle();
        if(isUnitDefend) DefenseStyle();
    }

    private void AtackStyle() //�����
    {
        //����� ���������� �����
        Enemy[] Enemies = FindObjectsOfType<Enemy>();//����� ���� �������� � ����������� Enemy

        Transform ClosedEnemy = FindClosetUnit(Enemies);//��������� ����

        if (ClosedEnemy != null) //���� �� ����� ���� ����� ������
        {
            distToEnemy = Vector3.Distance(ClosedEnemy.position, transform.position); //������ ��������� �� ���������� �����
        }
        else return;
       
        if (distToEnemy > minimalDistance) //���� ���� � ��������
        {
            nav.enabled = true;
            nav.SetDestination(ClosedEnemy.position);//���� � ���������� �����
        }
        //������� ��������� �����
        if (distToEnemy < minimalDistance)
        {
            transform.LookAt(ClosedEnemy); 
            nav.enabled = false;
            _animator.SetTrigger("Atack");
        }
    }

    private void DefenseStyle()//������
    {
       
        Enemy[] Enemies = FindObjectsOfType<Enemy>();//����� ���� �������� � ����������� Enemy
        Transform ClosedEnemy = FindClosetUnit(Enemies);//��������� ����
        distToEnemy = Vector3.Distance(ClosedEnemy.position, transform.position); //������ ��������� �� ���������� �����
        float distToDefendPoint = Vector3.Distance(defendPoint.position, transform.position); //��������� ������������ �����

        if (distToDefendPoint > 1 && ClosedEnemy != null)
        {
            if (distToEnemy > radius)//���� � ����� ������
            {
                nav.enabled = true;
                nav.SetDestination(defendPoint.position);
            }
            else if (distToEnemy < radius)//���� � ���������� �����
            {
                nav.enabled = true;
                nav.SetDestination(ClosedEnemy.position);

            }
            
            if (distToEnemy < minimalDistance)//������� ���������� �����
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

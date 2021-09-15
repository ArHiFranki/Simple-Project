using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    

    private NavMeshAgent nav;//������
    private float distToEnemy; //��������� �� ���������� �����

    [SerializeField] [Header("����������� ��������� �� �������� ����� �������")] private float minimalDistance;
    [SerializeField] [Header("����������")] float radius;
    [SerializeField] bool isUnitAtack;
    [SerializeField] bool isUnitDefend;
    [SerializeField] [Header("����� ������� ����� ��������")] Transform defendPoint;


    [Header("������ �������")] public GameObject BulletPrefab;
    [Header("������")] public GameObject BulletStartPosition;
    [Header("�������� ����")] public float bulletSpeed;
    [SerializeField] [Header("����������������")] float fireRate;
    private float timeAfterLastShot; //����� ����� ���������� ��������
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
       // _animator = GetComponent<Animator>();
    }



    void Update()
    {
        timeAfterLastShot += Time.deltaTime;
        if (isUnitAtack) AtackStyle();
        if (isUnitDefend) DefenseStyle();
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
            //_animator.SetTrigger("Atack");
            arrowCreate(ClosedEnemy);

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
                //_animator.SetTrigger("Atack");
                arrowCreate(ClosedEnemy);
            }
        }
        else if (distToDefendPoint < 1 && distToEnemy < minimalDistance)
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
            GameObject NewArrow = Instantiate(BulletPrefab, BulletStartPosition.transform.position, transform.rotation); //������� 
            Arrow arrow = NewArrow.GetComponent<Arrow>();
            arrow.Target = closedEnemy;
            arrow._speed = bulletSpeed;
            arrow.Damage = _damage;
           
            timeAfterLastShot = 0;
        }
    }
}

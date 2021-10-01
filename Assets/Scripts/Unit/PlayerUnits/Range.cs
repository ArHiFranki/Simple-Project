using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : PlayerUnit
{
    [SerializeField]
    [Header("����������� ��������� �� �������� ����� �������")]
    private float _distanceMin;

    [SerializeField]
    [Header("����������")]
    float _radius;

    [SerializeField] [Header("����������������")]
    float _fireRate;

    [Header("�������� ����")] 
    public float _bulletSpeed;

    private NavMeshAgent _navMeshAgent;
    private float _nearestEnemyDistance;
    
    [Header("������ �������")]
    public GameObject _bulletPrefab;

    [Header("������")]
    public GameObject _bulletStartPosition;
   
    //����� ����� ���������� ��������
    private float _timeAfterLastShot; 
    
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }



    void Update()
    {
        //����������� �� ������
        _statusSelectionMenu.transform.LookAt(Camera.main.transform);

        _timeAfterLastShot += Time.deltaTime;
        if (_isUnitAtack) AtackStyle();
        if (_isUnitDefend && _defendPoint != null) DefenseStyle();
    }

    //�����
    private void AtackStyle()
    {
        //����� ���������� �����
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        Transform ClosedEnemy = FindNearestObject(enemies);//��������� ����

        if (ClosedEnemy != null) //���� �� ����� ���� ����� ������
        {
            _nearestEnemyDistance = Vector3.Distance(ClosedEnemy.position, transform.position); //������ ��������� �� ���������� �����
        }
        else return;

        if (_nearestEnemyDistance > _distanceMin) //���� ���� � ��������
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(ClosedEnemy.position);//���� � ���������� �����
        }
        //������� ��������� �����
        if (_nearestEnemyDistance < _distanceMin)
        {
            transform.LookAt(ClosedEnemy);
            _navMeshAgent.enabled = false;
            //_animator.SetTrigger("Atack");
            arrowCreate(ClosedEnemy);

        }
    }

    private void DefenseStyle()//������
    {

        Enemy[] Enemies = FindObjectsOfType<Enemy>();//����� ���� �������� � ����������� Enemy

        Transform ClosedEnemy = FindNearestObject(Enemies);//��������� ����
         
        if(ClosedEnemy != null)
        {
          _nearestEnemyDistance = Vector3.Distance(ClosedEnemy.position, transform.position); //������ ��������� �� ���������� �����
        }

        float distToDefendPoint = Vector3.Distance(_defendPoint.position, transform.position); //��������� ������������ �����

        if (distToDefendPoint > 1 && ClosedEnemy != null)
        {
            if (_nearestEnemyDistance > _radius)//���� � ����� ������
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(_defendPoint.position);
            }
            else if (_nearestEnemyDistance < _radius)//���� � ���������� �����
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(ClosedEnemy.position);
            }

            if (_nearestEnemyDistance < _distanceMin)//������� ���������� �����
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
            GameObject NewArrow = Instantiate(_bulletPrefab, _bulletStartPosition.transform.position, transform.rotation); //������� 
            Arrow arrow = NewArrow.GetComponent<Arrow>();
            arrow.Target = closedEnemy;
            arrow._speed = _bulletSpeed;
            arrow.Damage = _damage;
           
            _timeAfterLastShot = 0;
        }
    }
}

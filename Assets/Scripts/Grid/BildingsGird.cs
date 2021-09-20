using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildingsGird : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);//������ �����
    [SerializeField][Header("������� ���������� ������")]
    private int _deployGridSize;

    /*
    [SerializeField] private int _cursorPositionX;
    [SerializeField] private int _cursorPositionY;
    [SerializeField] private int _gridSizaX;
    [SerializeField] private int _gridSizeY;
    [SerializeField] private int _flyingBildingSizeX;
    [SerializeField] private int _flyingBildingSizeY;
    */
    private int _statePosition;
    private Building[,] gird;//��������� ������ ������
    private Building flyingBilding;//�������� (���������) ������ 
    private Camera mainCamera;//������

    private void Awake()
    {
        gird = new Building[GridSize.x, GridSize.y];//������������� �������

        mainCamera = Camera.main;
    }



    private void Update()
    {
        PlaceBuilding();//����������  ������

    }
    private void OnDrawGizmos()//��������� ����� ������� � ����� (��������������)
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Gizmos.color = new Color(0.88f, 0.5f, 0.3f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    public void StartPlacingBilding(Building BildingPrefab) //����� ��������� (����������) ������
    {
        if (flyingBilding != null) //���� ��� ���� �������� ������, ������� ���
        {
            Destroy(flyingBilding.gameObject);
        }

        flyingBilding = Instantiate(BildingPrefab);//�������� �������
    }

    private void PlaceBuilding()//���������� 
    {
        if (flyingBilding != null)
        {
            if (Input.GetKeyDown(KeyCode.R)) //������� ������
            {
                flyingBilding.RotateBilding();
            }

            var groundPlane = new Plane(Vector3.up, Vector3.zero); //���������
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);//���

            if (groundPlane.Raycast(ray, out float position))//�������� ����������� ���� � ���������
            {
                Vector3 worldosition = ray.GetPoint(position);//��������� �������

                //���������� ���������
                int cursorPositionX = Mathf.RoundToInt(worldosition.x);
                int cursorPositionY = Mathf.RoundToInt(worldosition.z);
                

                bool available = true; //����� �� ��������� ������

                /*
                _cursorPositionX = cursorPositionX;
                _cursorPositionY = cursorPositionY;
                _gridSizaX = GridSize.x;
                _gridSizeY = GridSize.y;
                _flyingBildingSizeX = flyingBilding.Size.x;
                _flyingBildingSizeY = flyingBilding.Size.y;
                */


                _statePosition = flyingBilding.statePosition;
                if (_statePosition == 0 || _statePosition == 1)
                {
                    if (cursorPositionX < 0 || cursorPositionX > GridSize.x - flyingBilding.Size.x) available = false;
                    if (cursorPositionY < 0 || cursorPositionY > GridSize.y - flyingBilding.Size.y) available = false;
                }

                if (_statePosition == 2 || _statePosition == 3)
                {
                    if (cursorPositionX - flyingBilding.Size.x < -1) available = false;
                    if (cursorPositionY - flyingBilding.Size.y < -1) available = false;
                    if (cursorPositionX >= GridSize.x) available = false;
                    if (cursorPositionY >= GridSize.y) available = false;

                }

               if (flyingBilding.GetComponent<PlayerUnit>()) 
                {
                    if (cursorPositionY >= _deployGridSize) available = false;
                }

                if (available && IsPlaceTaken(cursorPositionX, cursorPositionY)) available = false;


                flyingBilding.transform.position = new Vector3(cursorPositionX, 0, cursorPositionY);//������� �������� ��������� ������
                flyingBilding.SetTransperent(available);//������ ���� ��������� ������

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBilding(cursorPositionX, cursorPositionY);//���������� ������
                }
            }
        }
    }

    private bool IsPlaceTaken (int placeX, int placeY) //���� ����� ������
    {
        if (_statePosition == 0 || _statePosition == 1)
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    if (gird[placeX + x, placeY + y] != null) return true; //���� ������ ������
                }
            }
        }

        if (_statePosition == 2 || _statePosition == 3)
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    if (gird[placeX - x, placeY - y] != null) return true; //���� ������ ������
                }
            }
        }
        return false;
    }

    void PlaceFlyingBilding(int placeX, int placeY) //���������� ������
    {
        if (_statePosition == 0 || _statePosition == 1)//������ � ������ ���������
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    gird[placeX + x, placeY + y] = flyingBilding;
                }
            }
        }
        if (_statePosition == 2 || _statePosition == 3)//������ � ��������� ���������
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    gird[placeX - x, placeY - y] = flyingBilding;
                }
            }
        }

     


        flyingBilding.SetNormalColor();//������ ���������� ����
        flyingBilding = null;//��������� ������
    }


  
}

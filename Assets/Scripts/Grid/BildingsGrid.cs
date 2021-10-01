using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildingsGrid : MonoBehaviour
{
    //������ �����
    public Vector2Int _gridSize = new Vector2Int(10, 10);

    private Camera _mainCamera;

    [SerializeField][Header("������� ���������� ������")]
    private int _deployGridSize;

    [SerializeField][Header("������� ����� ����� ��� ������ ��������� ���������")]
    private Building _targetPoint;

    [SerializeField][Header("������ �� �����")]
    private GameObject _cellOnGrid;

    [SerializeField][Header("������� ��� ����� ������ �������� ����")]
    private GameObject _cellParent1;

    [SerializeField][Header("������� ��� ����� ������ �������� ����")]
    private GameObject _cellParent2;

    private int _statePosition;

    //�������� (���������) ������ 
    private Building _flyingBilding;

    //���� �������� ����� ������ ����
    private Building _unitflyingBilding;

    //��������� ������ ������
    private Building[,] _buildingGrid;

    //������ �� ���� ��� ����������� ���������� � �������� �����
    private GameObject[,] _cellsOnGrid;
       
    /*
    ����������� ����
    [SerializeField] private int _cursorPositionX;
    [SerializeField] private int _cursorPositionY;
    [SerializeField] private int _gridSizaX;
    [SerializeField] private int _gridSizeY;
    [SerializeField] private int _flyingBildingSizeX;
    [SerializeField] private int _flyingBildingSizeY;
    */


    private void Awake()
    {
        //������������� ������� ����������� ��������
        _buildingGrid = new Building[_gridSize.x, _gridSize.y];
        _cellsOnGrid = new GameObject[_gridSize.x, _gridSize.y];
        _mainCamera = Camera.main;

        //����������� �����
        Displaying�ells();
        CelParentSetActive(false);
    }

    private void Update()
    {
        //����������  ������
        PlaceBuilding();
        if (_unitflyingBilding != null)
        {
            SetDefendPoint();
        }
    }

    //��������� ����� ������� � ����� (��������������)
    private void OnDrawGizmos()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Gizmos.color = new Color(0.88f, 0.5f, 0.3f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    //����� ��������� (����������) ������
    public void StartPlacingBilding(Building BildingPrefab) 
    {
        if (_flyingBilding != null) 
        {
            Destroy(_flyingBilding.gameObject);
        }
        _flyingBilding = Instantiate(BildingPrefab);
    }

    //���������� ������
    private void PlaceBuilding()
    {
        if (_flyingBilding != null)
        {
            //������� ������
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                _flyingBilding.RotateBilding();
            }

            //���������
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            //�������� ����������� ���� � ���������
            if (groundPlane.Raycast(ray, out float position))
            {
                //��������� �������
                Vector3 worldosition = ray.GetPoint(position);

                //���������� ���������
                int cursorPositionX = Mathf.RoundToInt(worldosition.x);
                int cursorPositionY = Mathf.RoundToInt(worldosition.z);

                //����� �� ��������� ������
                bool available = true; 

                /*
                 ���������� ����������� �����
                _cursorPositionX = cursorPositionX;
                _cursorPositionY = cursorPositionY;
                _gridSizaX = GridSize.x;
                _gridSizeY = GridSize.y;
                _flyingBildingSizeX = flyingBilding.Size.x;
                _flyingBildingSizeY = flyingBilding.Size.y;
                */

                _statePosition = _flyingBilding._statePosition;

                if (_statePosition == 0 || _statePosition == 1)
                {
                    if (cursorPositionX < 0 || cursorPositionX > _gridSize.x - _flyingBilding._gridBuildingSize.x) available = false;
                    if (cursorPositionY < 0 || cursorPositionY > _gridSize.y - _flyingBilding._gridBuildingSize.y) available = false;
                }
                else if (_statePosition == 2 || _statePosition == 3)
                {
                    if (cursorPositionX - _flyingBilding._gridBuildingSize.x < -1) available = false;
                    if (cursorPositionY - _flyingBilding._gridBuildingSize.y < -1) available = false;
                    if (cursorPositionX >= _gridSize.x) available = false;
                    if (cursorPositionY >= _gridSize.y) available = false;
                }
 
                if (_flyingBilding.GetComponent<PlayerUnit>()) 
                {
                    if (cursorPositionY >= _deployGridSize) available = false;
                }

                if (available && IsPlaceTaken(cursorPositionX, cursorPositionY)) available = false;

                //������� �������� ��������� ������
                _flyingBilding.transform.position = new Vector3(cursorPositionX, 0, cursorPositionY);

                //����� ����� ����� ��������� ������
                _flyingBilding.SetTransperent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBilding(cursorPositionX, cursorPositionY);
                }
            }
        }
    }

    //���� ����� ������
    private bool IsPlaceTaken (int placeX, int placeY) 
    {
        if (_statePosition == 0 || _statePosition == 1)
        {
            for (int x = 0; x < _flyingBilding._gridBuildingSize.x; x++)
            {
                for (int y = 0; y < _flyingBilding._gridBuildingSize.y; y++)
                { 
                    //���� ������ ������
                    if (_buildingGrid[placeX + x, placeY + y] != null) return true; 
                }
            }
        }
        else if (_statePosition == 2 || _statePosition == 3)
        {
            for (int x = 0; x < _flyingBilding._gridBuildingSize.x; x++)
            {
                for (int y = 0; y < _flyingBilding._gridBuildingSize.y; y++)
                {
                    //���� ������ ������
                    if (_buildingGrid[placeX - x, placeY - y] != null) return true; 
                }
            }
        }
        return false;
    }

    //���������� ������
    void PlaceFlyingBilding(int placeX, int placeY) 
    {
        if (_statePosition == 0 || _statePosition == 1)
        {
            for (int x = 0; x < _flyingBilding._gridBuildingSize.x; x++)
            {
                for (int y = 0; y < _flyingBilding._gridBuildingSize.y; y++)
                {
                    celColorChaige(_cellsOnGrid[placeX +x, placeY + y],true);
                    _buildingGrid[placeX + x, placeY + y] = _flyingBilding;
                }
            }
        }
        else if (_statePosition == 2 || _statePosition == 3)
        {
            for (int x = 0; x < _flyingBilding._gridBuildingSize.x; x++)
            {
                for (int y = 0; y < _flyingBilding._gridBuildingSize.y; y++)
                {
                    celColorChaige(_cellsOnGrid[placeX - x, placeY - y], true);
                    _buildingGrid[placeX - x, placeY - y] = _flyingBilding;
                }
            }
        }

        //������ ���������� ���� ����� ��������� ������
        _flyingBilding.SetNormalColor();

        if (_flyingBilding.GetComponent<PlayerUnit>() && _unitflyingBilding == null)
        {
            //�������� ����� ��� ����������� ������ ���������
            _unitflyingBilding = _flyingBilding;
        }

        _flyingBilding = null;
        CelParentSetActive(false);       
    }

    //���������� �������������� �����
    void SetDefendPoint()
    {
        PlayerUnit playerUnit = _unitflyingBilding.GetComponent<PlayerUnit>();

        if (playerUnit._isUnitAtack)
        {
            _unitflyingBilding = null;
        }
        else if (playerUnit._isUnitDefend)
        {
            CelParentSetActive(true);

            //�������� ������� �����
            _flyingBilding = Instantiate(_targetPoint);
            playerUnit._defendPoint = _flyingBilding.transform;
            _unitflyingBilding = null;
        }
    }

    //����������� �����
    void Displaying�ells()
    {
        for (int i = 0; i < _gridSize.x; i++)
        {
            for (int j = 0; j < _gridSize.y; j++)
            {
                Vector3 cellPosition = transform.position + new Vector3(i, 0, j);

                if (j < _deployGridSize)
                {
                    _cellsOnGrid[i, j] = Instantiate(_cellOnGrid, _cellParent1.transform);
                }
                else
                {
                    _cellsOnGrid[i, j] = Instantiate(_cellOnGrid, _cellParent2.transform);
                }
                _cellsOnGrid[i, j].transform.position = cellPosition;
            }
        }
    }

    //����� ����� ������ 
    void celColorChaige(GameObject _cell, bool taken)
    {
        Renderer cellRenderer = _cell.GetComponent<Renderer>();
        if (taken)
        {
            cellRenderer.material.color = Color.red;
        }
        else
        {
            cellRenderer.material.color = Color.green;
        }
    }

    void CelParentSetActive(bool active)
    { 
       _cellParent1.SetActive(active);
       _cellParent2.SetActive(active);   
    }
}

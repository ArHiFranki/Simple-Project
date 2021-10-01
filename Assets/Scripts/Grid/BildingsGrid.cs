using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildingsGrid : MonoBehaviour
{
    //Размер сетки
    public Vector2Int _gridSize = new Vector2Int(10, 10);

    private Camera _mainCamera;

    [SerializeField][Header("Область размещения юнитов")]
    private int _deployGridSize;

    [SerializeField][Header("Целевая точка юнита при выборе защитного состояния")]
    private Building _targetPoint;

    [SerializeField][Header("Ячейка на сетке")]
    private GameObject _cellOnGrid;

    [SerializeField][Header("Коробка для ячеек первой половины поля")]
    private GameObject _cellParent1;

    [SerializeField][Header("Коробка для ячеек второй половины поля")]
    private GameObject _cellParent2;

    private int _statePosition;

    //Активное (выбранное) здание 
    private Building _flyingBilding;

    //Юнит которому нужно задать цель
    private Building _unitflyingBilding;

    //Двумерный массив зданий
    private Building[,] _buildingGrid;

    //ячейки на стке для отоброжения свободного и занятого места
    private GameObject[,] _cellsOnGrid;
       
    /*
    проверочные поля
    [SerializeField] private int _cursorPositionX;
    [SerializeField] private int _cursorPositionY;
    [SerializeField] private int _gridSizaX;
    [SerializeField] private int _gridSizeY;
    [SerializeField] private int _flyingBildingSizeX;
    [SerializeField] private int _flyingBildingSizeY;
    */


    private void Awake()
    {
        //Инициализация массива размещенных оюъектов
        _buildingGrid = new Building[_gridSize.x, _gridSize.y];
        _cellsOnGrid = new GameObject[_gridSize.x, _gridSize.y];
        _mainCamera = Camera.main;

        //отображение ячеек
        DisplayingСells();
        CelParentSetActive(false);
    }

    private void Update()
    {
        //Размещение  здания
        PlaceBuilding();
        if (_unitflyingBilding != null)
        {
            SetDefendPoint();
        }
    }

    //Отрисовка сетки объекта в сцене (Вспомогательно)
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

    //Спавн активного (выбранного) здания
    public void StartPlacingBilding(Building BildingPrefab) 
    {
        if (_flyingBilding != null) 
        {
            Destroy(_flyingBilding.gameObject);
        }
        _flyingBilding = Instantiate(BildingPrefab);
    }

    //Размещение здания
    private void PlaceBuilding()
    {
        if (_flyingBilding != null)
        {
            //Поворот здания
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                _flyingBilding.RotateBilding();
            }

            //плоскость
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            //проверка пересечения луча и плоскости
            if (groundPlane.Raycast(ray, out float position))
            {
                //положение курсора
                Vector3 worldosition = ray.GetPoint(position);

                //Округление координат
                int cursorPositionX = Mathf.RoundToInt(worldosition.x);
                int cursorPositionY = Mathf.RoundToInt(worldosition.z);

                //можно ли поставить здание
                bool available = true; 

                /*
                 заполнение проверочных полей
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

                //Задание кооринат активному зданию
                _flyingBilding.transform.position = new Vector3(cursorPositionX, 0, cursorPositionY);

                //Смена цвета сетки активного здания
                _flyingBilding.SetTransperent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBilding(cursorPositionX, cursorPositionY);
                }
            }
        }
    }

    //Если место занято
    private bool IsPlaceTaken (int placeX, int placeY) 
    {
        if (_statePosition == 0 || _statePosition == 1)
        {
            for (int x = 0; x < _flyingBilding._gridBuildingSize.x; x++)
            {
                for (int y = 0; y < _flyingBilding._gridBuildingSize.y; y++)
                { 
                    //если клетка занята
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
                    //если клетка занята
                    if (_buildingGrid[placeX - x, placeY - y] != null) return true; 
                }
            }
        }
        return false;
    }

    //поставаить здание
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

        //Задать нормальный цвет сетке активного здания
        _flyingBilding.SetNormalColor();

        if (_flyingBilding.GetComponent<PlayerUnit>() && _unitflyingBilding == null)
        {
            //Запомнит юнита для дальнейщего выбора поведения
            _unitflyingBilding = _flyingBilding;
        }

        _flyingBilding = null;
        CelParentSetActive(false);       
    }

    //установить оборонительную точку
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

            //Создание целевой точки
            _flyingBilding = Instantiate(_targetPoint);
            playerUnit._defendPoint = _flyingBilding.transform;
            _unitflyingBilding = null;
        }
    }

    //отображение ячеек
    void DisplayingСells()
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

    //смена цвета ячейки 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);//Размер сетки
    [SerializeField][Header("Область размещения юнитов")]
    private int _deployGridSize;

    [SerializeField][Header("Целевая точка юнита при выборе защитного состояния")]
    private Building targetPoint;

    [SerializeField][Header("Ячейка на сетке")]
    private GameObject cellOnGrid;

    [SerializeField][Header("Коробка для ячеек первой половины поля")]
    private GameObject cellParent1;

    [SerializeField][Header("Коробка для ячеек второй половины поля")]
    private GameObject cellParent2;


    /*
    [SerializeField] private int _cursorPositionX;
    [SerializeField] private int _cursorPositionY;
    [SerializeField] private int _gridSizaX;
    [SerializeField] private int _gridSizeY;
    [SerializeField] private int _flyingBildingSizeX;
    [SerializeField] private int _flyingBildingSizeY;
    */

    private int _statePosition;
    private Building[,] grid;//Двумерный массив зданий

    private Building flyingBilding;//Активное (выбранное) здание 

    private Building UnitflyingBilding;//Юнит которому нужно задать цель
    private Camera mainCamera;//Камера

    private GameObject[,] cells; //ячейки для отоброжения

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];//Инициализация массива размещенных оюъектов
        cells = new GameObject[GridSize.x, GridSize.y];

        mainCamera = Camera.main;

        DisplayingСells();//отображение ячеек
        cellParent1.SetActive(false);
        cellParent2.SetActive(false);

    }

    private void Update()
    {
        PlaceBuilding();//Размещение  здания
        if (UnitflyingBilding != null)
        {
            SetDefendPoint();
        }

      
    }


    private void OnDrawGizmos()//Отрисовка сетки объекта в сцене (Вспомогательно)
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

    public void StartPlacingBilding(Building BildingPrefab) //Спавн активного (выбранного) здания
    {
        if (flyingBilding != null) //Если уже есть активное здание, удалить его
        {
            Destroy(flyingBilding.gameObject);
        }

        flyingBilding = Instantiate(BildingPrefab);//Создание обьекта
    }

    private void PlaceBuilding()//Размещение 
    {
        if (flyingBilding != null)
        {
            if (Input.GetKeyDown(KeyCode.R)) //Поворот здания
            {
                flyingBilding.RotateBilding();
            }

            var groundPlane = new Plane(Vector3.up, Vector3.zero); //плоскость
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);//Луч

            if (groundPlane.Raycast(ray, out float position))//проверка пересечения луча и плоскости
            {
                Vector3 worldosition = ray.GetPoint(position);//положение курсора

                //Округление координат
                int cursorPositionX = Mathf.RoundToInt(worldosition.x);
                int cursorPositionY = Mathf.RoundToInt(worldosition.z);
                

                bool available = true; //можно ли поставить здание

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


                flyingBilding.transform.position = new Vector3(cursorPositionX, 0, cursorPositionY);//Задание кооринат активному зданию
                flyingBilding.SetTransperent(available);//Задать цвет активному зданию

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBilding(cursorPositionX, cursorPositionY);//поставаить здание
                }
            }
        }
    }

    private bool IsPlaceTaken (int placeX, int placeY) //Если место занято
    {
        if (_statePosition == 0 || _statePosition == 1)
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    if (grid[placeX + x, placeY + y] != null) return true; //если клетка занята
                }
            }
        }

        if (_statePosition == 2 || _statePosition == 3)
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    if (grid[placeX - x, placeY - y] != null) return true; //если клетка занята
                }
            }
        }
        return false;
    }

    void PlaceFlyingBilding(int placeX, int placeY) //поставаить здание
    {
        if (_statePosition == 0 || _statePosition == 1)//первое и второе положения
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    celColorChaige(cells[placeX +x, placeY + y],true);
                    grid[placeX + x, placeY + y] = flyingBilding;
                }
            }
        }
        if (_statePosition == 2 || _statePosition == 3)//третье и четвертое положение
        {
            for (int x = 0; x < flyingBilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBilding.Size.y; y++)
                {
                    celColorChaige(cells[placeX - x, placeY - y], true);
                    grid[placeX - x, placeY - y] = flyingBilding;
                }
            }
        }

        flyingBilding.SetNormalColor();//Задать нормальный цвет

        
        if (flyingBilding.GetComponent<PlayerUnit>() && UnitflyingBilding == null)
        {
            UnitflyingBilding = flyingBilding;//Сохраняем юнита
        }

        
        flyingBilding = null;//Поставить здание
        CelParentSetActive(false);
       
    }

    void SetDefendPoint()//установить оборонительную точку
    {
        PlayerUnit playerUnit= UnitflyingBilding.GetComponent<PlayerUnit>();

        if (playerUnit.isUnitAtack)
        {
            UnitflyingBilding = null;
        }
        if (playerUnit.isUnitDefend)
        {
            CelParentSetActive(true);
           
            flyingBilding = Instantiate(targetPoint);//Создание обьекта
            playerUnit.defendPoint = flyingBilding.transform;
            UnitflyingBilding = null;
        }
        
    }

    void DisplayingСells()//отображение ячеек
    {
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                Vector3 cellPosition = transform.position + new Vector3(i, 0, j);

                if (j <_deployGridSize)
                {
                    cells[i, j] = Instantiate(cellOnGrid, cellParent1.transform);
                }
                else cells[i, j] = Instantiate(cellOnGrid, cellParent2.transform);

                cells[i, j].transform.position = cellPosition;

            }
        }
    }
    
    void celColorChaige(GameObject _cell, bool taken)//смена цвета ячейки 
    {
        Renderer cellRenderer = _cell.GetComponent<Renderer>();
        if (taken)
        {
            cellRenderer.material.color = Color.red;
        }else cellRenderer.material.color = Color.green;

    }

    void CelParentSetActive(bool active)
    { 
       cellParent1.SetActive(active);
       cellParent2.SetActive(active);   
    }
}

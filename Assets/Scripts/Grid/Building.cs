using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт здания
public class Building : MonoBehaviour
{
    [Header("Размер здания на сетке")] public Vector2Int _gridBuildingSize = Vector2Int.one;
    [HideInInspector]public int _statePosition = 0;

    private Color _gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);

    //Задать цвет активному зданию
    public void SetTransperent(bool available)
    {
        //если объект можно поставить
        if (available)
        {
            _gizmosColor = Color.green;
        }
        else 
        {
            _gizmosColor = Color.red;
        }
    }

    //Вернуть нормальный цвет
    public void SetNormalColor()
    {
        _gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);
    }

    //Отрисовка сетки объекта в сцене (Вспомогательно)
    private void OnDrawGizmos()
    {
        for (int x = 0; x < _gridBuildingSize.x; x++)
        {
            for (int y = 0; y < _gridBuildingSize.y; y++)
            {
                Gizmos.color = _gizmosColor;
                if (_statePosition == 0 || _statePosition == 1)
                {
                    Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
                }
                else
                {
                    Gizmos.DrawCube(transform.position + new Vector3(-x, 0, -y), new Vector3(1, 0.1f, 1));
                }
            }
        }
    }

    //Вращение обьекта
    public void RotateBilding()
    {
        float rotationAngle = 90;
        //поворот объекта
        transform.Rotate(0,rotationAngle,0);

        if (_statePosition < 3)
        {
            _statePosition++;
        }
        else
        {
            _statePosition = 0;
        }

        //поворот сетки
        int _newX = _gridBuildingSize.y;
        int _newY = _gridBuildingSize.x;
        _gridBuildingSize = new Vector2Int(_newX, _newY);
    }
}

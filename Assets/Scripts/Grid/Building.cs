using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт здания
public class Building : MonoBehaviour
{
    [SerializeField] private Renderer MainRenderer;//Отображение объекта

    public Vector2Int Size = Vector2Int.one;//Размер здания на сетке

    public int statePosition = 0;

    public void SetTransperent(bool available)//Задать цвет активному зданию
    {
        if (available)//если объект можно поставить
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormalColor()//Вернуть нормальный цвет
    {
        MainRenderer.material.color = Color.white;
    }


    private void OnDrawGizmos()//Отрисовка сетки объекта в сцене (Вспомогательно)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0.88f, 0.5f, 0f, 0.3f);
                if (statePosition == 0 || statePosition == 1)
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

    public void RotateBilding()//Вращение обьекта
    {
        float degrees = 90;//Угол поворота
        transform.Rotate(0,degrees,0);//поворот меша

        if (statePosition < 3)
            statePosition++;
        else
            statePosition = 0;

        //поворот сетки
        int NevX = Size.y;
        int NevY = Size.x;
        Size = new Vector2Int(NevX, NevY);
    }
}

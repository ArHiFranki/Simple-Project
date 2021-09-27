using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������
public class Building : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;//������ ������ �� �����

    [HideInInspector]public int statePosition = 0;

    Color gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);


    public void SetTransperent(bool available)//������ ���� ��������� ������
    {
        if (available)//���� ������ ����� ���������
        {
            gizmosColor = Color.green;
        }
        else 
        {
            gizmosColor = Color.red;
        }
    }

    public void SetNormalColor()//������� ���������� ����
    {

        gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);

    }


    private void OnDrawGizmos()//��������� ����� ������� � ����� (��������������)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = gizmosColor;
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

    public void RotateBilding()//�������� �������
    {
        float degrees = 90;//���� ��������
        transform.Rotate(0,degrees,0);//������� ����

        if (statePosition < 3)
            statePosition++;
        else
            statePosition = 0;

        //������� �����
        int NevX = Size.y;
        int NevY = Size.x;
        Size = new Vector2Int(NevX, NevY);
    }
}

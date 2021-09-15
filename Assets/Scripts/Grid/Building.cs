using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������
public class Building : MonoBehaviour
{
    [SerializeField] private Renderer MainRenderer;//����������� �������

    public Vector2Int Size = Vector2Int.one;//������ ������ �� �����

    public int statePosition = 0;

    public void SetTransperent(bool available)//������ ���� ��������� ������
    {
        if (available)//���� ������ ����� ���������
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormalColor()//������� ���������� ����
    {
        MainRenderer.material.color = Color.white;
    }


    private void OnDrawGizmos()//��������� ����� ������� � ����� (��������������)
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

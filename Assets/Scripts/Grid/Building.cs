using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������
public class Building : MonoBehaviour
{
    [Header("������ ������ �� �����")] 
    public Vector2Int gridBuildingSize = Vector2Int.one;

    [HideInInspector] public int _statePosition = 0;

    private Color _gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);

    // ������ ���� ��������� ������
    public void SetTransperent(bool isAvailable)
    {
        // ���� ������ ����� ���������
        if (isAvailable)
        {
            _gizmosColor = Color.green;
        }
        else 
        {
            _gizmosColor = Color.red;
        }
    }

    // ������� ���������� ����
    public void SetNormalColor()
    {
        _gizmosColor = new Color(0.88f, 0.5f, 0f, 0.3f);
    }

    // ��������� ����� ������� � ����� (��������������)
    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridBuildingSize.x; x++)
        {
            for (int y = 0; y < gridBuildingSize.y; y++)
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

    // �������� �������
    public void RotateBilding()
    {
        float rotationAngle = 90f;

        // ������� �������
        transform.Rotate(0,rotationAngle,0);

        if (_statePosition < 3)
        {
            _statePosition++;
        }
        else
        {
            _statePosition = 0;
        }

        // ������� �����
        int _newX = gridBuildingSize.y;
        int _newY = gridBuildingSize.x;
        gridBuildingSize = new Vector2Int(_newX, _newY);
    }
}

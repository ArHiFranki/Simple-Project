using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, transform.position, _speed * Time.deltaTime);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }
}

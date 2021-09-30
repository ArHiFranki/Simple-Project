using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform Target;
    public float _speed;
    public int Damage;
    private void Update()
    {
        if (Target != null)
        {
            
            Vector3 target = new Vector3(Target.position.x, Target.position.y + 1, Target.position.z);

            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) //если объект сталкивается с чем-то
    {
        Debug.Log("Bulet hit " + other.gameObject.name); //вывести в консоль имя обьекта с которм было столкновение

        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage(Damage);
        }

        Destroy(gameObject);
    }

   
}

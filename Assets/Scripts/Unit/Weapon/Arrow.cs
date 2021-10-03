using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform targetOfArrow;
    public float arrowSpeed;
    public int damage;
    private void Update()
    {
        PursuitOfTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }

    private void PursuitOfTarget()
    {
        if (targetOfArrow != null)
        {
            Vector3 target = new Vector3(targetOfArrow.position.x, targetOfArrow.position.y + 1, targetOfArrow.position.z);

            transform.position = Vector3.MoveTowards(transform.position, target, arrowSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

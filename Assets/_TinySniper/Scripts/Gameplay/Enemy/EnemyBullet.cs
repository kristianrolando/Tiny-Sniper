using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody rb;

    Transform _target;

    public void Initial(Transform target)
    {
        _target = target;
        transform.LookAt(target);
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }
}

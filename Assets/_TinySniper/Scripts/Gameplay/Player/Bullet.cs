using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    Transform _target;
    float damage;
    Vector3 targetPos;
    Vector3 direction;
    Vector3 velocity;

    public void Initial(Vector3 pos, float damage, Transform target)
    {
        direction = (pos - transform.position).normalized;
        targetPos = pos;
        this.damage = damage;
        _target = target;
        transform.LookAt(pos);
        rb = GetComponent<Rigidbody>();

        rb.AddForce(direction.normalized * 300f, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        //rb.MovePosition(Vector3.SmoothDamp(rb.position, targetPos, ref velocity, 0.01f, 800f));
        //if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<IDamageAble>() != null)
        {
            collision.gameObject.GetComponent<IDamageAble>().GotDamage(damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    public bool b_IsActive = false;
    public bool b_IsMove = false;

    public IDamageble originMaster;

    protected Animator anim;
    protected Rigidbody2D rigidbody;

    protected Vector2 direction = Vector2.zero;
    protected Vector3 rotation = Vector2.zero;
    protected float moveSpeed = 0.0f;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (b_IsMove)
        {
            rigidbody.velocity = direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveSpeed = 0.0f;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;
        }

        transform.rotation = Quaternion.Euler(rotation);
    }

    public void Shot(Vector2 origin, Vector2 target, float speed)
    {
        direction = (target - origin).normalized;
        rotation = new Vector3(0.0f, 0.0f, (Mathf.Atan2((origin - target).y, (origin - target).x) * Mathf.Rad2Deg));
        moveSpeed = speed;

        b_IsActive = true;
        b_IsMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            b_IsMove = false;

            moveSpeed = 0.0f;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;

            if (originMaster.b_IsDamageble())
                originMaster.Hit(collision.transform.GetComponent<IDamageble>());

            anim.SetTrigger("t_Collision");
        }
    }

    protected void EndBulletEvent()
    {
        b_IsActive = false;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeControl : MonoBehaviour
{
    private Rigidbody2D rig_Slime;
    private Animator anim_Slime;
    public Transform check_Point;
    public LayerMask layer;

    public float speed;
    public float radius_Check;
    public int health_Slime;

    // Start is called before the first frame update
    void Start()
    {
        rig_Slime = GetComponent<Rigidbody2D>();
        anim_Slime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        OnCollision();
    }

    void Move()
    {
        rig_Slime.velocity = new Vector2(speed, rig_Slime.velocity.y);
    }

    void OnCollision()
    {

        Collider2D check = Physics2D.OverlapCircle(check_Point.position, radius_Check, layer);

        if (check != null)
        {
            speed = -speed;
            if (transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public void OnHit()
    {
        anim_Slime.SetTrigger("Hit");
        health_Slime--;

        if (health_Slime <= 0)
        {
            speed = 0;
            anim_Slime.SetTrigger("Dead");
            Destroy(gameObject, 0.5f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(check_Point.position, radius_Check);
    }

}

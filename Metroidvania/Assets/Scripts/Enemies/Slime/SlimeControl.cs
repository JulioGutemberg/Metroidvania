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
        
        if (transform.rotation.y == 0) //Quando o valor da rotação é 0 , speed tem um valor negativo.
        {                              //When the value of rotation is 0, speed has a negative value.
            speed += 2;
        } else {
            speed -= 2; 
        }
        StartCoroutine(IsDamaging());

        if (health_Slime <= 0)
        {
            speed = 0;
            anim_Slime.SetTrigger("Death");
            Destroy(gameObject, 0.5f);
        }
    }

    IEnumerator IsDamaging()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (transform.rotation.y == 0)
        {
            speed -= 2;
        }
        else
        {
            speed += 2;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(check_Point.position, radius_Check);
    }

}

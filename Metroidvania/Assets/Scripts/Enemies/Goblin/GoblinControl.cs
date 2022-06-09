using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinControl : MonoBehaviour
{
    private Rigidbody2D rig_Goblin;
    private Vector2 direction;
    private Animator anim_Goblin;
    public Transform pointRaycast_Front;
    public Transform pointRaycast_Back;
    public Transform point_Attack;
    public LayerMask playerLayer;


    public bool isFront;
    public bool isRight;
    public int health;
    public float stopDistance;
    public float speed;
    public float maxVision;
    public float radius;


    // Start is called before the first frame update
    void Start()
    {
        rig_Goblin = GetComponent<Rigidbody2D>();
        anim_Goblin = GetComponent<Animator>();

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    private void Update()
    {
        TrackingPlayer();
    }
    void FixedUpdate()
    {
        OnMove();
    }

    void TrackingPlayer()
    {
        RaycastHit2D hit_Front = Physics2D.Raycast(pointRaycast_Front.position, direction, maxVision);

        if (hit_Front.collider != null)
        {
            if (hit_Front.transform.CompareTag("Player"))
            {
                isFront = true;
                float distance = Vector2.Distance(transform.position, hit_Front.transform.position);

                if (distance <= stopDistance)
                {
                    isFront = false;
                    rig_Goblin.velocity = Vector2.zero;
                    anim_Goblin.SetInteger("Transition", 2);
                }
            }

            else if (hit_Front.transform.CompareTag("Wall"))
            {
                isFront = false;
                anim_Goblin.SetInteger("Transition", 0);
            }
        }

        RaycastHit2D hit_Back = Physics2D.Raycast(pointRaycast_Back.position, -direction, maxVision);

        if (hit_Back.collider != null)
        {
            if (hit_Back.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                isFront = true;
            }

            else if (hit_Back.transform.CompareTag("Ground") || hit_Front.transform.CompareTag("Wall"))
            {
                isFront = false;
                anim_Goblin.SetInteger("Transition", 0);
            }
        }
    }

    void OnMove()
    {
        if (isFront)
        {
            anim_Goblin.SetInteger("Transition", 1);
            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig_Goblin.velocity = new Vector2(speed, rig_Goblin.velocity.y);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig_Goblin.velocity = new Vector2(-speed, rig_Goblin.velocity.y);
            }
        }
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(point_Attack.position, radius, playerLayer);

        if (hit != null)
        {
            hit.GetComponent<PlayerControl>().OnDamage();

        }
    }

    public void OnHit()
    {
        anim_Goblin.SetTrigger("Hit");
        health--;

        if (health <= 0)
        {
            speed = 0;
            anim_Goblin.SetTrigger("Death");
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(pointRaycast_Front.position, direction * maxVision); //Desenha uma linha na frente
        Gizmos.DrawRay(pointRaycast_Back.position, -direction * maxVision); //Desenha uma linha nas costas
        Gizmos.DrawWireSphere(point_Attack.position, radius);
    }
}


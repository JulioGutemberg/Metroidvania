using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushmonsterControl : MonoBehaviour
{
    private Transform mushmonster;
    private SpriteRenderer mushSprite;
    private Animator anim_Mush;

    public Transform[] position;
    public Transform jump_Point;
    public LayerMask layerPlayer;

    public int idTarget;
    public int health_Mush;
    public int jumpForce;
    public bool isRight;
    public float speed;
    public float radiusCheck;

    // Start is called before the first frame update
    void Start()
    {
        mushSprite = GetComponent<SpriteRenderer>();
        mushmonster = GetComponent<Transform>();
        anim_Mush = GetComponent<Animator>();
        idTarget = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        OnCollision();
    }

    void Move()
    {
        if (mushmonster != null)
        {
            mushmonster.position = Vector3.MoveTowards(mushmonster.position, position[idTarget].position, speed * Time.deltaTime);

            if (mushmonster.position == position[idTarget].position)
            { 
                idTarget += 1;
                if (idTarget == position.Length)
                {
                    idTarget = 0;
                }

            }

            if (position[idTarget].position.x < mushmonster.position.x && isRight == true)
            {
                Flip();
            }
            else if (position[idTarget].position.x > mushmonster.position.x && isRight == false)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isRight = !isRight;
        mushSprite.flipX = !mushSprite.flipX;
    }

    public void OnHit()
    {
        anim_Mush.SetTrigger("Hit");
        health_Mush--;
        speed = 1;
        StartCoroutine(IsDamaging());

        if (health_Mush <= 0)
        {
            speed = 0;
            anim_Mush.SetTrigger("Death");
            Destroy(gameObject, 0.5f);
        }
    }


    void OnCollision()
    {
        Collider2D check = Physics2D.OverlapCircle(jump_Point.position, radiusCheck, layerPlayer);

        if (check != null)
        {
            Debug.Log("Player na minha cabeça");
            anim_Mush.SetInteger("Transition", 1);
            speed = 0;
            PlayerControl.instance.rigPlayer.AddForce(new Vector2(0, jumpForce));
            StartCoroutine(OnCrushed());

        }
        
    }

    IEnumerator OnCrushed()
    {
        Debug.Log("Levantando");
        yield return new WaitForSeconds(1f);
        speed = 3;
        anim_Mush.SetInteger("Transition", 0);
    }

    IEnumerator IsDamaging()
    {
        yield return new WaitForSeconds(0.5f);
        speed = 3;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jump_Point.position, radiusCheck);
    }

}

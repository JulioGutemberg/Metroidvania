using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animPlayer;
    private ResourceSystem manaHP;

    public static PlayerControl instance;
    public Transform hit_Point;
    public Rigidbody2D rigPlayer;
    public LayerMask enemyLayer;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool isDamaging;
    private float timeCount;

    public float speedPlayer;
    public float jumpForce;
    public float hit_Radius;
    public float recoveryTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        animPlayer = GetComponent<Animator>();
        rigPlayer = GetComponent<Rigidbody2D>();
        manaHP = GetComponent<ResourceSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
        Recovery();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rigPlayer.velocity = new Vector2(movement * speedPlayer, rigPlayer.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                animPlayer.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                animPlayer.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isAttacking)
        {
            animPlayer.SetInteger("Transition", 0);
        }
    }

    void Jump() {

        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                animPlayer.SetInteger("Transition", 2);
                rigPlayer.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                //soundFX.PlaySFX(soundFX.jumpSFX);
            }
            else if (doubleJump)
            {
                animPlayer.SetBool("isDown", false);
                animPlayer.SetInteger("Transition", 3);
                rigPlayer.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                //soundFX.PlaySFX(soundFX.jumpSFX);
            }
            
        }

    }

    public void Down()
    {
       animPlayer.SetBool("isDown", true);  
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            animPlayer.SetInteger("Transition", 5);
            Collider2D hit = Physics2D.OverlapCircle(hit_Point.position, hit_Radius, enemyLayer);

            if (hit != null){

                if(hit.GetComponent<SlimeControl>())
                {
                    hit.GetComponent<SlimeControl>().OnHit();
                }

                if (hit.GetComponent<MushmonsterControl>())
                {
                    hit.GetComponent<MushmonsterControl>().OnHit();
                }
            
            }

            StartCoroutine(OnAttack());
        }

    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
    }

    public void OnDamage()
    {
        if (!isDamaging)
        {
            animPlayer.SetTrigger("Hit");
            manaHP.health--;
            isDamaging = true;
        }

        if (manaHP.health <= 0)
        {
            Debug.Log("Morri");
        }
    }
    void Recovery()
    {
        if (isDamaging)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= recoveryTime)
            {
                isDamaging = false;
                timeCount = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {      
        switch(collision.gameObject.layer){

            case 6:
                Debug.Log("No chao");
                isJumping = false;
                animPlayer.SetBool("isDown", false);
                break;

            case 8:
                OnDamage();
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8){
            OnDamage();
        }  
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hit_Point.position, hit_Radius);
    }
}

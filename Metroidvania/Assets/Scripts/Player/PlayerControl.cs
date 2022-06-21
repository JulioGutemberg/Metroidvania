using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animPlayer;
    private ResourceSystem hpSystem;
    private SystemSound playerAudio;

    public static PlayerControl instance;
    public Transform hit_Point;
    public Rigidbody2D rigPlayer;
    public LayerMask enemyLayer;
    public LayerMask treasureLayer;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool isDamaging;
    public bool isGrounded;
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
        hpSystem = GetComponent<ResourceSystem>();
        playerAudio = GetComponent<SystemSound>();
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

    #region Actions
    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rigPlayer.velocity = new Vector2(movement * speedPlayer, rigPlayer.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                isGrounded = true;
                animPlayer.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                isGrounded = true;
                animPlayer.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isAttacking)
        {
            isGrounded = true;
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
                isGrounded = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if (doubleJump)
            {
                animPlayer.SetBool("isDown", false);
                animPlayer.SetInteger("Transition", 3);
                rigPlayer.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                isGrounded = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
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
            Collider2D hitTreasure = Physics2D.OverlapCircle(hit_Point.position, hit_Radius, treasureLayer);
            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null){

                if(hit.GetComponent<SlimeControl>())
                {
                    hit.GetComponent<SlimeControl>().OnHit();
                }

                if (hit.GetComponent<MushmonsterControl>())
                {
                    hit.GetComponent<MushmonsterControl>().OnHit();
                }

                if (hit.GetComponent<GoblinControl>())
                {
                    hit.GetComponent<GoblinControl>().OnHit();
                }

            }

            if (hitTreasure != null)
            {
                if (hitTreasure.GetComponent<SystemTreasure>())
                {
                    hitTreasure.GetComponent<SystemTreasure>().getTreasure();
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
            hpSystem.health--;
            isDamaging = true;
            playerAudio.PlaySFX(playerAudio.hurtSound);
        }

        if (hpSystem.health <= 0)
        {
            GameController.instance.ShowGameOver();
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
    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {      
        switch(collision.gameObject.layer){

            case 6:
                isJumping = false;
                animPlayer.SetBool("isDown", false);
                break;

            case 8:
                OnDamage();
                break;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.parent = collision.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8){
            OnDamage();
        }

        if(collision.CompareTag("Coin"))
        {
            collision.GetComponent<Animator>().SetTrigger("Hit");
            GameController.instance.GetCoin();
            playerAudio.PlaySFX(playerAudio.coindSound);
            Destroy(collision.gameObject, 0.3f);
        }

        if (collision.CompareTag("Potion")){
            if (hpSystem.health < hpSystem.healthMAX)
            {
                hpSystem.health++;
            } else
            {
                GameController.instance.GetCoin();
            }

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("NextLVL"))
        {
            GameController.instance.NextLvl();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") && isGrounded==true)
        {
            animPlayer.SetInteger("Transition", 4);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            this.transform.parent = null;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hit_Point.position, hit_Radius);
    }
}

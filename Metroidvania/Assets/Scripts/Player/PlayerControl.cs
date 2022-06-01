using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rigPlayer;
    private Animator animPlayer;
    private SpriteRenderer rendererPlayer;
    public static PlayerControl instance;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;

    public float speedPlayer;
    public float jumpForce;

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
        rigPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        rendererPlayer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
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
            rendererPlayer.flipX = false;
        }

        if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                animPlayer.SetInteger("Transition", 1);
            }
            rendererPlayer.flipX = true;
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

    void Down()
    {
        //if (isDown == true) {
        
            animPlayer.SetBool("isDown", true);
        
        //}
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("No chao");
            isJumping = false;
            animPlayer.SetBool("isDown", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTrap : MonoBehaviour
{
    private Rigidbody2D rig_Spike;

    // Start is called before the first frame update
    void Start()
    {
        rig_Spike = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            PlayerControl.instance.OnDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            PlayerControl.instance.OnDamage();
        }
    }
}

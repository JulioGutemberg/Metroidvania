using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemLever : MonoBehaviour
{
    private Animator anim_Lever;
    public Animator anim_Alvo;
    public TMP_Text txt;
    public LayerMask layerPlayer;
    public float radiusCheck;

    // Start is called before the first frame update
    void Start()
    {
        anim_Lever = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        OnCollision();
    }

    void OnPulling()
    {
        anim_Lever.SetBool("isActive", true);
        anim_Alvo.SetBool("isActive", true);
    }

    void OnCollision()
    {

        Collider2D check = Physics2D.OverlapCircle(transform.position, radiusCheck, layerPlayer);

        if(check != null)
        {
            txt.gameObject.SetActive(true);

            if (Input.GetButtonDown("Submit"))
            {
                OnPulling();
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusCheck);
    }
}

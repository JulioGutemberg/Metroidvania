using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemButton : MonoBehaviour
{
    private Animator anim_Button;
    public Animator anim_Alvo;

    // Start is called before the first frame update
    void Start()
    {
        anim_Button = GetComponent<Animator>();    
    }

    #region Actions
    void OnPressed()
    {
        anim_Button.SetBool("isActive", true);
        anim_Alvo.SetBool("isActive", true);
    }

    void OnExit()
    {
        anim_Button.SetBool("isActive", false);
        anim_Alvo.SetBool("isActive", false);
    }
    #endregion

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") || collision.gameObject.CompareTag("Player"))
        {
            OnPressed();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") || collision.gameObject.CompareTag("Player"))
        {
            OnExit();
        }
    }
}

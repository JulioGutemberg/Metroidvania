using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSystem : MonoBehaviour
{
    public int health;
    public int healthMAX;
    public Image[] hearts;
    public Sprite heart;
    public Sprite noHeart;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = heart;
            } 
            else
            {
                hearts[i].sprite = noHeart;
            }

            if(i < healthMAX)
            {
                hearts[i].enabled = true;
            } 
            else
            {
                hearts[i].enabled = false;
            }
        }
        
    }
}

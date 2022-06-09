using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTreasure : MonoBehaviour
{
    private Animator anim;
    public GameObject collectablePrefab;
    public Transform pointSpawn;
    public int countItens;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void getTreasure()
    {
        anim.SetTrigger("isOpen");
    }

    void SpawnItem()
    {
        for (int i = 0; i < countItens; i++)
        {
            Instantiate(collectablePrefab, pointSpawn.position, pointSpawn.rotation);
        }
    }

}

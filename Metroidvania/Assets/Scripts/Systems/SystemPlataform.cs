using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPlataform : MonoBehaviour
{
    private Vector3 pontoDestino;

    public Transform platform;
    public Transform pontoA;
    public Transform pontoB;

    public float speedPlatform;
    public GameObject player;
    

    // Start is called before the first frame update

    void Start()
    {
        platform.position = pontoA.position;
        pontoDestino = pontoB.position;
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, pontoDestino, speedPlatform * Time.deltaTime);

        if (platform.position == pontoDestino)
        {
            if (pontoDestino == pontoA.position)
            {
                pontoDestino = pontoB.position;
            }
            else if (pontoDestino == pontoB.position)
            {
                pontoDestino = pontoA.position;
            }
        }
    }
}

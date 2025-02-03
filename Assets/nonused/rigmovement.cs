using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigmovement : MonoBehaviour
{
    public GameObject playerrig;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerrig.transform.position = player.transform.position;
    }
}

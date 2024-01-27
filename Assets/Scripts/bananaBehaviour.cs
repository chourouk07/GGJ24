using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //make banana always rotate randomly
        transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }
    //make this gameobject destroy itself if it hits something with enemy tag  
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ob_c2 : MonoBehaviour
{
    public float movement_z = -35;
    public GameObject ob_3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            transform.position += new Vector3(movement_z, 0, 0) * Time.deltaTime;

        }
        


    }
    public void bounce()
    {
        movement_z *= -1f;
        Instantiate(ob_3,transform.position,Quaternion.identity);
    } 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bounce"))
        {
            bounce();
        }
    }
}

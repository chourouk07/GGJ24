using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float launchForce = 80f;
    public float Cd = 1f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LaunchProjectile();
        }
        
    }

    void LaunchProjectile()
    {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
       // projectile.transform.Translate(Vector3.forward * Time.deltaTime * launchForce);

        Destroy(projectile, 2f);
    }
    
}

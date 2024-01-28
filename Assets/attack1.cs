using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack1 : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;
    public float spawnInterval = 2f;
    public float boss_Hp = 100f;
    public float distance=100f;
    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("attack", 0f, spawnInterval);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void attack() {
        if (distance > 10)
        {
            if (boss_Hp > 30)
            {
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


                Instantiate(obstaclePrefab1, randomSpawnPoint.position, Quaternion.identity);

            }
            else
            {
                low_hp_attack();

            }
        }
        else
        {
            close_attack();
        }

        
        
    }
    private void close_attack()
    {



    }
    private void low_hp_attack()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


        Instantiate(obstaclePrefab2, randomSpawnPoint.position, Quaternion.identity);


    }
}

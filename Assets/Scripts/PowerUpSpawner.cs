using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  script gebruikt bij  {Spawners/EnemySpawner} script  bij EnemySpawner
public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject deleveryPrefab;

    [Header("SpawnTime")]
    public float spawnTime = 30f;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;

    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;

    [Header("Z Spawn Range")]
    public float zMin;
    public float zMax;

    [Header("Speed")]
    public int speed;

    private bool stopSpawn;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        //print(counter.spawn);
        if (counter.spawn) //will check if true
        {
            Vector3 SpawnPosition = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
            GameObject Enemy = Instantiate(deleveryPrefab, SpawnPosition, Quaternion.identity);

            EnemyMovement em = Enemy.GetComponent<EnemyMovement>();
            em.target = GameObject.FindWithTag("AI Lock").transform;
            em.speed = speed;

            GoTo gt = Enemy.GetComponent<GoTo>();
            gt.target = GameObject.FindWithTag("AI Lock").transform;
        }
    }


    void Update()
    { }

}

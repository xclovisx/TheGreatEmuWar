using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  script gebruikt bij  {Spawners/Meteor spawner} script  bij Meteor spawner
public class meteorSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject meteorPrefab;

    [Header("SpawnTime")]
    public float spawnTime = 1f;

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

    [Header("Sound")]
    public AudioClip ExplosionSound;

    private bool stopSpawn;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {

        if (counter.spawn) //will check if true
        {
            Vector3 SpawnPosition = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
            GameObject meteor = Instantiate(meteorPrefab, SpawnPosition, Quaternion.identity);

            meteorMovement mv = meteor.GetComponent<meteorMovement>();
            mv.target = GameObject.FindWithTag("Player").transform;
            mv.speed = speed;

            meteorDeleter md = meteor.GetComponent<meteorDeleter>();
            md.explosionSound = ExplosionSound;
        }
    }


    void Update()
    { }

}

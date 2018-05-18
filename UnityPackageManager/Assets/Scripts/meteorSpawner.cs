using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool stopSpawn;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        LifeCounter mc = GetComponent<LifeCounter>();

        if (GameObject.Find("screenSpawn").GetComponent<LifeCounter> ().spawn) //will check if true
        {
            Vector3 SpawnPosition = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
            GameObject meteor = Instantiate(meteorPrefab, SpawnPosition, Quaternion.identity);

            meteorMovement mv = meteor.GetComponent<meteorMovement>();
            mv.target = GameObject.FindWithTag("Player").transform;
            mv.speed = speed;
        }
    }


    void Update()
    { }

}

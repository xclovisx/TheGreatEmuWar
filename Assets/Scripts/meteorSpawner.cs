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

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        /*Vector3 SpawnPosition = new Vector3(0,10,0);
        Instantiate(meteorPrefab, SpawnPosition, Quaternion.identity);*/
    }


    void Spawn()
    {
        Vector3 SpawnPosition = new Vector3/*(0, 10, 0);*/(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        Instantiate(meteorPrefab, SpawnPosition, Quaternion.identity);
    }
    void Update()
    {/*
        function OnCollisionEnter(theCollision : Collision){
            if (theCollision.gameObject.tag == "balk")
            {
                Destroy(gameObject);
            }
        }*/
    }
}

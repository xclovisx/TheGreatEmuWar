using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour {
    [SerializeField]
    GameObject[] asteroids;
    [SerializeField]
    int asteroidQnt = 10;
    [SerializeField]
    int spacing = 150;

	// Use this for initialization
	void Start () {
        GenerateField();
    }

    void GenerateField() {
        for (int x = 0; x < asteroidQnt; x++)
        {
            for (int y = 0; y < asteroidQnt; y++)
            {
                for (int z = 0; z < asteroidQnt; z++)
                {
                    SpawnAsteroid(x, y, z);
                }
            }
        }
    }
    void SpawnAsteroid(int x, int y, int z) {
        Instantiate(asteroids[Random.Range(0, asteroids.Length)], 
            new Vector3(transform.position.x + (x * spacing) + AOffset(),
                        transform.position.y + (y * spacing) + AOffset(),
                        transform.position.z + (z * spacing) + AOffset()), Quaternion.identity, transform);
    }

    float AOffset() {
       return Random.Range(-spacing * 2, spacing * 2);
    }
}

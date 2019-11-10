using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipScript : MonoBehaviour
{
    public GameObject lightHouse;
    public GameObject shipPrefab;
    Transform shipPrefabTransform;

    public GameObject cannonball;


    float spawnTimer = 0;
    int spawnLimit = 5;
    int spawnCount = 0;
    float spawnThreshold = 1.5f;
    Vector3 offset;

    public List<GameObject> enemyShips = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        shipPrefabTransform = shipPrefab.transform;

        
    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnThreshold && spawnCount < spawnLimit)
        {

            float randomZ = Random.Range(-100, 100);
            float randomX = Random.Range(0f, 1f);

            if (randomX < 0.5f) randomX = 50;
            else randomX = -50;

            offset = new Vector3(randomX, 0, randomZ);
            Vector3 shipPosition = offset;


            var enemyShip = (GameObject)Instantiate(shipPrefab, shipPosition, shipPrefabTransform.rotation);
            enemyShips.Add(enemyShip);

            spawnCount++;
            spawnTimer = 0;

        }
    }

    public List<GameObject> getEnemyShips()
    {
        return enemyShips;
    }
}

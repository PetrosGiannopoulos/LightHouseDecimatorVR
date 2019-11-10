using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShipScript : MonoBehaviour
{

    public GameObject lightHouse;
    public GameObject shipPrefab;
    Transform shipPrefabTransform;
    public GameObject explosionparticle;


    float spawnTimer = 0;
    int spawnLimit = 5;
    int spawnCount = 0;
    float  spawnThreshold = 1.5f;
    Vector3 offset;

    public List<GameObject> friendlyShips = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        shipPrefabTransform = shipPrefab.transform;
        
    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer += Time.deltaTime;
       
        if (spawnTimer>=spawnThreshold && spawnCount <spawnLimit) {

            float randomX = Random.Range(-100,100);
            float randomZ = Random.Range(0f, 1f);

            if (randomZ < 0.5f) randomZ = 50;
            else randomZ = -50;

            offset = new Vector3(randomX,0,randomZ);
            Vector3 shipPosition = offset;
            

            var friendlyShip = (GameObject)Instantiate(shipPrefab,shipPosition,shipPrefabTransform.rotation);
            friendlyShips.Add(friendlyShip);

            spawnCount++;
            spawnTimer = 0;

        }
    }

    public List<GameObject> getFriendlyShips()
    {
        return friendlyShips;
    }

    public void removeFromList(int i)
    {
        spawnCount--;
        if(friendlyShips[i]!=null) Destroy(friendlyShips[i]);
        friendlyShips.RemoveAt(i);
        
    }

    public void reduce()
    {
        spawnCount--;
    }

    public void removeFromList(GameObject ship)
    {
        spawnCount--;

        if (ship != null)
        {
            Destroy(ship);
            friendlyShips.Remove(ship);
        }
    }

    public bool isNotEmpty()
    {
        return friendlyShips.Count > 0;
    }
}

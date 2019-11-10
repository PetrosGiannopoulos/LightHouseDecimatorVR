using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Well, I hit something!");
        if (collision.gameObject.tag == "FriendlyShip")
        {
            GameObject shipObject = collision.gameObject;
            FriendlyStatsScript ship = collision.gameObject.GetComponent<FriendlyStatsScript>();
            int attackHP = ship.Damage(25f);
            Destroy(this.gameObject);
            if (attackHP <= 0 )
            {
                //collision.transform.GetComponent<FriendlyShipScript>().reduce();
                //if(shipObject != null)collision.transform.GetComponent<FriendlyShipScript>().removeFromList(shipObject);
                //ship.GetComponent<FriendlyShipScript>().removeFromList(shipObject);
            }
            
        }
    }
}

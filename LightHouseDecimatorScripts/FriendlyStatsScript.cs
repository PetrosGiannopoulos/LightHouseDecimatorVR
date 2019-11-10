using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FriendlyStatsScript : MonoBehaviour
{
    public static int ShipKills;

    public StudioEventEmitter hit;
    public StudioEventEmitter death;

    int maximumHP;
    float currentHP;
    bool isSinked = false;
    public FriendlyShipScript fss;

    // Start is called before the first frame update
    void Start()
    {
        fss = GameObject.Find("FriendlyShipGenerator").GetComponent<FriendlyShipScript>();

        maximumHP = 100;
        currentHP = 100;
    }

    public int Damage(float damage)
    {
       
        currentHP -= damage;
        Instantiate(fss.explosionparticle,fss.explosionparticle.transform.position, fss.explosionparticle.transform.rotation);
        if (currentHP <= 0)
        {
            //Destroy(this.gameObject);
            isSinked = true;
            death.Play();
            StartCoroutine(sinkShip());
            ShipKills++;



        }
        else
            hit.Play();
        return (int)currentHP;
    }

    public float healthBarAmount()
    {
        return currentHP / maximumHP;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator sinkShip()
    {
        bool sinking = true;
        while (sinking)
        {
            transform.Translate(0,-0.2f,0,Space.World);
            if (transform.position.y < -3f)
            {
                
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool isSink()
    {
        return isSinked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

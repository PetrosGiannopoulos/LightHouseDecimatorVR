using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsScript : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter death;

    int maximumHP;
    float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        maximumHP = 100;
        currentHP = 100;
    }

    public int Damage(float damage)
    {

        currentHP -= damage;
        
        if (currentHP <= 0)
        {
            death.Play();
            Destroy(this.gameObject);
            



        }
        
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

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Beam")
        {
            //Debug.Log("Check");
            Damage(0.1f);
        }
        else if (other.gameObject.tag == "DeathBeam")
        {
            //Debug.Log("Mega check");
            Damage(0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

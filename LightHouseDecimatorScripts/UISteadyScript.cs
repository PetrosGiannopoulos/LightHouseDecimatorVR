using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISteadyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(0.04f,13.47f,0.1f));
       
    }
}

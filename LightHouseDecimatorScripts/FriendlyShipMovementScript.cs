using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FriendlyShipMovementScript : MonoBehaviour
{
    GameObject lighthouse;
    float speed = 4.0f;
    Transform originalTransform;
    Vector3 originalPosition;
    Vector3 offset;
    public static int ResourceCounter;
    Vector3 waypoint;
    float destinationDistance = 5f;

    float changingWaypointTimer = 0;
    float changnigWaypointThreshold = 1f;
    List<Vector3> waypoints = new List<Vector3>();
    int waypointCounter = 0;

    public StudioEventEmitter fogHorn;

    bool atPier = false;
    float resourceDropTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        lighthouse = GameObject.Find("Headless_light_house");
        originalTransform = transform;

        offset = new Vector3(2,0,2);

        originalPosition = transform.position;

        waypoints.Add(new Vector3(4.26f, -3.51f,-17.6f));
        waypoints.Add(originalPosition);
        waypoints.Add(new Vector3(4.26f, -3.07f, 25.46f));
        selectWaypoint();

       

       
        

    }

    // Update is called once per frame
    void Update()
    {
        if (!atPier)
        {
            changingWaypointTimer += Time.deltaTime;

            if (Vector3.Distance(transform.position, waypoint) <= destinationDistance)
            {
                if(waypointCounter==0 || waypointCounter == 2)atPier = true;
                StartCoroutine(DropResources());
            }

            //waypoint = lighthouse.transform.position;
            //transform.rotation = originalTransform.rotation;
            Vector3 direction = (waypoint - transform.position);
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90, 0));

            Vector3 control1 = 0.3f * transform.position + 0.7f * waypoint;
            Vector3 control2 = new Vector3(transform.position.z + waypoint.z, 0, (waypoint.x - transform.position.x) * 0.5f);

            if (waypointCounter == 0) direction = waypoint - catRomSpline(0.1f, transform.position, control1, control2, waypoint);

            transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);

            transform.position.Set(transform.position.x, 0, transform.position.z);
        }
    }

    IEnumerator DropResources()
    {
        if (waypointCounter == 0 || waypointCounter == 2)
        {
            fogHorn.Play();
            yield return new WaitForSeconds(resourceDropTime);
            atPier = false;
            ResourceCounter++;
        }
        selectWaypoint();
        


    }

    public void selectWaypoint()
    {
        waypointCounter++;
        if (waypointCounter > 2) waypointCounter = 0;
        waypoint = waypoints[waypointCounter];
        
    }

    Vector3 spline(float t1, Vector3 P0, Vector3 P1, Vector3 P2)
    {

        Vector3 a0 = P0;
        Vector3 a2 = ((P1 - P0) - t1 * (P2 - P0)) / (t1*(t1-1));
        Vector3 a1 = P2 - P0-a2;

        Vector3 P = a2*(t1*t1)+a1*t1+a0;

        return P;
    }

    Vector3 catRomSpline(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {

        Vector3 A = (-p1 + 3 * p2 - 3 * p3 + p4);
        Vector3 B = (2 * p1 - 5 * p2 + 4 * p3 - p4);
        Vector3 C = (-p1 + p3);
        Vector3 D = (2*p2);

        Vector3 P = (A * (t * t * t) + B * (t * t) + C * t + D)/2;

        return P;
    }
}

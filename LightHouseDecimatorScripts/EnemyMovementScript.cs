using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnemyMovementScript : MonoBehaviour
{
    GameObject lighthouse;
    float speed = 4.0f;
    Vector3 offset;

    float a = 43.01f;
    float b = 22.3f;
    float theta;

    bool isEllipse = false;

    float rotateThreshold = 3.0f;
    float rotateTimer = 0;
    float rotationSpeed = 2f;

    float angle1;
    float angle2;

    Vector3 control1;
    Vector3 control2;

    Vector3 originalDirection;

    public bool isHunting = false;
    float huntingDistance = 20f;

    GameObject friendlyGenerator;

    int huntingTargetId = -1;

    public GameObject cannonball;
    Transform cannonballTransform;
    public EnemyShipScript ess;
    public FriendlyShipScript fss;

    float attackCD = 2.5f;
    public enum States { Idle, Attacking };
    public States state = States.Idle;

    GameObject target;

    float attackSpeed = 1.7f;
    Vector3 direction;

    public StudioEventEmitter shoot;

    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        ess = GameObject.Find("EnemyShipGenerator").GetComponent<EnemyShipScript>();
        fss = GameObject.Find("FriendlyShipGenerator").GetComponent<FriendlyShipScript>();
        lighthouse = GameObject.Find("Headless_light_house");
        friendlyGenerator = GameObject.Find("FriendlyShipGenerator");

        offset = new Vector3(2, 0, 2);

        //cannonball = GameObject.FindGameObjectWithTag("CannonballAttack");
        //cannonballTransform = cannonball.transform;

        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        //transform.rotation = originalRotation;

        direction = (lighthouse.transform.position - transform.position);
       
        //transform.rotation = originalTransform.rotation;
        if (Vector3.Distance(lighthouse.transform.position, transform.position) <= 48.45f && isEllipse==false && isHunting==false)
        {

            isEllipse = true;
            theta = (Mathf.Atan2(direction.x, direction.z) + Mathf.PI / 2);
            originalDirection.z = a * Mathf.Cos(theta);
            originalDirection.x = b * Mathf.Sin(theta);
        }

        if (isEllipse && rotateTimer>=rotateThreshold)
        {
            theta = (Mathf.Atan2(direction.x, direction.z)+(rotateTimer/rotateThreshold)* Mathf.PI / 2) +Time.deltaTime;
            direction.z = a * Mathf.Cos(theta);
            direction.x = b * Mathf.Sin(theta);


        }

        //transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90, 0));
        if (isEllipse && rotateTimer < rotateThreshold)
        {

            //control1 = 0.3f * transform.position + 0.7f * (transform.position+originalDirection);
            //control2 = new Vector3(transform.position.z + (transform.position + originalDirection).z, 0, ((transform.position + originalDirection).x - transform.position.x) * 0.5f);

           
            //direction = (transform.position + originalDirection) - catRomSpline((rotateTimer/rotateThreshold), transform.position, control1, control2, (transform.position + originalDirection));
            theta = (Mathf.Atan2(direction.x, direction.z)) + (rotateTimer/rotateThreshold)*Mathf.PI/2;
            direction.z = a * Mathf.Cos(theta);
            direction.x = b * Mathf.Sin(theta);

            rotateTimer += Time.deltaTime * rotationSpeed;



        }

        int countTargets = 0;
        

        if (isHunting == false && friendlyGenerator.GetComponent<FriendlyShipScript>().isNotEmpty()) {

            float minDist = 10000f;
            foreach (GameObject go in friendlyGenerator.GetComponent<FriendlyShipScript>().getFriendlyShips())
            {
                
                if (go == null)
                {
                    countTargets++;
                    continue;
                }
                if (Vector3.Distance(go.transform.position, transform.position) <= huntingDistance)
                {
                    isEllipse = false;
                    isHunting = true;

                    if (Vector3.Distance(go.transform.position, transform.position) < minDist)
                    {
                        huntingTargetId = countTargets;
                        minDist = Vector3.Distance(go.transform.position, transform.position);
                        target = go;
                    }
                    
                }
                countTargets++;
            }
        }
        if (isHunting && target != null)
        {
            Vector3 attackDirection = target.transform.position - transform.position;
            direction = target.transform.position - transform.position;


            if ((attackDirection).magnitude <= huntingDistance && state != States.Attacking)
            {

                StartCoroutine(Attack(attackDirection));
                

                //Destroy(cannonballAttack, 1.2f);




            }
            else if ((attackDirection).magnitude <= huntingDistance)
            {

                isHunting = false;
            }
             
        }

        

        transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90, 0));
        transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);

       // transform.position.Set(transform.position.x,0,transform.position.z);
    }

    IEnumerator Attack(Vector3 attackDirection)
    {

        state = States.Attacking;

        //throw cannonball
        // while in range
        while ((attackDirection).magnitude <= huntingDistance && target != null)
        {
           // Debug.Log("Target Pos: " + target.transform.position);

            attackDirection = (target.transform.position - transform.position);

            Quaternion cannonDirection = Quaternion.Euler(0, Mathf.Atan2(attackDirection.x, attackDirection.z) * Mathf.Rad2Deg + 90, 0);

            var cannonballAttack = (GameObject)Instantiate(ess.cannonball, transform.position, cannonDirection);

            attackDirection.y = 0;
            cannonballAttack.transform.GetComponent<Rigidbody>().AddForce(attackDirection * 6, ForceMode.Impulse);
            shoot.Play();

            if (target.GetComponent<FriendlyStatsScript>().isSink()) {

                fss.reduce();
                break;
            }
            //direction = attackDirection;
            //Destroy(cannonballAttack, 1.2f);
            yield return new WaitForSeconds(attackSpeed);
        }

        isHunting = false;
        state = States.Idle;
        target = null;

        //friendlyGenerator.GetComponent<FriendlyShipScript>().getFriendlyShips()[huntingTargetId].
    }

    Vector3 catRomSpline(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {

        Vector3 A = (-p1 + 3 * p2 - 3 * p3 + p4);
        Vector3 B = (2 * p1 - 5 * p2 + 4 * p3 - p4);
        Vector3 C = (-p1 + p3);
        Vector3 D = (2 * p2);

        Vector3 P = (A * (t * t * t) + B * (t * t) + C * t + D) / 2;

        return P;
    }


    /*private void OnDrawGizmos()
    {


        if(isHunting)Gizmos.DrawRay(transform.position, friendlyGenerator.GetComponent<FriendlyShipScript>().getFriendlyShips()[huntingTargetId].transform.position);


    }*/


}

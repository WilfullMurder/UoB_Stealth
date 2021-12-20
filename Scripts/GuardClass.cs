using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardClass : MonoBehaviour
{

    public static event System.Action OnSpottedPlayer;

    public float MoveSpeed = 5.0f;
    public float WaitTime = 0.3f;
    public float TurnSpeed = 90.0f;
    public float SpotTime = 1.0f;

    public Light SpotLight;
    public float ViewDistance;
    public LayerMask layerMask;
  

    public Transform PathHolder;

    float ViewAngle;
    float SpottedTimer;
    Transform PlayerTransform;
    Color PatrolColor;

    void OnDrawGizmos()
    {
        Vector3 StartPos = PathHolder.GetChild(0).position; //Gets the starting position (child 0 transform)
        Vector3 PreviousPos = StartPos; //Overrides previous position

        foreach (Transform PathPoint in PathHolder)
        {
            
            Gizmos.DrawSphere(PathPoint.position, 0.3f); //Draws a sphere at location
            Gizmos.DrawLine(PreviousPos, PathPoint.position); //Draws a line between previous position and next pathpoint position
            PreviousPos = PathPoint.position; //overrides previous position
        }

        //close the loop

        Gizmos.DrawLine(PreviousPos, StartPos); //draws a line between first and last positions

   
        

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * ViewDistance); //Draws a ray in the forward direction 
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ViewAngle = SpotLight.spotAngle;
        PatrolColor = SpotLight.color;
       
        //Make array of all path points

        Vector3[] PathPoints = new Vector3[PathHolder.childCount]; 

        for(int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PathHolder.GetChild(i).position;
            PathPoints[i] = new Vector3(PathPoints[i].x, transform.position.y, PathPoints[i].z);
        }

        StartCoroutine(FollowDesignatedPath(PathPoints));
    }

    IEnumerator FollowDesignatedPath(Vector3[] PathPoints)
    {
        transform.position = PathPoints[0]; //Get first path point

        int PathPointIndex = 1; //Index starts at 1

        Vector3 TargetPathPoint = PathPoints[PathPointIndex]; //Get Location of current pathpoint
        transform.LookAt(TargetPathPoint);

        while(true) //loop forever
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPathPoint, MoveSpeed * Time.deltaTime); //Move towards Target path point
            if(transform.position == TargetPathPoint)
            {
                //Target Path Point has been reached, increment PathPointIndex.
                PathPointIndex = (PathPointIndex + 1) % PathPoints.Length; //Modulus of PathPoints.Length returns zero at max to reset loop
                TargetPathPoint = PathPoints[PathPointIndex]; //set the next path point
                yield return new WaitForSeconds(WaitTime); //Wait for WaitTime
                yield return StartCoroutine(TurnToFace(TargetPathPoint));
            }
            yield return null; //Pause between each loop
        }


    }

    IEnumerator TurnToFace(Vector3 Target)
    {
        Vector3 DirectionToTarget = (Target - transform.position).normalized; //Calculate normalized Direction
        float Angle = 90.0f - Mathf.Atan2(DirectionToTarget.z, DirectionToTarget.x) * Mathf.Rad2Deg; //Calculate Angle from Atan Radian, convert to degrees
        
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, Angle)) > 0.05f) //While the absolute value is greater than 0.5
        {
            float NewAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, Angle, TurnSpeed * Time.deltaTime); //Lerp rotation using deltaTime
            transform.eulerAngles = Vector3.up * NewAngle; //Set NewAngle transform
            yield return null; // Pause between each loop
        }
    }

    bool PlayerVisible()
    {
        if(Vector3.Distance(transform.position, PlayerTransform.position) < ViewDistance) 
        {
            //The distance between player and guard is less than the view distance
            //Calculate the angle between player and guard
            Vector3 DirectionToTarget = (PlayerTransform.position - transform.position).normalized;
            float AngleBetweenGuardAndTarget = Vector3.Angle(transform.forward, DirectionToTarget);

            if(AngleBetweenGuardAndTarget < ViewAngle / 2.0f)
            {
                //Angle between guard and player is less than the half view angle

                if (!Physics.Linecast(transform.position, PlayerTransform.position, layerMask))
                {
                    //The line cast has not hit an obstacle
                    return true;
                }
            }
        }
        //the player is not visible
        return false;
    }



    void Update()
    {
        if (PlayerVisible())
        {
            SpottedTimer += Time.deltaTime;
           
        }
        else
        {
            SpottedTimer -= Time.deltaTime;
         
        }

        SpottedTimer = Mathf.Clamp(SpottedTimer, 0, SpotTime);

        SpotLight.color = Color.Lerp(PatrolColor, Color.red, SpottedTimer/SpotTime);

        if(SpottedTimer >= SpotTime)
        {
            if(OnSpottedPlayer != null)
            {
                OnSpottedPlayer();
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardClass : MonoBehaviour
{

    public float MoveSpeed = 5.0f;
    public float WaitTime = 0.3f;
    public float TurnSpeed = 90.0f;


    public Transform PathHolder;

    public float ViewDistance;

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

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * ViewDistance);
    }

    // Start is called before the first frame update
    void Start()
    {

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
        Vector3 DirectionToTarget = (Target - transform.position).normalized;
        float Angle = 90.0f - Mathf.Atan2(DirectionToTarget.z, DirectionToTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, Angle)) > 0.05f)
        {
            float NewAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, Angle, TurnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * NewAngle;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

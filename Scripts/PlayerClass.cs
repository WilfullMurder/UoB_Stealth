using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UoBStealthGame.Highscores;
using UoBStealthGame.UI;

public class PlayerClass : MonoBehaviour
{

    public event System.Action OnFinishedLevel;

    public float MoveSpeed = 7;
    public float SmoothMoveTime = 0.1f;
    public float TurnSpeed = 8;

    float Angle;
    float SmoothInputMagnitude;
    float SmoothMovementVelocity;
    Vector3 Velocity;

    public int Score = 0;
    
    Rigidbody MyRigidBody;

    bool bDisabled;



    private UI_Game UI_Game;
    public HighScoreTable HST;

    // Start is called before the first frame update
    void Start()
    {
        MyRigidBody = GetComponent<Rigidbody>();
        GuardClass.OnSpottedPlayer += Disable;
        //writer = GameObject.Find("JSONWriter").GetComponent<JSONWriter>();

        UI_Game = GameObject.Find("Canvas").GetComponent<UI_Game>();
        if(UI_Game == null)
        {
            Debug.LogError("UI_Game is null!");
        }

   
        if (HST == null)
        {
            Debug.LogError("(highScoreSystem is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 InputDirection = Vector3.zero;
        if(!bDisabled)
        {
            InputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }

        float InputMagnitude = InputDirection.magnitude;
        SmoothInputMagnitude = Mathf.SmoothDamp(SmoothInputMagnitude, InputMagnitude, ref SmoothMovementVelocity, SmoothMoveTime);

        float TargetAngle = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg;

        Angle = Mathf.LerpAngle(Angle, TargetAngle, Time.deltaTime * TurnSpeed * InputMagnitude);
        MyRigidBody.MoveRotation(Quaternion.Euler(Vector3.up * Angle));

        Velocity = transform.forward * MoveSpeed * SmoothInputMagnitude;

        MyRigidBody.MovePosition(GetComponent<Rigidbody>().position + Velocity * Time.deltaTime);
    }


    void Disable()
    {
        bDisabled = true;
    }

    void OnDestroy()
    {
        GuardClass.OnSpottedPlayer -= Disable;
    }

    void OnTriggerEnter(Collider hitCollider)
    {

        switch(hitCollider.tag)
        {

            case "Finish":
                {
                    Disable();
                    if (OnFinishedLevel != null)
                    {
                        OnFinishedLevel();
                    }
                    break;
                }

            case "Pickup":
                {
                    
                   hitCollider.gameObject.SetActive(false);
                    Score++;
                  if(UI_Game != null)
                    {
                        UI_Game.UpdateScreenScore(Score);
                    }

                    HST.SetPlayerScoreTracker(Score);
                
                    break;
                }



        }
       
    }






}

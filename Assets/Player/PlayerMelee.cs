using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    public enum MeleeState {NotMelee, WindUp, FollowThrough}

    [SerializeField]
    private MeleeState meleeState = MeleeState.NotMelee;
    
    private Collider hitBox;
    
    [SerializeField]
    private float windUpTime = 0.3f;
    
    [SerializeField]
    private float followThroughTime = 0.3f;

    private float currTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        currTime -= Time.deltaTime;

        switch (meleeState){
            case MeleeState.FollowThrough:
                if (currTime < 0.0f){
                    EndHit();
                }
                break;
            case MeleeState.WindUp:
                if (currTime < followThroughTime){
                    MeleeHit();
                }
                break;
            case MeleeState.NotMelee:
                if ( Input.GetButtonDown("Fire1") ){
                    StartMelee();
                }
                break;
        }
        

        
    }

    void StartMelee()
    {
        Debug.Log("WindUp!");
        currTime = windUpTime + followThroughTime;
        meleeState = MeleeState.WindUp;
    }

    void MeleeHit()
    {
        Debug.Log("Hit!");
        meleeState = MeleeState.FollowThrough;
    }

    void EndHit()
    {
        Debug.Log("End!");
        currTime = 0.0f;
        meleeState = MeleeState.NotMelee;
    }
}

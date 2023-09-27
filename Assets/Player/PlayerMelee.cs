using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{

    [SerializeField]
    private GameObject hitBoxPrefab;
    
    public enum MeleeState {NotMelee, WindUp, FollowThrough}

    [SerializeField]
    private MeleeState meleeState = MeleeState.NotMelee;
    
    
    private PlayerMovement playerMovement;
    
    private Collider hitBox;
    
    [SerializeField]
    private float windUpTime = 0.3f;
    
    [SerializeField]
    private float followThroughTime = 0.3f;

    [SerializeField] 
    private float offset = 1.6f / 2.0f;
    
    private float currTime = 0.0f;

    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

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
        Instantiate(hitBoxPrefab, new Vector3(transform.position.x + Mathf.Sign(playerMovement.moveInput) * offset, transform.position.y, 0.0f), Quaternion.identity);
    }

    void EndHit()
    {
        Debug.Log("End!");
        currTime = 0.0f;
        meleeState = MeleeState.NotMelee;
    }
}

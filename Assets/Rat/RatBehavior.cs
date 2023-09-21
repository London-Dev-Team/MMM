using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatBehavior : MonoBehaviour
{

    public enum MovementState { Moving, Stopped, OnFlywheel };
    private MovementState movementState = MovementState.Moving;
    private enum DirMoving { Right = 1, Left = -1};
    private DirMoving dirMoving = DirMoving.Right;

    [SerializeField]
    private float speed = 5.0f;
    
    // Start is called before the first frame update
  
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;

    [SerializeField]
    private float minMoveTime = 1.0f;
    [SerializeField]
    private float maxMoveTime = 2.0f;
    private float moveTimeTarget;
    private float moveTime = 0.0f;
    
    [SerializeField]
    private float minStopTime = 1.0f;
    [SerializeField]
    private float maxStopTime = 2.0f;
    private float stopTimeTarget;
    private float stopTime = 0.0f;
    
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = Vector3.right * speed;
        boxCollider2D = GetComponent<BoxCollider2D>();
        SetNewMoveTimeTarget();
        SetNewStopTimeTarget();
    }

    private void Update()
    {
        if (movementState == MovementState.OnFlywheel){
            return;
        }
        
        UpdateMoveTimer();
        UpdateStopTimer();
    }

    
    private void UpdateMoveTimer()
    {
        if (movementState == MovementState.Moving){
            moveTime += Time.deltaTime;
        } 
        if (moveTime > moveTimeTarget){
            SetState(MovementState.Stopped);
        }
    }

    private void UpdateStopTimer()
    {
        if (movementState == MovementState.Stopped){
            stopTime += Time.deltaTime;
        }
        if (stopTime > stopTimeTarget){
            SetState(MovementState.Moving);
        }
    }

    private void SetNewMoveTimeTarget()
    {
        moveTimeTarget = Random.Range(minMoveTime, maxMoveTime);
    }
    
    private void SetNewStopTimeTarget()
    {
        stopTimeTarget = Random.Range(minStopTime, maxStopTime);
    }
    
    
    public void SetState(MovementState newState)
    {
        switch(newState) 
        {
            case MovementState.Moving:
                SetMovingState();
                break;
            case MovementState.Stopped:
                SetStoppedState();
                break;
            case MovementState.OnFlywheel:
                SetOnFlywheelState();
                break;
            default:
                Debug.LogError("Unknown MovementState passed");
                break;
        }
    }
    
    private void SetMovingState()
    {
        stopTime = 0.0f;
        movementState = MovementState.Moving;
        SetNewStopTimeTarget();
    }
    
    private void SetStoppedState()
    {
        movementState = MovementState.Stopped; 
        moveTime = 0.0f;
        rb2D.velocity = new Vector2( 0.0f, rb2D.velocity.y);
        SetNewMoveTimeTarget();
    }
    
    private void SetOnFlywheelState()
    {
        movementState = MovementState.OnFlywheel; 
        rb2D.velocity = new Vector2( 0.0f, rb2D.velocity.y);
    }
    
    private void FixedUpdate()
    {
        if (movementState != MovementState.Moving){
            return;
        }
        
        rb2D.velocity = new Vector2( (int)dirMoving * speed, rb2D.velocity.y);
    
        var hit = Physics2D.Raycast(transform.position, new Vector2( Math.Sign(rb2D.velocity.x) * 1.0f , 0.0f), 0.6f);

        if (hit.collider != null)
        {
            dirMoving = (DirMoving)(-1 * (int) dirMoving);
        }
        
    }
}

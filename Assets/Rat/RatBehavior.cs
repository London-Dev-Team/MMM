using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class RatBehavior : MonoBehaviour
{

    private enum MovementState { Moving, Stopped };
    private MovementState movementState = MovementState.Moving;
    private enum DirMoving { Right = 1, Left = -1};
    private DirMoving dirMoving = DirMoving.Right;

    [SerializeField]
    private float speed = 5.0f;
    
    // Start is called before the first frame update
  
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D.velocity = Vector3.right * speed;
    }

    void FixedUpdate()
    {
        rb2D.velocity = new Vector2( (int)dirMoving * speed, rb2D.velocity.y);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2( Math.Sign(rb2D.velocity.x) * 1.0f , 0.0f), 0.6f);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            dirMoving = (DirMoving)(-1 * (int) dirMoving);
        }
        
    }
}

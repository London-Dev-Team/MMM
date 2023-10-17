using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerOnMovingPlatform : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    public LayerMask whatIsGround;

    private Rigidbody2D mp; // Moving Platform

    private float movingPlatformVelocity;


    void Start()
    {
        rb  = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            mp = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            mp = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log(mp.velocity.x);
            // rb.AddForce(new Vector2(mp.velocity.x, 0));
        }
    }

}

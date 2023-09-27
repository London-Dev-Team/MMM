using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RBGravityBehaviour : MonoBehaviour
{

    private Rigidbody2D rb;
    
    [SerializeField] float velocityThreshold = 1f;

    [SerializeField] float gravityScale = 3.5f;
    [SerializeField] float fallGravityScale = 6.75f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (rb.velocity.y < -(velocityThreshold))
        {
            rb.gravityScale = fallGravityScale;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

    }

}

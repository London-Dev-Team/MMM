using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RBGravityBehaviour : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] float gravityScale = 3.5f;
    [SerializeField] float fallGravityScale = 6.75f;

    [SerializeField] float velocityThreshold = 5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        rb.gravityScale = gravityScale;
        if (!(rb.velocity.y < 1 && rb.velocity.y > -1))
        {
            rb.gravityScale = fallGravityScale;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBPlatformAttach : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);   
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == rb)
        {
            rb.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == rb)
        {
            rb.transform.parent = null;
        }
    }
    */

}

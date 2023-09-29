using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyWheelMovement : MonoBehaviour
{

    [SerializeField] float spinSpeed;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}

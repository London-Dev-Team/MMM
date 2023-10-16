using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelBehaviour : MonoBehaviour
{
    
    
    [SerializeField]
    private MechComponent mechComponent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("HitBox")){
            return;
        }

        
        if (mechComponent.isSmoking){
            mechComponent.Fix();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ConcentrationPiece : MonoBehaviour
{

    private Rigidbody2D rb;

    public bool isSlotted = false;

    [SerializeField]
    private float maxTimeUntilSlot = 1.0f;
    private float currTimeUntilSlot = 0.0f;
    
    [SerializeField]
    private float maxForce = 1000.0f;
    [SerializeField]
    private float minForce = 100.0f;
    [SerializeField]
    private float spread = 30.0f;
    
    [SerializeField]
    public ConcentrationSlot.ConcentrationSlotShape pieceShape = ConcentrationSlot.ConcentrationSlotShape.Square;

    private Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mat = GetComponent<MeshRenderer>().materials[0];
    }

    private void Update()
    {
        
        mat.color = Color.red;
        currTimeUntilSlot -= Time.deltaTime;
        if (currTimeUntilSlot < 0.0f){
            currTimeUntilSlot = 0.0f;
        }
        else{
            mat.color = Color.blue;
        }
    }

    public void Slot(Vector3 freezePosition)
    {
        if (currTimeUntilSlot != 0.0f){
            return;
        }
        
        Debug.Log("Slotted!");
        isSlotted = true;
        transform.position = freezePosition;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.rotation = Quaternion.identity;
    }
    
    public void Unslot()
    {
        currTimeUntilSlot = maxTimeUntilSlot;
        Debug.Log("Unslotted!");
        isSlotted = false;
        rb.constraints = RigidbodyConstraints2D.None;
        Bump();
    }
    
    
    public void Bump()
    {
        float force = Random.Range(minForce, maxForce);
        float angle = Random.Range(-spread  * Mathf.PI / 180.0f , spread  * Mathf.PI / 180.0f);
        angle += 90.0f * Mathf.PI / 180.0f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        rb.AddForce( direction * force);
    }
    
}

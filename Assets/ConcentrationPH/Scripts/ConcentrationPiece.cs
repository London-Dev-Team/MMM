using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConcentrationPiece : MonoBehaviour
{

    private Rigidbody2D rb;

    public bool isSlotted = false;
    
    [SerializeField]
    public ConcentrationSlot.ConcentrationSlotShape pieceShape = ConcentrationSlot.ConcentrationSlotShape.Square;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Slot(Vector3 freezePosition)
    {
        Debug.Log("Slotted!");
        isSlotted = true;
        transform.position = freezePosition;
    }
    
    public void Unslot()
    {
        Debug.Log("Unslotted!");
        isSlotted = false;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationSlot : MonoBehaviour
{
    public enum ConcentrationSlotShape {Square, Triangle, Sphere}

    [SerializeField]
    private ConcentrationSlotShape slotShape = ConcentrationSlotShape.Square;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ConcentrationPiece") ){
            ConcentrationPiece piece = col.gameObject.GetComponent<ConcentrationPiece>();
            if (piece.isSlotted){
                return;
            }

            if (piece.pieceShape != slotShape){
                return;
            }

            piece.Slot(transform.position - new Vector3(0, 0.25f, 0));
        }
    }
}

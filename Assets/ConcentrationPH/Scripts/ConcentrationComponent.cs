using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationComponent : MechComponent
{
    private List<ConcentrationPiece> pieceList = new List<ConcentrationPiece>();
    private List<ConcentrationSlot> slotList = new List<ConcentrationSlot>();

    public override void Start()
    {
        base.Start();
        foreach (Transform go in transform){
            ConcentrationPiece piece = go.gameObject.GetComponent<ConcentrationPiece>();
            if (piece != null){
                pieceList.Add(piece);
                continue;
            }
            
            ConcentrationSlot slot = go.gameObject.GetComponent<ConcentrationSlot>();
            if (slot != null){
                slotList.Add(slot);
            }
        }
        
    }

    public override void Update()
    {
        base.Update();

        bool allSlotted = true;
        foreach (ConcentrationPiece piece in pieceList){
            if (!piece.isSlotted){
                allSlotted = false;
            }
        }

        if (allSlotted){
            switch (mechComponentState){
                case MechComponentState.Broken:
                    Fix();
                    break;
                case MechComponentState.NotStarted:
                    StartComponent();
                    break;
                
            }
        }
    }
    
    public override bool StartComponent()
    {
        if (!base.StartComponent())
        {
            return false;
        }
        // Anything unique to fixing a component goes here
        // E.g. when you FIRST start a fan, it plays a sound
        Debug.Log("Started ConcentrationComponent!");
        return true;
    }

    public override bool Fix()
    {
        if (!base.Fix())
        {
            return false;
        }
        // Anything unique to fixing a component goes here
        // E.g. when you fix a fan you want it to start spinning
        Debug.Log("Fixed ConcentrationComponent!");
        return true;
    }


    public override bool Break()
    {
        if (!base.Break())
        {
            return false;
        }

        foreach (ConcentrationPiece piece in pieceList){
            piece.Unslot();
        }
        
        // Anything unique to fixing a component goes here
        // E.g. when you break a fan you want it to stop spinning
        Debug.Log("Broke ConcentrationComponent!");
        return true;
    }



    public override void ResetComponent()
    {
        base.ResetComponent();
        Debug.Log("Reset ConcentrationComponent!");
    }


    public override void ChildSaveProperties()
    {
        Debug.Log("Saving data specific to ConcentrationComponent!");
        // PlayerPrefs.SetString("ComponentProperty", componentProperty);
        // PlayerPrefs.Save();
    }
    
    
    public override void ChildLoadProperties()
    {
        Debug.Log("Loading data specific to ConcentrationComponent!");
        // String loadedComponentProperty = PlayerPrefs.GetString("ComponentProperty");
        // componentProperty = loadedComponentProperty;
    }
    
    public override void StartSmoking()
    {
        Debug.Log("Started Smoking!");
    }
    
    public override void StopSmoking()
    {
        Debug.Log("Stopped Smoking!");
    }
    
}

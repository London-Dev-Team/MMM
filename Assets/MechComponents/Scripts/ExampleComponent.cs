using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleComponent : MechComponent
{
    
    protected override void Fix()
    {
        base.Fix();
        // Anything unique to fixing a component goes here
        // E.g. when you fix a fan you want it to start spinning
        Debug.Log("Fixed ExampleComponent!");
    }


    protected override void Break()
    {
        base.Break();
        // Anything unique to fixing a component goes here
        // E.g. when you break a fan you want it to stop spinning
        Debug.Log("Broke ExampleComponent!");
    }

}

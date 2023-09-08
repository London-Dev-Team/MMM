using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleComponent : MechComponent
{
    public override bool StartComponent()
    {
        if (!base.StartComponent())
        {
            return false;
        }
        // Anything unique to fixing a component goes here
        // E.g. when you FIRST start a fan, it plays a sound
        Debug.Log("Started ExampleComponent!");
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
        Debug.Log("Fixed ExampleComponent!");
        return true;
    }


    public override bool Break()
    {
        if (!base.Break())
        {
            return false;
        }

        // Anything unique to fixing a component goes here
        // E.g. when you break a fan you want it to stop spinning
        Debug.Log("Broke ExampleComponent!");
        return true;
    }



    public override void ResetComponent()
    {
        base.ResetComponent();
        Debug.Log("Reset ExampleComponent!");
    }

}

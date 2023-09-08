using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechComponent : MonoBehaviour
{
    public enum MechComponentState { NotStarted, Running, Broken };
    private MechComponentState mechComponentState = MechComponentState.NotStarted;


    protected virtual void Fix()
    {
        mechComponentState = MechComponentState.Running;
    }


    protected virtual void Break()
    {
        mechComponentState = MechComponentState.Broken;
    }


    MechComponentState GetState()
    {
        return mechComponentState;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public abstract class MechComponent : MonoBehaviour
{
    public enum MechComponentState { NotStarted, Running, Broken };
    [SerializeField]
    public MechComponentState mechComponentState = MechComponentState.NotStarted;

    public virtual bool StartComponent()
    {
        if (mechComponentState != MechComponentState.NotStarted)
        {
            Debug.LogError("Cannot StartComponent() this component unless it's NotStarted!");
            return false;
        }
        mechComponentState = MechComponentState.Running;
        return true;
    }

    public virtual bool Fix()
    {
        if (mechComponentState != MechComponentState.Broken)
        {
            Debug.LogError("Cannot Fix() this component unless it's Broken!");
            return false;
        }

        mechComponentState = MechComponentState.Running;
        return true;
    }


    public virtual bool Break()
    {
        if (mechComponentState != MechComponentState.Running)
        {
            Debug.LogError("Cannot Break() this component unless it's Running!");
            return false;
        }
        mechComponentState = MechComponentState.Broken;
        return true;
    }


    public MechComponentState GetState()
    {
        return mechComponentState;
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(transform.position.x, transform.position.y,  125, 50), "Fix"))
            Fix();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 60,  125, 50),
                    "Break"))
            Break();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 120, 125, 50),
                    "Start"))
            StartComponent();
    }

}


[CustomEditor(typeof(MechComponent), true)]
public class ComponentState : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        MechComponent myScript = (MechComponent)target;

        if (GUILayout.Button("Start"))
        {
            myScript.StartComponent();
        }

        if (GUILayout.Button("Fix"))
        {
            myScript.Fix();
        }

        if (GUILayout.Button("Break"))
        {
            myScript.Break();
        }
    }
}
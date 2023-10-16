using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;


public abstract class MechComponent : MonoBehaviour, ISerializedActObject
{
    public enum MechComponentState { NotStarted, Running, Broken };
    [SerializeField]
    public MechComponentState mechComponentState = MechComponentState.NotStarted;
    
    [SerializeField]
    private float currentBreakTime = 0.0f;
    [SerializeField]
    private float minBreakTime = 1.0f;
    [SerializeField]
    private float maxBreakTime = 2.0f;
    private float targetBreakTime = 2.0f;
    
    [SerializeField]
    private float smokingTimeThreshold = 1.0f; // The last 1 second is when to start smoking.
    public bool isSmoking = false;

    public virtual void Start()
    {
        SetNewBreakTarget();
    }

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
        if (mechComponentState == MechComponentState.NotStarted)
        {
            Debug.LogError("Cannot Fix() this component if it's NotStarted!");
            return false;
        }
        
        isSmoking = false;
        SetNewBreakTarget();
        currentBreakTime = 0.0f;
        StopSmoking();
        
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
        
        isSmoking = false;
        currentBreakTime = 0.0f;
        StopSmoking();
        
        return true;
    }

    
    public virtual void ResetComponent()
    {
        mechComponentState = MechComponentState.NotStarted;
    }
    
    
    public virtual void Update()
    {
        if (mechComponentState == MechComponentState.Running){
            UpdateBreakTimer();
        }
    }

    public abstract void StartSmoking();
    public abstract void StopSmoking();
    
    private void UpdateBreakTimer()
    {
        currentBreakTime += Time.deltaTime;

        if ( currentBreakTime > (targetBreakTime - smokingTimeThreshold) ){
            if (!isSmoking){
                StartSmoking();
            }
            isSmoking = true;
        }
        if ( currentBreakTime < targetBreakTime ){
            return;
        }

        Break();
    }

    private void SetNewBreakTarget()
    {
        targetBreakTime = Random.Range(minBreakTime, maxBreakTime);
    }
    
    public MechComponentState GetState()
    {
        return mechComponentState;
    }

    public void SaveProperties()
    {
        ChildSaveProperties();
        PlayerPrefs.SetInt("mechComponentState", (int)mechComponentState);
        PlayerPrefs.SetFloat("currentBreakTime", currentBreakTime);
        PlayerPrefs.SetFloat("targetBreakTime", targetBreakTime);
        PlayerPrefs.Save();
        Debug.Log("saved mechComponentState " + mechComponentState );
    }

    public void LoadProperties()
    {
        ChildLoadProperties();
        if (PlayerPrefs.HasKey("mechComponentState"))
        {
            int loadedMechComponentState = PlayerPrefs.GetInt("mechComponentState");
            mechComponentState = (MechComponentState)loadedMechComponentState;
            float loadedCurrentBreakTime = PlayerPrefs.GetFloat("currentBreakTime");
            currentBreakTime = loadedCurrentBreakTime;
            float loadedTargetBreakTime = PlayerPrefs.GetFloat("targetBreakTime");
            targetBreakTime = loadedTargetBreakTime;

        }
        else{
            Debug.LogError("There is no unlock save data!");
            mechComponentState = MechComponentState.NotStarted;
        }
    }

    public abstract void ChildSaveProperties();


    public abstract void ChildLoadProperties();
    
    
    void OnGUI()
    {
        if (GUI.Button(new Rect(transform.position.x, transform.position.y, 125, 50), "Fix"))
            Fix();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 60, 125, 50),
                    "Break"))
            Break();
        if (GUI.Button(new Rect(transform.position.x, transform.position.y + 120, 125, 50),
                    "Start"))
            StartComponent();
        
        GUI.Label(new Rect(transform.position.x + 40,transform.position.y + 180, 125, 50), mechComponentState.ToString());
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
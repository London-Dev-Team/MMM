using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class ActManager : MonoBehaviour
{


    [Header("Manager References")]

    [SerializeField]
    private ProgressManager progressManager;


    [Header("For Pausing")]

    [SerializeField]
    private bool isUpdatingProgress = true;


    [Header("Components")]

    [SerializeField]
    private List<GameObject> componentObjectList = new List<GameObject>();

    private List<MechComponent> componentList = new List<MechComponent>();

    [Header("Progress Variables")]

    [SerializeField]
    private int winThreshold = 100;

    [SerializeField]
    private int loseThreshold = 0;

    [SerializeField]
    private int startingProgress = 50;

    [SerializeField]
    private int progressCounter = 0;

    private bool allComponentsStarted = false;

    public enum ActState { Playing, Won, Lost };
    [SerializeField]
    public ActState actState = ActState.Playing;

    [Range(0, 5)]
    [SerializeField]
    private int actIndex = 0;


    private float updateTimer = 0.0f;
    private float tickTime = 1.0f;


    GUIStyle style;

    void Start()
    {
        style = new GUIStyle();
        style.normal.textColor = Color.black;

        progressCounter = startingProgress;
        foreach (GameObject componentObj in componentObjectList)
        {
            componentList.Add(componentObj.GetComponent<MechComponent>());
        }

    }


    void Update()
    {

        if (isUpdatingProgress)
        {
            updateTimer += Time.deltaTime;
            if (updateTimer > tickTime)
            {
                updateTimer = 0.0f;
                UpdateProgress();
            }
        }

        if (!allComponentsStarted){
            allComponentsStarted = true;
            foreach (MechComponent component in componentList)
            {
                if (component.mechComponentState != MechComponent.MechComponentState.Running){
                    allComponentsStarted = false;
                }
            }
        }
        
    }


    void StopAct()
    {
        isUpdatingProgress = false;
    }


    void StartAct()
    {
        isUpdatingProgress = true;
    }


    void UpdateProgress()
    {

        if (!allComponentsStarted){
            return;
        }
        
        foreach (MechComponent component in componentList)
        {
            if (component.GetState() == MechComponent.MechComponentState.Running)
            {
                progressCounter++;
            }
            else if (component.GetState() == MechComponent.MechComponentState.Broken)
            {
                progressCounter--;
            }
            else if (component.GetState() == MechComponent.MechComponentState.NotStarted)
            {
                ;
            }
        }

        if (progressCounter <= loseThreshold)
        {
            LoseAct();
        }
        else if (progressCounter >= winThreshold)
        {
            WinAct();
        }
    }


    public void WinAct()
    {
        if (actState == ActState.Won)
        {
            Debug.LogError("You've already won! Reset to try again!");
            return;
        }

        Debug.Log("ACT WON!");
        actState = ActState.Won;
        if (progressCounter < winThreshold)
        {
            progressCounter = winThreshold;
        }
        progressManager.UnlockAct(actIndex + 1);
        progressManager.SaveProgress();
        StopAct();
    }


    public void LoseAct()
    {
        if (actState == ActState.Lost)
        {
            Debug.LogError("You've already lost! Reset to try again!");
            return;
        }

        Debug.Log("ACT LOST!");
        actState = ActState.Lost;
        if (progressCounter > loseThreshold)
        {
            progressCounter = loseThreshold;
        }
        StopAct();
    }

    public void ResetAct()
    {
        Debug.Log("RESET ACT!");

        foreach (MechComponent component in componentList)
        {
            component.ResetComponent();
        }
        progressCounter = startingProgress;
        actState = ActState.Playing;
        StartAct();
    }

    

    void OnGUI()
    {
        GUI.Label(new Rect(transform.position.x,
            transform.position.y, 125, 50), progressCounter + " / " + winThreshold,
            style);
    }
}

[CustomEditor(typeof(ActManager))]
public class ActProgress : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        ActManager myScript = (ActManager)target;

        if (GUILayout.Button("Win Act"))
        {
            myScript.WinAct();
        }

        if (GUILayout.Button("Lose Act"))
        {
            myScript.LoseAct();
        }

        if (GUILayout.Button("Reset Act"))
        {
            myScript.ResetAct();
        }
    }
}
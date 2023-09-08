using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

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

    [Header("Progress Variable")]

    [SerializeField]
    private int winThreshold = 100;

    [SerializeField]
    private int loseThreshold = 0;

    [SerializeField]
    private int progressCounter = 50;

    [Range(0, 5)]
    [SerializeField]
    private int actIndex = 0;


    private float updateTimer = 0.0f;
    private float tickTime = 1.0f;

    void Start()
    {
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

        if (progressCounter <= loseThreshold)
        {
            LoseAct();
            StopAct();
        }
        else if (progressCounter >= winThreshold)
        {
            WinAct();
            StopAct();
        }

    }

    void StopAct(){
        isUpdatingProgress = false;
    }

    void UpdateProgress()
    {

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
    }


    public void WinAct()
    {
        Debug.Log("ACT WON!");
        progressManager.UnlockAct(actIndex + 1);
        progressManager.SaveProgress();
    }


    public void LoseAct()
    {
        Debug.Log("ACT LOST!");
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
    }
}
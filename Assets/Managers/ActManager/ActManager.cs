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

    // This will be a list of Components instead of ints
    private List<int> componentList = new List<int>();

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
            // Below will be uncommented once component script exists:
            // componentList.Add(componentObj.GetComponent<Component>());
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
        }
        else if (progressCounter >= winThreshold)
        {
            WinAct();
        }

    }


    void UpdateProgress()
    {

        // This int will be a Component script
        foreach (int component in componentList)
        {
            // If component.state = RUNNING
            if (component == 0)
            {
                progressCounter++;
            }
            // If component.state = BROKEN
            else if (component == 1)
            {
                progressCounter--;
            }
            // If component.state = NOTSTARTED
            else if (component == 2)
            {
                ;
            }
        }
    }



    public void WinAct()
    {
        Debug.Log("ACT WON!");
        progressManager.UnlockAct(actIndex+1);
        progressManager.SaveProgress();
    }


    public void LoseAct()
    {
        Debug.Log("ACT LOST!");
    }
}

[CustomEditor(typeof(ActManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        ActManager myScript = (ActManager)target;

        if(GUILayout.Button("Win Act"))
        {
            myScript.WinAct();
        }

        if(GUILayout.Button("Lose Act"))
        {
            myScript.LoseAct();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActManager : MonoBehaviour
{

    [SerializeField]
    private bool isUpdatingProgress = true;

    [SerializeField]
    private List<GameObject> componentObjectList = new List<GameObject>();

    // This will be a list of Components instead of ints
    private List<int> componentList = new List<int>();

    [SerializeField]
    private int winThreshold = 100;

    [SerializeField]
    private int loseThreshold = 0;

    [SerializeField]
    private int progressCounter = 50;


    private float updateTimer = 0.0f;

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
            if (updateTimer > 1.0f)
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


    void WinAct()
    {
        Debug.Log("ACT WON!");
    }


    void LoseAct()
    {
        Debug.Log("ACT LOST!");
    }
}

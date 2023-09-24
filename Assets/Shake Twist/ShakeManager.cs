using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class ShakeManager : MonoBehaviour
{

    private List<Rigidbody2D> allRBs = new List<Rigidbody2D>();

    [SerializeField]
    private float maxForce = 1000.0f;
    [SerializeField]
    private float minForce = 100.0f;
    [SerializeField]
    private float spread = 30.0f;
    
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects){
            
            if (!go.activeInHierarchy){
                continue;
            }
            
            Rigidbody2D[] rbs = go.GetComponents<Rigidbody2D>();

            if (rbs.Length == 0){
                continue;
            }
            
            if (rbs[0] != null)
            {
                allRBs.Add(rbs[0]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bump()
    {
        foreach (Rigidbody2D rb in allRBs){
            float force = Random.Range(minForce, maxForce);
            float angle = Random.Range(-spread  * Mathf.PI / 180.0f , spread  * Mathf.PI / 180.0f);
            angle += 90.0f * Mathf.PI / 180.0f;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.AddForce( direction * force);
        }
    }
    
}


[CustomEditor(typeof(ShakeManager))]
public class ShakeManagerGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Functions", EditorStyles.boldLabel);

        ShakeManager myScript = (ShakeManager)target;

        if (GUILayout.Button("Bump!"))
        {
            myScript.Bump();
        }
    }
}
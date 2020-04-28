using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class SpawnTool : EditorWindow
{
    private Vector3 centerPoint;
    private int squareSize = 10;
    private string spawnedObjectName = "Object";
    private Transform parentObject;
    private GameObject spawnObject;
    private float minimumDistance;
    private int iterations = 100;
    private string placeOnTag;

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Tools/SpawnTool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpawnTool)); // create window
    }

    private void OnGUI()
    {
        // Title
        GUILayout.Label("SpawnTool", EditorStyles.boldLabel);

        centerPoint = EditorGUILayout.Vector3Field("Center Point", centerPoint);
        spawnObject = EditorGUILayout.ObjectField("Spawnable", spawnObject, typeof(GameObject), false) as GameObject;
        minimumDistance = EditorGUILayout.FloatField("Minimum distance between spawned entities", minimumDistance);
        spawnedObjectName = EditorGUILayout.TextField("Spawned name", spawnedObjectName);
        iterations = EditorGUILayout.IntField("Iterations", iterations);
        placeOnTag = EditorGUILayout.TagField("Tag to place on", placeOnTag);
        squareSize = EditorGUILayout.IntField("SquareSize", squareSize);

        if(GUILayout.Button("Spawn them"))
        {
            SpawnObjects(iterations);
        }
    }

    private void SpawnObjects(int iterations)
    {
        int failedAttempts = 0;
        List<Vector3> spawned = new List<Vector3>();
        for (int i = 0; i < iterations; i++)
        {
            if(failedAttempts >= 10000)
            {
                return;
            }

            Vector3 objectPos = GetRandomVector3(-squareSize, squareSize); // make a randomized vector in the specified space

            objectPos.y = 0; // put the object at ground-height

            // check if close to other spawned entities#
            bool wellPlaced = true;
            foreach (Vector3 vector in spawned)
            {
                if (GetDistanceXY(vector, objectPos) < minimumDistance)
                {
                    wellPlaced = false;
                    break;
                }
            }
            if(wellPlaced)
            {
                GameObject spawnedObject = Instantiate(spawnObject, objectPos, Quaternion.identity);
                Quaternion quart = spawnedObject.transform.rotation;
                quart.y = Random.Range(0, float.MaxValue);
                spawnedObject.transform.rotation = quart;
                spawnedObject.name = spawnedObjectName;
                spawned.Add(objectPos);
            }
            else
            {
                i--;
                failedAttempts++;
            }
        }
    }

    private Vector3 GetRandomVector3(float minValue, float maxValue)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Random.Range(minValue, maxValue);
        vector.y = Random.Range(minValue, maxValue);
        vector.z = Random.Range(minValue, maxValue);
        return vector;
    }

    private float GetDistanceXY(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2); // TO DO
    }
}

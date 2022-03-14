using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShadowVariation : MonoBehaviour
{
    Vector3[,] points = new Vector3[10, 10];
    int[,] shadowMatrix = new int[10, 10];
    int[,] changeMatrix = new int[10, 10];
    HashSet<int[,]> shadowStateList = new HashSet<int[,]>();
    int[,] changeFrequencyHeatmap = new int[10, 10];
    int numOfUpdates = 0;
    bool shadowFrequencyRun = false;
    int prevUpdateTime = -1;
    GameObject sun = null;
    bool simulating = false;
    Stopwatch watch;

    // public GameObject CFHPlane;

    // Start is called before the first frame update
    void Start()
    {
        watch = new System.Diagnostics.Stopwatch();
        sun = GameObject.Find("Sun");
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (simulating)
        {

            if (!watch.IsRunning) watch.Start();
            checkShadow(sun);
        }
        else
        {
            if (watch.IsRunning) watch.Stop();
        }


        // if ((int)(sun.transform.rotation.eulerAngles.x) == 359 && !shadowFrequencyRun)
        // {
        //     shadowFrequecency();
        //     shadowFrequencyRun = true;
        //     computeCPH();
        //     //sun.GetComponent<SunSim>().StopSimulation();
        // }

    }

    public void setSimulating(bool b)
    {
        simulating = b;
    }

    void init()
    {
        // Getting the middle point of the park, and computing the starting point of the grid
        var planePos = transform.position;
        var planeSca = transform.localScale * 10;
        var startPoint = new Vector3(planePos.x - (planeSca.x / 2), planePos.y, planePos.z - (planeSca.z / 2));

        // Splitting the park into 10 square cells and getting the point in the middle of each cell
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var newpoint = new Vector3((float)(startPoint.x + 5 + (i * 10)), startPoint.y, (float)(startPoint.z + 5 + (j * 10)));
                points[i, j] = newpoint;
                // Initialising the shadow and change matrices with zeros
                shadowMatrix[i, j] = 0;
                changeMatrix[i, j] = 0;
            }
        }
    }

    public (int[,], int) getShadowVariationMatrix()
    {
        return (shadowMatrix, numOfUpdates);
    }

    public (int[,], int) getCFHMatrix()
    {
        int[,] previousState = null;
        UnityEngine.Debug.Log(shadowStateList.Count);

        foreach (var shadowMatrix in shadowStateList)
        {
            if (previousState != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (shadowMatrix[i, j] != previousState[i, j])
                        {
                            changeMatrix[i, j]++;
                        }
                    }
                }
            }
            previousState = shadowMatrix;
        }

        // Finding the max value of the changeMatrix
        var maxValue = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (changeMatrix[i, j] > maxValue)
                {
                    maxValue = changeMatrix[i, j];
                }
            }
        }

        return (changeMatrix, maxValue);
    }

    void checkShadow(GameObject sun)
    {
        if (watch.Elapsed.Seconds != prevUpdateTime)
        {
            int[,] shadowStates = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var hit = Physics.Raycast(points[i, j], sun.transform.forward * -1, Mathf.Infinity);
                    if (hit)
                    {
                        shadowStates[i, j] = 0;
                        shadowMatrix[i, j]++;
                    }
                    else
                    {
                        shadowStates[i, j] = 1;
                    }
                }
            }

            prevUpdateTime = watch.Elapsed.Seconds;
            shadowStateList.Add(shadowStates);
            numOfUpdates++;
        }
    }

    public void clearMatrices()
    {
        // Reset the matrices used to calculate shadow variation and CFH
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                shadowMatrix[i, j] = 0;
            }
        }
        shadowStateList.Clear();
        numOfUpdates = 0;
        UnityEngine.Debug.Log(shadowStateList.Count);
    }
}

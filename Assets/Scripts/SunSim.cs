using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunSim : MonoBehaviour
{
    private bool simulating = false;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (simulating)
        {
            Vector3 v = new Vector3(0.2f, 0, 0);
            transform.Rotate(v);
        }

    }
    public bool StartStopSimulation()
    {
        simulating = !simulating;
        return simulating;
    }
    public void simulationFinished()
    {
        simulating = false;
        transform.rotation = Quaternion.Euler(90f, 0f, 60f);
    }

    public void startNewSimulation()
    {
        transform.rotation = Quaternion.Euler(15f, 0f, 15f);
        simulating = true;
    }
}
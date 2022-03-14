using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInteraction : MonoBehaviour
{
    public Text simText;
    public Text visScore;
    public Text editMode;
    public Text skyVisScore;
    GameObject landmark;
    GameObject sun;
    GameObject results;
    GameObject shadowVariation;
    GameObject buildingManager;
    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager");
        shadowVariation = GameObject.Find("Park");
        sun = GameObject.Find("Sun");
        landmark = GameObject.Find("LandmarkManager");
        results = GameObject.Find("Results");
    }

    // Update is called once per frame
    void Update()
    {
        var angle = (int)(sun.transform.rotation.eulerAngles.x);
        if (angle == 359)
        {
            sun.GetComponent<SunSim>().simulationFinished();
            shadowVariation.GetComponent<ShadowVariation>().setSimulating(false);
            var startButton = GameObject.Find("SimBtn");
            startButton.GetComponentInChildren<Text>().text = "New Simulation";
        }
    }
    public void showShadowVariation()
    {
        var (matrix, numOfUpdates) = shadowVariation.GetComponent<ShadowVariation>().getShadowVariationMatrix();
        results.GetComponent<Results>().showHeatMap(matrix, numOfUpdates, "Shadow Variation");
    }
    public void showCFH()
    {
        var (matrix, maxValue) = shadowVariation.GetComponent<ShadowVariation>().getCFHMatrix();
        results.GetComponent<Results>().showHeatMap(matrix, maxValue, "Change Frequency Heatmap");
    }
    public void hideResults()
    {
        results.GetComponent<Results>().showHideResults(false);
    }
    public void landmarkVisibility()
    {
        var vis = landmark.GetComponent<RayCasting>().computeVisibility();
        visScore.text = "Visibility Score: " + vis;
    }
    public void clearLines()
    {
        landmark.GetComponent<RayCasting>().clearLines();
        visScore.text = "Visibility Score: Press button";
    }
    public void startStopSimulation()
    {
        if (simText.text == "New Simulation")
        {
            sun.GetComponent<SunSim>().startNewSimulation();
            shadowVariation.GetComponent<ShadowVariation>().clearMatrices();
            shadowVariation.GetComponent<ShadowVariation>().setSimulating(true);
            var startButton = GameObject.Find("SimBtn");
            startButton.GetComponentInChildren<Text>().text = "Stop Simulation";

        }
        else
        {
            var simulating = sun.GetComponent<SunSim>().StartStopSimulation();
            shadowVariation.GetComponent<ShadowVariation>().setSimulating(simulating);
            if (simulating) simText.text = "Stop Simulation";
            else simText.text = "Start Simulation";
        }
    }
    public void toggleEditMode()
    {
        var eMode = buildingManager.GetComponent<BuildingManager>().toggleEditMode();
        if (eMode) editMode.text = "Edit Mode: On";
        else editMode.text = "Edit Mode: Off";
    }

    public void deleteBuilding()
    {
        var succesful = buildingManager.GetComponent<BuildingManager>().deleteBuilding();
        // legg til en error message om return er false for da er ikke en bygning added
    }

    public void addBuilding()
    {
        var succesful = buildingManager.GetComponent<BuildingManager>().addBuilding();
        // legg til en error message om return er false for da er ikke en bygning added
    }

    public void makeLandmark()
    {
        buildingManager.GetComponent<BuildingManager>().changeLandmark();
    }

    public void computeSkyVisibility()
    {
        var skyVisibility = buildingManager.GetComponent<BuildingManager>().skyVisibility();
        skyVisScore.text = "Sky Visibility Score : " + skyVisibility;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    bool editMode = false;
    GameObject currentSelected;
    Color originalColor;
    GameObject currentLandmark;
    GameObject rayCaster;
    GameObject building;
    GameObject selSpot;
    public GameObject smallBuilding;
    public GameObject mediumBuilding;
    public GameObject largeBuilding;
    public Dropdown dropDown;
    public GameObject selectedSpot;

    // Start is called before the first frame update
    void Start()
    {
        currentLandmark = GameObject.Find("Landmark");
    }

    // Update is called once per frame
    void Update()
    {
        if (editMode)
        {
            selectBuilding();
        }
    }

    public bool toggleEditMode()
    {
        editMode = !editMode;
        if (!editMode && currentSelected != null)
        {
            currentSelected.GetComponent<MeshRenderer>().material.color = originalColor;
            currentSelected = null;
        }
        if (!editMode) deleteSpotter();
        return editMode;
    }

    public void selectBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > 246)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitController;
                var hit = Physics.Raycast(ray, out hitController);
                if (hit)
                {
                    var collided = hitController.collider.gameObject;
                    if (collided.name == "Building")
                    {
                        if (currentSelected != null)
                        {
                            currentSelected.GetComponent<MeshRenderer>().material.color = originalColor;
                        }

                        var color = collided.GetComponent<MeshRenderer>().material.color;
                        originalColor = color;
                        collided.GetComponent<MeshRenderer>().material.color = Color.red;
                        currentSelected = collided;
                    }
                    else
                    {
                        deleteSpotter();
                        var spotHolder = selectedSpot;
                        var addPoint = hitController.point;
                        var spotY = addPoint.y;
                        addPoint.y = spotY + 6;
                        var spot = Renderer.Instantiate(spotHolder);
                        spot.transform.position = addPoint;
                        spot.name = "Spotter";
                        selSpot = spot;
                    }
                }
            }
        }
    }

    void deleteSpotter()
    {
        if (selSpot != null) Renderer.DestroyImmediate(selSpot, true);
    }

    public bool deleteBuilding()
    {
        if (currentSelected != null)
        {
            Renderer.DestroyImmediate(currentSelected, true);
            return true;
        }
        else return false;
    }

    public bool changeLandmark()
    {
        if (currentSelected != null)
        {
            var temp = currentLandmark;
            currentLandmark.name = "Building";
            currentLandmark.tag = "Building";
            currentSelected.name = "Landmark";
            currentLandmark = currentSelected;
            return true;
        }
        else return false;
    }

    public bool addBuilding()
    {
        var value = dropDown.value;
        if (value == 0) building = smallBuilding;
        if (value == 1) building = mediumBuilding;
        if (value == 2) building = largeBuilding;
        var addPoint = selSpot.transform.position;
        addPoint.y = 0;
        addPoint.y = building.transform.localScale.y / 2;
        var newBuilding = Renderer.Instantiate(building);
        newBuilding.transform.position = addPoint;
        newBuilding.name = "Building";
        newBuilding.tag = "Building";
        deleteSpotter();
        return true;
    }

    public float skyVisibility()
    {
        if (selSpot != null)
        {
            var pos = selSpot.transform.position;
            var hits = 0;
            var numOfRays = 0;

            for (int i = 1; i < 10; i++)
            {
                var origin = new Vector3(pos.x, pos.y, pos.z);
                for (int j = 0; j < 72; j++)
                {
                    var deltaTheta = 10 * i;
                    var dir = selSpot.transform.TransformDirection(Mathf.Cos(j), deltaTheta * Mathf.PI / 180, Mathf.Sin(j));
                    RaycastHit hitController;
                    var hit = Physics.Raycast(origin, dir, out hitController, Mathf.Infinity);
                    Debug.DrawRay(origin, dir * 10000, Color.red, Mathf.Infinity);
                    if (hit)
                    {
                        hits++;
                    }
                    numOfRays++;
                }
            }
            var visibility = (float)((decimal)1 - ((decimal)hits / (decimal)numOfRays));
            return visibility;
        }

        return -0.1f;
    }
}

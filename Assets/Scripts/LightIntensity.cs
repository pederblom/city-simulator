using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    GameObject sun;
    public GameObject intensityBar;
    int heightOffset = 2;
    int maxHeight = 10;
    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 sunDirection = sun.transform.forward;
        float angle = sun.transform.rotation.eulerAngles.x;
        float intensity = 1 - (Mathf.Abs(angle - 90) / 80);
        float xScale = intensityBar.transform.localScale.x;
        float yScale = intensityBar.transform.localScale.y;
        float zScale = intensityBar.transform.localScale.z;
        var position = intensityBar.transform.position;
        var newYPos = heightOffset + (yScale / 2);

        RaycastHit hitController;
        var hit = Physics.Raycast(intensityBar.transform.position, -1 * sunDirection, out hitController, Mathf.Infinity);
        if (hit)
        {
            intensityBar.transform.position = new Vector3(position.x, heightOffset, position.z);
            intensityBar.transform.localScale = new Vector3(xScale, 0, xScale);
        }
        else
        {
            intensityBar.transform.position = new Vector3(position.x, newYPos, position.z);
            intensityBar.transform.localScale = new Vector3(xScale, maxHeight * intensity, zScale);
        }
    }
}

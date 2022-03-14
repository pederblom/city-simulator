using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RayCasting : MonoBehaviour
{
    public LineRenderer rayLine;
    Dictionary<int, LineRenderer> renderedLines = new Dictionary<int, LineRenderer>();
    int numOfLines = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float computeVisibility()
    {
        clearLines();
        var landmark = GameObject.Find("Landmark");
        HashSet<GameObject> buildings = new HashSet<GameObject>();
        var pos = landmark.transform.position;
        var scale = landmark.transform.localScale;
        for (int i = 1; i < 11; i++)
        {
            var delta = scale.y / 10;
            var origin = new Vector3(pos.x, i * delta, pos.z);
            for (int j = 0; j < 72; j++)
            {
                var dir = landmark.transform.TransformDirection(Mathf.Cos(j), 0, Mathf.Sin(j));
                RaycastHit hitController;
                var hit = Physics.Raycast(origin, dir, out hitController, Mathf.Infinity);
                if (hit)
                {
                    var collided = hitController.collider.gameObject;
                    if (collided.name == "Building")
                    {
                        buildings.Add(collided);
                        renderLine(origin, hitController.point);
                    }
                }
            }
        }
        var buildingObjects = GameObject.FindGameObjectsWithTag("Building");
        var visScore = (float)((decimal)(buildings.Count) / (decimal)(buildingObjects.Length));
        return visScore;
    }

    void renderLine(Vector3 origin, Vector3 hitPoint)
    {
        var line = rayLine;
        List<Vector3> pos = new List<Vector3>();
        pos.Add(hitPoint);
        pos.Add(origin);
        line.SetPositions(pos.ToArray());
        line.useWorldSpace = true;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.endWidth = (float)0.5;
        line.startWidth = (float)0.5;
        Renderer.Instantiate(line);
        renderedLines.Add(numOfLines, line);
        numOfLines++;
    }

    public void clearLines()
    {
        for (int i = 0; i < renderedLines.Count; i++)
        {
            var ray = GameObject.Find("Line(Clone)");
            Renderer.DestroyImmediate(ray, true);
        }
        renderedLines.Clear();
        numOfLines = 0;
    }
}

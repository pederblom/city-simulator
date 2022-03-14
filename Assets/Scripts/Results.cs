using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    Image resultImage;
    public Image image;
    public Text x;
    public Text title;
    public Text rangeHigh;
    public Text rangeLow;
    // Start is called before the first frame update
    void Start()
    {
        resultImage = GetComponent<Image>();
        showHideResults(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showHeatMap(int[,] matrix, int max, string rTitle)
    {
        clearMatrix();

        title.text = rTitle;

        var startingPoint = new Vector3(-70, -105, 0);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var img = image;

                var intensity = (float)((decimal)(matrix[i, j]) / (decimal)(max));
                img.color = Color.black + Color.red * intensity;
                var plane = Renderer.Instantiate(img);
                plane.transform.SetParent(resultImage.transform);
                plane.transform.localPosition = new Vector3(startingPoint.x + (20 * i), startingPoint.y + (20 * j), startingPoint.z);
            }
        }

        showHideResults(true);
    }
    public void showHideResults(bool b)
    {
        resultImage.enabled = b;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<Image>();
            if (child) child.enabled = b;
        }
        x.enabled = b;
        title.enabled = b;
        rangeHigh.enabled = b;
        rangeLow.enabled = b;

        //Clear matrix if it exists
        if (!b && GameObject.Find("MatrixTile(Clone)") != null) clearMatrix();
    }

    private void clearMatrix()
    {
        for (int i = 0; i < 100; i++)
        {
            var tile = GameObject.Find("MatrixTile(Clone)");
            Renderer.DestroyImmediate(tile, true);
        }
    }

}

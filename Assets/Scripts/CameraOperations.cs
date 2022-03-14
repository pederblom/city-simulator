using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperations : MonoBehaviour
{
    int zoomSpeed = 10;
    float panSpeed = 0.5f;
    int movespeed = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            var direction = transform.forward;
            var movement = Input.mouseScrollDelta.y * zoomSpeed;
            transform.position += new Vector3(movement * direction.x, movement * direction.y, movement * direction.z);
        }

        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + panSpeed, transform.rotation.eulerAngles.z);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - panSpeed, transform.rotation.eulerAngles.z);
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - panSpeed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + panSpeed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }

        if (Input.GetMouseButton(2) || Input.GetKey("m"))
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.position += -transform.right * movespeed * Time.deltaTime;
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.localPosition += transform.right * movespeed * Time.deltaTime;

            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.localPosition += -transform.up * movespeed * Time.deltaTime;
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                transform.localPosition += transform.up * movespeed * Time.deltaTime;
            }
        }
    }
}

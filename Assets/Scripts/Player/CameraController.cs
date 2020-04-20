using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public new Camera camera;

    public float speed = 10f;

    public bool scaleSpeedToScreenSize;

    public float scroll = 1.5f;

    // Update is called once per frame
    void Update()
    {
        //Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.position += movementVector * Time.unscaledDeltaTime * speed * (scaleSpeedToScreenSize ? camera.orthographicSize : 1f);

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
            camera.orthographicSize *= scroll;
        else if(Input.GetAxis("Mouse ScrollWheel") > 0)
            camera.orthographicSize /= scroll;
    }
}

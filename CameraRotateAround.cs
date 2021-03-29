using UnityEngine;
using System.Collections;

public class CameraRotateAround : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float sensitivity = 3; // чувствительность мышки
    public float MinlimitY = 80; // ограничение вращения по Y
    public float MaxlimitY = 80; // ограничение вращения по Y
    public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
    public float zoomMax = 10; // макс. увеличение
    public float zoomMin = 20; // мин. увеличение
    public float CamAngle = 40;
    public float yMin;
    public float yMax;
    private float OffsetY;
    private float X, Y;

    void Start()
    {
        //limit = Mathf.Abs(limit);
        //if (limit > 90) limit = 90;
        offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax));
        transform.position = target.position + offset;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) offset.z += zoom;                  
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) offset.z -= zoom;     
        offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

        //if (Input.GetAxisRaw("Vertical") > 0) OffsetY += 0.1f;
        //else if (Input.GetAxisRaw("Vertical") < 0) OffsetY -= 0.1f;

        Y += Input.GetAxis("Vertical") * sensitivity;
        Y = Mathf.Clamp(Y, MinlimitY, MaxlimitY);
        X = transform.localEulerAngles.y + -Input.GetAxis("Horizontal") * sensitivity;  
        transform.localEulerAngles = new Vector3(Y, X, 0);
        transform.position = transform.localRotation * offset + target.position;//new Vector3(target.position.x, OffsetY*sensitivity, target.position.z);
    }
}
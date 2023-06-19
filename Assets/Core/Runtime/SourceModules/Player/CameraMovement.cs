using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cameraHinge;

    [SerializeField]
    private Vector2 sensetivity;

    // Stored required properties.
    private float yAxis;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float x = Input.GetAxis("Mouse X") * sensetivity.x * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensetivity.y * Time.deltaTime;

        transform.Rotate(new Vector3(0, x, 0));

        yAxis -= y;
        yAxis = Mathf.Clamp(yAxis, -90, 90);

        cameraHinge.localRotation = Quaternion.Euler(yAxis, 0, 0);
    }
}

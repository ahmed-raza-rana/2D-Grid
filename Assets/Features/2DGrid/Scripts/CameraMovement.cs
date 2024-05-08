using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    Vector3 _touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public float panSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float minX = -10.0f;
    public float maxX = 10.0f;
    public float minZ = -10.0f;
    public float maxZ = 10.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * zoomSpeed);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = _touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction.y = 0; // Ignore changes in the Y axis
            PanCamera(direction);
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    void PanCamera(Vector3 direction)
    {
        Vector3 newPosition = Camera.main.transform.position + direction * panSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);
        Camera.main.transform.position = newPosition;
    }
}
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement
    public float lookSpeed = 2f;  // Speed of mouse look
    private float rotationX = 0f;

    void Update()
    {
        // Get input for movement
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Convert movement direction from local to world space while ignoring Y-axis
        Vector3 worldMove = transform.TransformDirection(moveDirection);
        worldMove.y = 0; // Prevent movement on the Y-axis

        // Apply movement
        transform.position += worldMove * moveSpeed * Time.deltaTime;

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY; // Rotate up/down
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Prevent flipping

        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y + mouseX, 0);
    }
}


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = 9.81f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private bool isSprinting = false;
    private float verticalVelocity = -2f;

    public bool inputLocked = false; // Hidden from Inspector

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (inputLocked) return;
        HandleMouseLook();
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        if (inputLocked) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection *= currentSpeed * Time.deltaTime;

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity * Time.deltaTime;
        controller.Move(moveDirection);
    }

    void HandleMouseLook()
    {
        if (inputLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (inputLocked) return;

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
}












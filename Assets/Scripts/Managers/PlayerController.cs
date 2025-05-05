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
    private float verticalVelocity = -2f; // Initialize gravity effect

    private PauseMenu pauseMenu;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pauseMenu = GetComponent<PauseMenu>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.Pause();
        }
        HandleMouseLook();
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection *= currentSpeed * Time.deltaTime;

        // Apply gravity properly
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Small downward force to keep grounded
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Apply gravity when not grounded
        }

        // Apply gravity to the vertical movement but separate it from horizontal movement
        moveDirection.y = verticalVelocity * Time.deltaTime;

        controller.Move(moveDirection);
    }



    void HandleMouseLook()
    {
        if (Time.timeScale == 1)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity); // Jump impulse
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
}












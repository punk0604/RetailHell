using UnityEngine;
using System.Collections;

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

    private float originalMoveSpeed;
    private float originalSprintSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        originalMoveSpeed = moveSpeed;
        originalSprintSpeed = sprintSpeed;
    }

    void Update()
    {
        if (inputLocked) return;
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleItemUse();
    }

    void HandleMovement()
    {
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    void HandleItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.Instance.energyDrinksOwned > 0)
        {
            GameManager.Instance.energyDrinksOwned--;
            StartCoroutine(ApplySpeedBoost(60f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.Instance.zenSodasOwned > 0)
        {
            GameManager.Instance.zenSodasOwned--;
            TaskStressManager stressManager = FindObjectOfType<TaskStressManager>();
            if (stressManager != null) stressManager.DisableStressForShift();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.Instance.snackBarsOwned > 0)
        {
            GameManager.Instance.snackBarsOwned--;
            TaskStressManager stressManager = FindObjectOfType<TaskStressManager>();
            if (stressManager != null) stressManager.IncreaseStressCap(5);
        }
    }

    public IEnumerator ApplySpeedBoost(float duration)
    {
        moveSpeed = originalMoveSpeed * 1.5f;
        sprintSpeed = originalSprintSpeed * 1.5f;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        sprintSpeed = originalSprintSpeed;
    }
}













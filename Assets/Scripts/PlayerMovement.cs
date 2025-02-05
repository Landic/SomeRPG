using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Движение игрока")]
    public float movementSpeed = 5.0f, mouseSensitivity = 2.0f, jumpForce = 2.0f, gravity = 9.81f, verticalRotation = 0f;
    public Transform playerCamera;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [Header("Приседание")]
    public float crouchHeight = 1.0f, standingHeight = 2.0f, crouchSpeed = 2.5f;

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera == null) Debug.LogError("Камера не привязана в PlayerMovement!");
    }
    void Update() {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float horizontalInput = Input.GetAxis("Horizontal"), verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement.y = 0;

        if (Input.GetKey(KeyCode.LeftControl)) {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 10);
            movementSpeed = crouchSpeed;
        } else {
            controller.height = Mathf.Lerp(controller.height, standingHeight, Time.deltaTime * 10);
            movementSpeed = 5.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) velocity.y = Mathf.Sqrt(jumpForce * 2 * gravity);
        controller.Move(movement * movementSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity, mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        if (playerCamera != null) {
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
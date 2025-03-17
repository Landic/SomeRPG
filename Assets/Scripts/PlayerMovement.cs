using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Движение игрока")]
    public float movementSpeed = 5f, mouseSensitivity = 2f, jumpForce = 1.75f, gravity = 9.81f, verticalRotation = 0f;
    public Transform playerCamera;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [Header("Приседание")]
    public float crouchHeight = 1f, standingHeight = 2f, crouchSpeed = 2.5f;
    [Header("Шаги")]
    public AudioSource leftFootSound, rightFootSound;
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f;
    private float nextFootstepTime;
    private bool isLeftFootstep = true;

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
        HandeControl(movement);

        if (isGrounded && controller.velocity.magnitude > 0.1f && Time.time >= nextFootstepTime) {
            FootstepsSound();
            nextFootstepTime = Time.time + footstepInterval;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void HandeControl(Vector3 movement) {
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
    }
    void FootstepsSound() {
        AudioClip footstepClip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        if (isLeftFootstep) leftFootSound.PlayOneShot(footstepClip);
        else rightFootSound.PlayOneShot(footstepClip);
        isLeftFootstep = !isLeftFootstep;
    }
}
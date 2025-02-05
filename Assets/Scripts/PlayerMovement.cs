using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Движение игрока и гравитация")]
    public float movementSpeed = 5.0f;
    private CharacterController controller;

    void Start() => controller = GetComponent<CharacterController>();
    void Update() {
        float horizontalInput = Input.GetAxis("Horizontal"), verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement.y = 0;
        controller.Move(movement * movementSpeed * Time.deltaTime);
    }
}
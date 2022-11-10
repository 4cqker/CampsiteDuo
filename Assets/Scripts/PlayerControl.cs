using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject cameraPoint;
    public Transform groundedPoint;
    public CharacterController controller;
    public LayerMask groundMask;

    public float moveSpeed = 12f;
    public float camSpeedVertical = 10f;
    public float camSpeedHorizontal = 10f;
    public float jumpHeight = 3f;

    private float xInput = 0f;
    private float zInput = 0f;
    private float xRotation = 0f;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Mouse Movement Set up
        float mouseX = Input.GetAxis("MouseHorz") * Time.deltaTime * camSpeedHorizontal;
        float mouseY = Input.GetAxis("MouseVert") * Time.deltaTime * camSpeedVertical;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        //WASD Movement Set up
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        Vector3 moveVector = transform.right * xInput + transform.forward * zInput;
        Vector3 moveVectorCleaned = moveVector.normalized * moveSpeed * Time.deltaTime;

        controller.Move(moveVectorCleaned);

        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Ground Checking
        isGrounded = Physics.CheckSphere(groundedPoint.position, 0.5f, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -1f;
        
    }
}

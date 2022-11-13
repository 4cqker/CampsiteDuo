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
    public float sprintSpeed = 16f;
    public int normalFOV = 60;
    public int sprintFOV = 75;
    public float lerpFOVTime = 0.2f;
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

        //if (Input.GetButtonDown("Sprint")) mainCamera.fieldOfView = Mathf.Lerp(normalFOV, sprintFOV, lerpFOVTime);

        if (Input.GetButton("Sprint"))
        {
            moveVector = moveVector.normalized * sprintSpeed * Time.deltaTime;
            mainCamera.fieldOfView = sprintFOV;
        }
        else
        {
            moveVector = moveVector.normalized * moveSpeed * Time.deltaTime;
            mainCamera.fieldOfView = normalFOV;
        }

        // Need to turn this into a co-routine to make it happen properly, would have to make a method 
        //if (Input.GetButtonUp("Sprint")) mainCamera.fieldOfView = Mathf.Lerp(sprintFOV, normalFOV, lerpFOVTime);

        controller.Move(moveVector);

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

    IEnumerator FOVChange (float time)
    {
        return null;
    }
}

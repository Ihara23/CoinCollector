using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    private Animation anim;
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;
    public float laneDistance = 4;

    public float jumpforce;
    public float Gravity = -20;
    private float initialYPosition;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        controller = GetComponent<CharacterController>();
        anim.Play("idle");

    }

    // Update is called once per frame
    void Update()
    {

        //Increase the speed
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        if (!PlayerManager.isGameStarted)
        {
            return; // Exit the Update method if the game has not started
        }

        direction.z = forwardSpeed;



        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            else if (!isJumping)
            {
                direction.y = -1; // Reset y-direction when grounded and not jumping
                anim.Play("Running");

            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        // Handle lane switching
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        // Calculate target position based on lane
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        controller.Move(direction * Time.deltaTime);

        // Preserve the initial y position only when grounded and not jumping
        if (controller.isGrounded && !isJumping)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = initialYPosition;
            transform.position = newPosition;
        }

        // Check if the character has landed after a jump
        if (controller.isGrounded && isJumping)
        {
            isJumping = false;
            direction.y = -1;
        }
    }

    private void FixedUpdate()
    {

        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        isJumping = true;
        direction.y = jumpforce;
        //anim.Play("Jump");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<SoundsManager>().PlaySound("GameOver");
        }
    }
}

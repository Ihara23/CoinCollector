using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1;
    public float laneDistance = 4;

    public float jumpforce;
    public float Gravity = -20;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        direction.z = forwardSpeed;


        if (controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
            /*if (SwipeManager.swipeUp)
            {
                Jump();
            }*/
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        // to determine to be in which lane
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction.y = -1;
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction.y = -1;
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        //Using Mouse
        /*if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }*/

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            direction.y = -1;
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            direction.y = -1;
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = targetPosition;
        //transform.position = Vector3.Lerp(transform.position,targetPosition,100*Time.deltaTime);
        //controller.center = controller.center;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpforce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}

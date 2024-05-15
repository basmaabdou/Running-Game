using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 4;//The distance between tow lanes

    public float jumpForce;
    public float Gravity = -20;

    public bool isGrounded;

    //wheel animation
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isSliding = false;

    void Start()
    {
        // Check if the CharacterController component is already attached
        controller = GetComponent<CharacterController>();

        // If it's not attached, add it
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        //to change speed (increase)
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;
       

        animator.SetBool("isGameStarted",true);
        
        direction.z = forwardSpeed;

        // isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        // animator.SetBool("isGrounded", isGrounded);

            //by keyword
        //   if (controller.isGrounded)
        //     {
        //         // direction.y = -2;
        //         if (Input.GetKeyDown(KeyCode.UpArrow))
        //         {
        //             Jump();
        //         }
        //     }else{
        //         direction.y += Gravity * Time.deltaTime;
        //     }


        //     //Gather the inputs on which lane we should be
        //     if (Input.GetKeyDown(KeyCode.RightArrow))
        //     {
        //         desiredLane++;
        //         if (desiredLane == 3)
        //         {
        //             desiredLane = 2;
        //         }
        //     }

        //     if (Input.GetKeyDown(KeyCode.LeftArrow))
        //     {
        //         desiredLane--;
        //         if (desiredLane == -1)
        //         {
        //             desiredLane = 0;
        //         }
        //     }




        // by mouse or mobile
        if (controller.isGrounded)
        {
            // direction.y = -1;
            if (SwipeManager.swipeUp)
            {
                Jump();
                animator.SetBool("isGrounded", isGrounded);
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
           animator.SetBool("isGrounded", !isGrounded);
        }

        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        //Gather the inputs on which lane we should be
        if (SwipeManager.swipeRight)
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
        }


        //Calculate where we should be in the future with new pos //if =1 then it in middle
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // transform.position = targetPosition;
        // transform.position = Vector3.Lerp(transform.position, targetPosition, 70 * Time.fixedDeltaTime);

        //not through on objects
        // controller.center= controller.center;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.magnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);


    }


    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        //forward move
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }


}

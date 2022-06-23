using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobscript : MonoBehaviour
{
    private Rigidbody2D BobRigidbody2D;

    [SerializeField]private float faceDirectionX;
    public float moveDirectionX;
    public LayerMask GroundLayers;
    private Transform BobGroundCheckLeft, BobGroundCheckRight, BobTopCheck;

    // serielize field makes the variable available in the unity editor

    [SerializeField]private float minBobWalkSpeedX = .25f;
    [SerializeField]private float BobWalkAccelerationX = .14f;
    [SerializeField]private float currentSpeedX;
    [SerializeField]private float maxBobWalkSpeedX = 5.5f;
    [SerializeField]private float BobReleaseAccelerationX = .05f;

    [SerializeField]private float BobSkidDeccelerationX = .2f;
    [SerializeField]private float BobSkidTurnaround = .5f;

    [SerializeField]private bool isChangingDirection;

    [SerializeField] private float BobJumpSpeedY;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool touchingCeiling;
    [SerializeField] private bool isJumping;

    [SerializeField] private float jumpTimeCounter;
    private float jumpTimeMultiplier;

    public float BobMaxJumpTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        BobRigidbody2D = GetComponent<Rigidbody2D>();
        BobGroundCheckLeft = transform.Find("LeftCheck");
        BobGroundCheckRight = transform.Find("RightCheck");
        BobTopCheck = transform.Find("TopCheck");
        //i dont completely understand this
    }

   

    // fixed udate can be called more or less then every frame
    void FixedUpdate()
    {

        

        if(faceDirectionX != 0) // if facing a direction ( with arrow keys )
        {

            if(currentSpeedX == 0) // if not moving
            {

                currentSpeedX = minBobWalkSpeedX; // move at minimum walk speed

            }else if(currentSpeedX < maxBobWalkSpeedX) // if moving slower than the max speed
            {
                if(isChangingDirection == false) // if not changing direction
                {
                    currentSpeedX = IncreaseWithinBound(currentSpeedX, BobWalkAccelerationX, maxBobWalkSpeedX); // increase until the max walk speed

                }
                

            }


        }else if( currentSpeedX > 0) // if Bob is moving
        {

            currentSpeedX = DecreaseWithinBound(currentSpeedX, BobReleaseAccelerationX, 0); // decrease speed to 0

        }



        
        //isChangingDirection = currentSpeedX > 0 && faceDirectionX * moveDirectionX < 0; // if moving and the direction facing times the direction moving is negative (facing in opposite way to moving)

        if (isChangingDirection) // if isChangingDirection is true
        {

            if (currentSpeedX > BobSkidTurnaround) // if faster than BobSkidTurnaround
            {
            }
                moveDirectionX = -faceDirectionX; // look the other way
                currentSpeedX = DecreaseWithinBound(currentSpeedX, BobSkidDeccelerationX, 0);

            

           

        }
        else // if changingDirecion is false
        {

            moveDirectionX = faceDirectionX; // set the movement direction to the facing direction

        }


        BobRigidbody2D.velocity = new Vector2(moveDirectionX * currentSpeedX, BobRigidbody2D.velocity.y); // move at current speed times direction to the right. If direction is negative bob will move left


        if (faceDirectionX > 0) // if facing right
        {
            
            transform.localScale = new Vector2(1, 1);
            // face right ^ ^

        }else if (faceDirectionX < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            // face left ^ ^
           
        }

    }


    // Update is called once per frame
    void Update()
    {

        jumpTimeMultiplier = -jumpTimeCounter + BobMaxJumpTime;

        faceDirectionX = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapPoint(BobGroundCheckLeft.position, GroundLayers) || Physics2D.OverlapPoint(BobGroundCheckRight.position, GroundLayers); // isGrounded is true if BobGroundCheckLeft or BobGroundCheckRight are touching the ground

        //touchingCeiling = Physics2D.OverlapPoint(BobTopCheck.position, GroundLayers);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) // if grounded and pressing space key down
        {

            isJumping = true; // set is jumping to true
            jumpTimeCounter = BobMaxJumpTime; // set time counter to max jump time
            BobRigidbody2D.velocity = Vector2.up * BobJumpSpeedY; // something to do with moving up. the higher BobjumpSpeedY, the higher the jump.

        }

        if (touchingCeiling == true && Input.GetKey(KeyCode.Space))
        {

            isJumping = false;

        }



        if (Input.GetKey(KeyCode.Space) && isJumping == true) // if holding down space bar
        {

            if (jumpTimeCounter > 0) // if there is time left in the jump time counter
            {

                BobRigidbody2D.velocity = Vector2.up * BobJumpSpeedY; // something to do with moving up. the higher BobjumpSpeedY, the higher the jump.

                jumpTimeCounter = DecreaseWithinBound(jumpTimeCounter, 1, 0); // decrease jump time counter
                jumpTimeCounter -= Time.deltaTime;
            }
            else // if there is no time left in the jump counter
            {

                isJumping = false; // set jumping to false

            }

        }


        if(Input.GetKeyUp(KeyCode.Space)) // if stop pressing space
        {

            isJumping = false; // set jumpin to false

        }


    }


    float IncreaseWithinBound(float val, float delta, float maxVal)
    {

        val += delta;

        if(val > maxVal)
        {

            val = maxVal;

        }

        return val;

    }


    float DecreaseWithinBound(float val, float delta, float minVal = 0)
    {

        val -= delta;
        if(val < minVal)
        {
            val = minVal;

        }

        return val;

    }


    


}

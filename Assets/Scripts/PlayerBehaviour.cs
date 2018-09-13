using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;

    [Tooltip("How fast the ball moves forward automatically")]
    [Range(0, 10)]
    public float rollSpeed = 5;

    public enum MobileHorizMovement
    {
        Accelerometer,
        ScreenTouch
    }

    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;

    [Header("Swipe Properties")]
    [Tooltip("How far will the player move upon swiping")]
    public float swipeMove = 2.0f;

    [Tooltip("How far (in pixel space) must the player swipe before we will execute the action")]
    public float minSwipeDistance = 2.0f;

    /// <summary>
    /// Stores the starting position of mobile touch events
    /// </summary>
    private Vector2 touchStart;

    /// <summary>
    /// A reference to the Rigidbody component
    /// </summary>
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access Rigidbody component
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update()
    {
        // If the game is paused, do nothing
        if (PauseScreenBehaviour.paused)
            return;

        // Movement in the X axis
        float horizontalSpeed = 0;

        // Check if we're running in the Unity Editor or a mobile build
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        // Check if we're moving to the side
        horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

        // If Mouse is held down (or screen is tapped on mobile)
        if (Input.GetMouseButton(0))
        {
            horizontalSpeed = CalculateMovement(Input.mousePosition);
        }

        // Check if we're running on an iOS or Android device
#elif UNITY_IOS || UNITY_ANDROID

        if (horizMovement == MobileHorizMovement.Accelerometer)
        {
            // Move player based on direction of Accelerometer
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }

        // Check if input has registered more than 0 touches
        if (Input.touchCount > 0)
        {
            // Store the first touch detected
            Touch touch = Input.touches[0];

            if (horizMovement == MobileHorizMovement.ScreenTouch)
            {
                horizontalSpeed = CalculateMovement(touch.position);
            }

            // Uncomment to use left & right movement
            //horizontalSpeed = CalculateMovement(myTouch.position);
            SwipeTeleport(touch);

            // Check to see if objects are touched
            TouchObjects(touch);
        }

#endif
        // Calculate movement force
        var movementForce = new Vector3(horizontalSpeed, 0, rollSpeed);

        // the amount of time since last frame
        movementForce *= (Time.deltaTime * 60);

        // Add force to the Rigidbody component
        rb.AddForce(movementForce);
    }

    /// <summary>
    /// Will figure out how to move the player horizontally
    /// </summary>
    /// <param name="pixelPos">The position the player has touched/clicked on</param>
    /// <returns>The direction to move in the X axis</returns>

    float CalculateMovement(Vector3 pixelPos)
    {
        // Converts mouse or touch location to Viewport Space (0 to 1 scale)
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);

        // Initializes xMove
        float xMove = 0;

        // If we press the right side of the screen
        if (worldPos.x > 0.5f)
        {
            xMove = 1;
        }
        // Otherwise we're on the left
        else
        {
            xMove = -1;
        }

        // Replace horizontalSpeed with our own value
        return xMove * dodgeSpeed;
    }

    /// <summary>
    /// Will teleport the player if swiped to the left or right
    /// </summary>
    /// <param name="touch">Current touch event</param>
    private void SwipeTeleport(Touch touch)
    {
        // Check if the touch just started
        if (touch.phase == TouchPhase.Began)
        {
            // If so, set touchStart
            touchStart = touch.position;
        }

        // If the touch has ended
        else if (touch.phase == TouchPhase.Ended)
        {
            // Get the position of the touch end
            Vector2 touchEnd = touch.position;

            // Calculate the difference between beginning & end on x axis
            float dx = touchEnd.x - touchStart.x;

            // If we're not moving far enough, don't teleport
            if (Mathf.Abs(dx) < minSwipeDistance)
            {
                return;
            }

            Vector3 moveDirection;

            // If moved negatively in x axis, move left
            if (dx < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                // otherwise, we're on the right
                moveDirection = Vector3.right;
            }

            RaycastHit hit;
           
            // Only move if we wouldn't hit something
            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                // Move the player
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }

    /// <summary>
    /// Will determine if we are touching a game object
    /// and if so, call events for it
    /// </summary>
    /// <param name="touch">Our touch event</param>
    private static void TouchObjects(Touch touch)
    {
        // Convert the position into a ray
        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        // Are we touching an object with a collider?
        if (Physics.Raycast(touchRay, out hit))
        {
            // Call the PlayerTouch function if it exists on a component attached to this object
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
}

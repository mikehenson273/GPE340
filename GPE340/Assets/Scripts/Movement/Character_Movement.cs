using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Movement : MonoBehaviour
{
    //variables
    public float moveSpeed; //how fast the player can move

    private Plane plane;
    private Ray rayPointer; //for tracking the mouse on screen
    private float sprintAmount; //a set amount for how much sprint the player has
    private float rayDistance; //how far from the camera the ray is
    private Animator anim; //player animator so that the character is animated... Also allows movement through root motion

    [SerializeField] private Slider staminaSlider; //Slider to show how much stamina the player has


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //sets up variable with this objects corresponding component

        //checks to make sure that the game object that this scrip is attached to is the player and if it is then add the players character movement
        //to the player controller object in the scene
        if (gameObject.tag == "Player")
        {
            //Debug.Log("Found Player Character"); //informs the player character was found
            //Looks for the move_controller in the scene using the specified tag and associates the pawn movement with character movement. 
            GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Movement_Controller>().pawn = gameObject.GetComponent<Character_Movement>();
            //Debug.Log("Added Character Movement to Player Controller"); //informs the controls were added
            plane = new Plane(Vector3.up, Vector3.zero);

            //searches for the UI Object of StaminaSlider and sets it equal to the slider here
            staminaSlider = GameObject.FindGameObjectWithTag("StaminaSlider").GetComponent<Slider>();
            sprintAmount = 100; //default sprint amount
            staminaSlider.value = sprintAmount; //sets the slider value to be the same as the sprint amount
        }
        //if the object that this is attached to is NOT the player then attach this objects character movement 
        //to the movement controller ON the object
        else if (gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<Movement_Controller>().pawn = gameObject.GetComponent<Character_Movement>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player") //if the script is on the player then enable rotations
        {
            LookAtMouse(); //function for rotating the player character to look at where the mouse is
        }
    }

    public void LookAtMouse()
    {
        rayPointer = Camera.main.ScreenPointToRay(Input.mousePosition); //creates a raycast to where the mouse is
        
        if (plane.Raycast(rayPointer, out rayDistance)) //if the mouse is on a plane/on screen
        {
            Vector3 targetLocation = rayPointer.GetPoint(rayDistance); //create a location based off where the mouse is
            Vector3 directionToTurn = targetLocation - transform.position; //calculates if mouse is in a different position than the character

            //create a variable using the horizontal and vertical positioning of the mouse by converting it to a float using the Tan y/x
            //and finally converts it to degrees
            float rotateTowards = Mathf.Atan2(directionToTurn.x, directionToTurn.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotateTowards, 0); //turns the character based off quartenion angles
        }
    }

    public void MovePlayerCharacter(Vector3 movementDirection)
    {
        //convert character direction from world space to local space
        movementDirection = transform.InverseTransformDirection(movementDirection);

        //Clamp (sets vector3 magnitude to <1)
        //Debug.Log(movementDirection.magnitude);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

        //multiply by speed -> we are now basing it off speed and not the direction magnitude
        movementDirection *= moveSpeed; //same as movementDirection = movementDirection * speed

        if (Input.GetKey(KeyCode.LeftShift) && sprintAmount > 0) //if the player has some sprint and is holding the left shift key
        {
            movementDirection *= 1.5f; //speeds up the movement further by 1.5 times
            //pass parameters to animator to determine which animation is playing
            anim.SetFloat("Right", movementDirection.x); //sets the float speed for animations
            anim.SetFloat("Forward", movementDirection.z); //sets the float speed for animations
            anim.speed = 1.5f; //sets up the speed of animations to be 1.5 times normal to allow player to move a bit faster
            SprintingTracker(); //tracks how much sprint is left and updates the stamina slider
        }

        //if the player isn't holding down left shift OR they are holding down left shift but the sprint amount
        //is less than or equal to 0
        else if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && sprintAmount <= 0))
        {
            //pass parameters to animator to determine which animation is playing
            anim.SetFloat("Right", movementDirection.x); //sets the float speed for animations
            anim.SetFloat("Forward", movementDirection.z); //sets the float speed for animations
            anim.speed = 1f; //resets the speed of the animator back down to normal speeds

            if (!Input.GetKey(KeyCode.LeftShift)) //only do this if the shift key isn't being pressed
            {
                sprintAmount += .05f; //increase the sprint counter/slider very slowly
                staminaSlider.value = sprintAmount; //adjust the slider to reflect the amount of sprint the player has
            }
        }
        
    }
    
    public void SprintingTracker()
    {
        //if neither axis is equal to zero and sprintAmount is greater than 0 do sprint calculations
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && sprintAmount > 0)
        {
            sprintAmount -= .25f; //decreases the amount of time the player can sprint
            staminaSlider.value = sprintAmount; //adjust the slider to reflect the amount of sprint the player has
        }
    }
}

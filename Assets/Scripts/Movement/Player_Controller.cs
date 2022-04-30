using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    //TODO: MOVE AMMO TO WEAPON CLASS IF POSSIBLE

    //variables
    [HideInInspector] public Pawn pawn; //holds whatever character is making use of this controller
    public GameObject weapon; //holds whatever weapon the character is using
    private int ammoCount; //holds an amount for the player to use with a weapon
    private int maxAmmo; //holds the max amount of ammo for the player

    public float sprintAmount; //a set amount for how much sprint the player has
    private float maxSprintAmount; //caps the amount of sprint
    private Slider staminaSlider; //Slider to show how much stamina the player has

    [HideInInspector] Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);

        //searches for the UI Object of StaminaSlider and sets it as the slider here
        staminaSlider = GameObject.FindGameObjectWithTag("StaminaSlider").GetComponent<Slider>();
        if (sprintAmount <= 0)
        {
            sprintAmount = 100; //default sprint amount if one isn't set
        }
        maxSprintAmount = sprintAmount; //sets the max amount of sprint so it can't increase more than the amount
        staminaSlider.value = sprintAmount; //sets the slider value to be the same as the sprint amount
        staminaSlider.maxValue = maxSprintAmount; //sets max slider value to designer chosen one
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if it has a pawn with an enabled Pawn script and if it does then move the character
        if (pawn != null && pawn.GetComponent<Pawn>().enabled == true)
        {
            MovingCharacter(); //moves the character
            LookAtMouse(); //function for rotating the player character to look at where the mouse is
            InstaKill(); //in case a glitch causing an out of bounds error happens
        }
        else if (pawn == null)
        {
            sprintAmount = maxSprintAmount; //if player dies and respawns set the sprint amount equal to the max amount
            staminaSlider.value = sprintAmount; //adjust slider accordingly
        }
        //if the script has been disabled then cease all movement
        else if (pawn.GetComponent<Pawn>().enabled == false)
        {
            pawn.MovePlayerCharacter(new Vector3(0, 0, 0));
        }
    }

    void MovingCharacter()
    {
        //pass controller positions to move function in player_movement
        //Debug.Log("Moving Player Character"); //informs that the player character is being moved
        pawn.MovePlayerCharacter(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        
        if (Input.GetKey(KeyCode.LeftShift) && sprintAmount > 0) //if the player has some sprint and is holding the left shift key
        {
            pawn.GetComponent<Pawn>().SetSprinting(true);
            SprintingTracker(); //tracks how much sprint is left and updates the stamina slider
        }

        //if the player isn't holding down left shift OR they are holding down left shift but the sprint amount
        //is less than or equal to 0
        else if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && sprintAmount <= 0))
        {
            //pass parameters to animator to determine which animation is playing
            pawn.GetComponent<Pawn>().SetSprinting(false);

            if (!Input.GetKey(KeyCode.LeftShift) && sprintAmount < maxSprintAmount) //only do this if the shift key isn't being pressed
                                                                                    //and if the sprint amount is less than the max amount
            {
                sprintAmount += .05f; //increase the sprint counter/slider very slowly
                staminaSlider.value = sprintAmount; //adjust the slider to reflect the amount of sprint the player has
            }
        }
        //if left click on mouse and has a weapon
        if (Input.GetKey(KeyCode.Mouse0) && weapon != null)
        {
            if (!weapon.GetComponent<Weapon>().FiredWeapon()) //if hasn't fired
            {
                pawn.anim.SetBool("FireBow", true);
                weapon.GetComponent<Weapon>().FireWeapon("Player");
            }
        }
        else if (!Input.GetKey(KeyCode.Mouse0) && weapon != null)
        {
            pawn.anim.SetBool("FireBow", false);
        }
    }

    public void LookAtMouse()
    {
        
        Ray rayPointer; //for tracking the mouse on screen
        float rayDistance; //how far from the camera the ray is

        rayPointer = Camera.main.ScreenPointToRay(Input.mousePosition); //creates a raycast to where the mouse is
        float rotateTowards;

        if (plane.Raycast(rayPointer, out rayDistance)) //if the mouse is on a plane/on screen
        {
            Vector3 targetLocation = rayPointer.GetPoint(rayDistance); //create a location based off where the mouse is
            Vector3 directionToTurn = targetLocation - pawn.transform.position; //calculates if mouse is in a different position than the character

            //create a variable using the horizontal and vertical positioning of the mouse by converting it to a float using the Tan y/x
            //and finally converts it to degrees
            if (pawn.transform.rotation.y >= 0 && pawn.transform.rotation.y <= 180)
            {
                //rotateTowards = (Mathf.Atan2(directionToTurn.x, directionToTurn.z) * Mathf.Rad2Deg) + 4.5f;
            }
            else if ((pawn.transform.rotation.y < 0 && pawn.transform.rotation.y >= -180) ||
                (pawn.transform.rotation.y <= 360 && pawn.transform.rotation.y > 180))
            {
                //rotateTowards = (Mathf.Atan2(directionToTurn.x, directionToTurn.z) * Mathf.Rad2Deg) - 4.5f;
            }
            rotateTowards = (Mathf.Atan2(directionToTurn.x, directionToTurn.z) * Mathf.Rad2Deg) + 4.5f;
            pawn.transform.rotation = Quaternion.Euler(0, rotateTowards, 0); //turns the character based off quartenion angles
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

    void InstaKill() //for killing the player in case a glitch happened to cause them to be out of bounds
    {
        if (Input.GetKeyDown(KeyCode.O)) //if user presses "O"
        {
            //grab max health and pass through to reduce health
            pawn.GetComponent<Health>().ReduceHealth(pawn.GetComponent<Health>().GetMaxHealth());
            //runs through and checks the player's health to initiate destruction
            pawn.GetComponent<Health>().HealthTracker();
        }
    }

    public int GetAmmoCount() //grabs how much ammo the player has
    {
        return ammoCount; //returns how much ammo there is
    }

    public void AddAmmo(int amountAdding) //adds an amount of ammo to the players stockpile
    {
        if (ammoCount + amountAdding < maxAmmo) //if the ammo count + added amount is less than max then add together
        {
            ammoCount += amountAdding; //adds the amount to the ammo count
        }
        else if (ammoCount + amountAdding >= maxAmmo) //if the ammo count + added amount is equal to or greater than just make count equal to max
        {
            ammoCount = maxAmmo; //maxes out ammo counter
        }
    }

    public void SetAmmo(int ammoAmount) //when picking up a weapon set the controllers max ammo and ammo count amounts
    {
        ammoCount = ammoAmount;
        maxAmmo = ammoAmount;
    }
    //pawn script applies for player and enemy both
    //2 controllers 1 for player 1 for enemy (controller controls inputs)
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    //variables
    public Character_Movement pawn; //holds whatever character is making use of this controller

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if attached to an object that is the player controller and if it is then move the player character
        if (gameObject.tag == "PlayerController")
        {
            //pass controller positions to move function in player_movement
            //Debug.Log("Moving Player Character"); //informs that the player character is being moved
            pawn.MovePlayerCharacter(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }
        //if not attached to a player controller than control the AI
        else if (gameObject.tag != "PlayerController")
        {
            //do AI movements here
        }
    }

    //should add health values here or in character movement so that I don't have to deal with them as much...
}

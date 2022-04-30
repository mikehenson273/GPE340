using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //holds singular instance

    public GameObject spawnArea; //holds an area for the player to spawn in, will rework into autopopulating list once I add a main menu for the game
    public GameObject playerCharacter; //holds the player character
    public GameObject tempPlayerCharacter; //holds a clone of the player instead of the player itself
    public GameObject playerControls; //holds the player controller so it doesn't have to be found every time

    public Camera_Movement cam;

    //mainly to tell if the player is unarmed and once the player gets a weapon, then enemies will spawn
    [HideInInspector] public bool enemiesCanSpawn;

    // Start is called before the first frame update
    void Start()
    {
        enemiesCanSpawn = false; //initial set up to prevent enemies from spawning outright

        //If an instance hasn't been created
        if (instance == null)
        {
            instance = this; //store current instance
            DontDestroyOnLoad(gameObject); //Don't delete if a new scene is loaded
        }

        else
        {
            Destroy(this.gameObject); //Destroy new instance
            Debug.Log("Error - A new game manager was detected and promptly deleted");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //add in a sceneChecker to ensure that the player and controller isn't being added/searched in/for during the main menu or game over scenes
        if (playerControls == null) //if there is no player controller then search for one
        {
            //if a player controller is found than hold it in the location in the manager
            playerControls = GameObject.FindGameObjectWithTag("PlayerController");
        }
        if (tempPlayerCharacter == null) //if there is no player
        {
            //set the weapon as null so the user doesn't respawn with one (might change to autospawn with one)
            playerControls.GetComponent<Player_Controller>().weapon = null;
            //sets the tempPlayerCharacter as the playerCharacter and spawns a clone as child of the player controller
            tempPlayerCharacter = Instantiate(playerCharacter, playerControls.transform);
            //spawns at location of the spawnArea
            tempPlayerCharacter.transform.position = spawnArea.transform.position;
            //playerControls.GetComponent<Player_Controller>().pawn = tempPlayerCharacter.GetComponent<Pawn>();
        }
    }
}

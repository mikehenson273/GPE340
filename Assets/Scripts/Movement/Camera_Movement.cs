using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private GameObject playerCharacter; //holds player gameobject for tracking character

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter == null) //upon initial startup tries to find player character
        {
            playerCharacter = GameObject.FindGameObjectWithTag("Player"); //searches for player
        }
        else if (playerCharacter != null) //if the player was found
        {
            transform.position = playerCharacter.transform.position + new Vector3(0f, 10.5f, -5f); //moves the camera to center the player in it
        }
    }

    public void SetCharacter() //first attempt, will try and enable later
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player"); //uses this function as the player character is respawning
    }
}

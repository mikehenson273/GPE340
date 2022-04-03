using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    private GameObject playerCharacter; //holds player gameobject for tracking character

    // Start is called before the first frame update
    void Start()
    {
        //upon initial startup tries to find player character (will rework as the class goes on so it can work with respawns)
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCharacter.transform.position + new Vector3(0f, 10.5f, -5f); //moves the camera to center the player in it
    }

    void SetCharacter(GameObject playerCharacterRespawn) //first attempt, will try and enable later
    {
        playerCharacter = playerCharacterRespawn; //uses the passed through GameObject as the player character for respawning
    }
}

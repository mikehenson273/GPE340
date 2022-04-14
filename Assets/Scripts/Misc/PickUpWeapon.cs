using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : PickUpBoosts
{
    [SerializeField] protected GameObject weapon; //so the designer can choose what weapon to place for pickup

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //if the colliding object is the player
        {
            if (GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon != null) //checks if there is a weapon in the controller
            {
                //if there is a weapon in the controller destroy it to add the newly picked up weapon
                Destroy(GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon);
            }
            //add in instantianting weapon to the player and making it a child of the player controller
            GameObject tempWeapon = Instantiate(weapon, other.gameObject.GetComponent<Pawn>().leftHand);
            //tempWeapon.transform.localPosition = collision.gameObject.GetComponent<Pawn>().leftHand.transform.localPosition;
            //tempWeapon.transform.localRotation = collision.gameObject.GetComponent<Pawn>().leftHand.transform.localRotation;
            //sets the player controller weapon object equal to the created weapon
            other.gameObject.GetComponentInParent<Player_Controller>().weapon = tempWeapon;
            //set the weapons position equal to the game object (purely for testing and will be changed)
            //tempWeapon.transform.position = collision.gameObject.transform.position;
            //sets the player character reference in the weapon to enable moving, will be changed
            tempWeapon.GetComponent<Weapon>().playerCharacter = other.gameObject.GetComponent<Pawn>();
            //collision.gameObject.GetComponent<Health>().AddToHealth(20); //add 20 points to health
            //destroys the pickup object
            Destroy(gameObject);
        }
    }
}

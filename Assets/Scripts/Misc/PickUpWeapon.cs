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
            //add in instantianting weapon to the player and making it a child of the pawn's left hand
            GameObject tempWeapon = Instantiate(weapon, other.gameObject.GetComponent<Pawn>().leftHand);

            //sets the player controller weapon object equal to the created weapon
            other.gameObject.GetComponentInParent<Player_Controller>().weapon = tempWeapon;
            
            //sets the player character reference in the weapon to enable moving, will be changed
            tempWeapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();
            
            //destroys the pickup object
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<Pawn>().GetPawnWeapon() != null)
            {
                other.gameObject.GetComponent<Pawn>().DestroyPawnWeapon();
            }

            //add in instantianting weapon to the player and making it a child of the pawn's left hand
            GameObject tempWeapon = Instantiate(weapon, other.gameObject.GetComponent<Pawn>().leftHand);

            //sets the pawn reference in the weapon to enable moving, will be changed
            tempWeapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();

            //destroys the pickup object
            Destroy(gameObject);
        }
    }
}

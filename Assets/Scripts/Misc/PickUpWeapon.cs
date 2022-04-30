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
            GameObject tempWeapon = Instantiate(weapon, other.gameObject.transform.parent);

            //sets the player controller weapon object equal to the created weapon
            other.gameObject.GetComponentInParent<Player_Controller>().weapon = tempWeapon;
            other.gameObject.GetComponentInParent<Player_Controller>().weapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();
            //sets the player character reference in the weapon to enable moving, will be changed
            //tempWeapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();

            GameManager.instance.enemiesCanSpawn = true; //allows the enemies to spawn once the player picks up a weapon

            //destroys the pickup object
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<Pawn>().GetPawnWeapon() == null)
        {
            //Debug.Log("Enemy Picked up weapon");
            //add in instantianting weapon to the enemy and making it a child of the enemy spawn location
            GameObject tempWeapon = Instantiate(weapon, other.gameObject.GetComponent<AI_Controller>().parentLocation);

            other.gameObject.GetComponent<AI_Controller>().weapon = tempWeapon; //sets the AI controller weapon equal to this object
            //sets the controllers weapon pawn character as the enemy that picked it up
            other.gameObject.GetComponent<AI_Controller>().weapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();
            tempWeapon.GetComponent<Weapon>().setAIDamageAndFireRate(); //if the enemy picked it up then adjust to be more fair to the player
            //Debug.Log("Pawn set on Weapon");
            //sets the pawn reference in the weapon to enable moving, will be changed
            //tempWeapon.GetComponent<Weapon>().pawnCharacter = other.gameObject.GetComponent<Pawn>();

            //destroys the pickup object
            Destroy(gameObject);
        }
    }
}

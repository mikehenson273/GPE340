using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damageOutput; //how much damage the player can do
    [SerializeField] protected float fireRate; //how often the user can fire the weapon
    protected bool hasFired; //checks to see if the character has fired yet

    public Pawn pawnCharacter; //holds the player object
    public GameObject weaponAmmo; //holds the weapon's ammo type

    public Transform leftHandGrip, //holds for the player's left hand to grip
                     rightHandGrip, //holds for the player's right hand to grip
                     firingPoint; //holds for where the ammo is supposed to fire from

    protected bool triedToFire; //error check mainly so AI doesn't immediately fire a weapon upon pickup
    
    protected void Awake()
    {
        triedToFire = false; //initial value set up mainly for the AI to ensure it doesn't immediately fire upon picking up the weapon
        hasFired = false; //ensures that when the weapon is picked up it can fire
        if (fireRate <= 0) //if the fire rate is 0
        {
            fireRate = 1.5f; //make it a default of 1.5 seconds
        }
        if (damageOutput <= 0) //if designer added in no damage make a default
        {
            damageOutput = 25;
        }
    }

    public virtual void FireWeapon(string layerName) //public function for over riding from a child class
    {
        Debug.Log("You have fired the weapon"); //for testing
    }

    public virtual bool FiredWeapon() //checks to see if weapon was fired
    {
        return hasFired; //returns the check
    }

    public float GetDamage() //returning how much damage can be done
    {
        return damageOutput;
    }

    public void setAIDamageAndFireRate() //if the pawn that equips a weapon is an AI reduce the damage and lower the rate of fire
    {
        damageOutput = Mathf.RoundToInt(damageOutput / 2);
        fireRate = fireRate * 2;
    }
}

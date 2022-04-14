using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damageOutput; //how much damage the player can do
    [SerializeField] protected float fireRate; //how often the user can fire the weapon
    protected bool hasFired; //checks to see if the character has fired yet

    public Pawn playerCharacter; //holds the player object
    public GameObject weaponAmmo; //holds the weapon's ammo type

    public Transform leftHandGrip,
                     rightHandGrip,
                     firingPoint;


    protected void Awake()
    {
        playerCharacter = GameManager.instance.tempPlayerCharacter.GetComponent<Pawn>(); //sets the inherited game object equal to the player character
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

    protected void Update()
    {
        gameObject.transform.SetParent(playerCharacter.leftHand);
    }

    public virtual void FireWeapon(string layerName)
    {
        Debug.Log("You have fired the weapon"); //for testing
    }

    public virtual bool FiredGun()
    {
        return hasFired;
    }

    public int GetDamage()
    {
        return damageOutput;
    }
}

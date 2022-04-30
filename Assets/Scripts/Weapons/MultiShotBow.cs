using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotBow : Weapon
{
    private Plane plane;
    private Ray rayPointer; //for tracking the mouse on screen
    private float rayDistance; //how far from the camera the ray is

    [HideInInspector] public Vector3 debugFireForward; //draws debug ray from firing point of bow

    private void Start()
    {
        gameObject.transform.SetParent(pawnCharacter.transform.parent); //sets the parent of the bow to as the parent object of the pawn
        //gameObject.transform.position = pawnCharacter.leftHand.transform.position;
    }

    private void Update()
    {
        //debugFireForward = firingPoint.transform.TransformDirection(Vector3.right) * 100;
        //Debug.DrawRay(firingPoint.transform.position, debugFireForward, Color.red);

        //TurnFiringPoint();
        if (pawnCharacter != null)
        {
            //positions the bow where the left hand is
            gameObject.transform.position = pawnCharacter.leftHand.transform.position;
            //rotate the bow in accordance to the pawn character so it's always in front or to the side
            gameObject.transform.rotation = pawnCharacter.transform.rotation * Quaternion.Euler(0, -90, 0);
        }
    }

    public override void FireWeapon(string layerName) //overrides the FireWeapon function inside the weapon class
    {
        //Debug.Log(pawnCharacter.name + ": fired arrow");
        //Debug.Log("Bow was fired");
        //error check for AIs so when they fire the first time that it doesn't spawn the arrows at the AIs spawn location
        if ((triedToFire && pawnCharacter.GetComponent<AI_Controller>()) || 
            pawnCharacter == GameManager.instance.tempPlayerCharacter.GetComponent<Pawn>())
        {
            SpawnArrows(layerName); //spawns in arrows with the same layer as the entity that fired them
        }


        StartCoroutine(FiringTimer()); //starts the timer
        //Debug.Log("Fired a multishot bow"); //testing will replace with instantiating
        StopCoroutine(FiringTimer()); //stops the timer
    }

    void SpawnArrows(string layerName)
    {
        //spawns temporary arrow based off bow's firingPoint position
        //GameObject tempAmmo1 = Instantiate(weaponAmmo, firingPoint.position, Quaternion.Euler(0, 0, 0));
        //spawns an arrow at the firingPoint position and rotation
        GameObject tempAmmo1 = Instantiate(weaponAmmo, firingPoint.position,
                               Quaternion.Normalize(pawnCharacter.GetComponent<Transform>().transform.rotation)
                               * Quaternion.Euler(0, 0, 0));

        Arrow tempArrowVars = tempAmmo1.GetComponent<Arrow>(); 
        tempArrowVars.damageDealt = damageOutput; //changes arrow damage to the damage given to the weapon
        tempArrowVars.pawnCharacter = pawnCharacter; //makes the arrows pawn character the same as the weapon's pawn character

        //spawns temporary arrow based off bow's firingPoint position and adjusts along z axis to spread out
        //GameObject tempAmmo2 = Instantiate(weaponAmmo, firingPoint.position + (firingPoint.forward * .5f), Quaternion.Euler(0, 0, 0));

        //spawns an arrow at the firingPoint position and rotation with an offset on the rotation so it heads in a different direction
        GameObject tempAmmo2 = Instantiate(weaponAmmo, firingPoint.position,
                               Quaternion.Normalize(pawnCharacter.GetComponent<Transform>().transform.rotation)
                               * Quaternion.Euler(0, -15, 0)); 

        tempArrowVars = tempAmmo2.GetComponent<Arrow>();
        tempArrowVars.damageDealt = damageOutput; //changes arrow damage to the damage given to the weapon
        tempArrowVars.pawnCharacter = pawnCharacter; //makes the arrows pawn character the same as the weapon's pawn character

        //spawns temporary arrow based off bow's firingPoint position and adjusts along z axis to spread out
        //GameObject tempAmmo3 = Instantiate(weaponAmmo, firingPoint.position + (firingPoint.forward * -.5f), Quaternion.Euler(0, 0, 0));

        //spawns an arrow at the firingPoint position and rotation with an offset on the rotation so it heads in a different direction
        GameObject tempAmmo3 = Instantiate(weaponAmmo, firingPoint.position,
                               Quaternion.Normalize(pawnCharacter.GetComponent<Transform>().transform.rotation)
                               * Quaternion.Euler(0, 15, 0));

        tempArrowVars = tempAmmo3.GetComponent<Arrow>();
        tempArrowVars.damageDealt = damageOutput; //changes arrow damage to the damage given to the weapon
        tempArrowVars.pawnCharacter = pawnCharacter; //makes the arrows pawn character the same as the weapon's pawn character

        //sets all 3 arrows to the same layer as whatever entity fired them
        tempAmmo1.layer = LayerMask.NameToLayer(layerName);
        tempAmmo2.layer = LayerMask.NameToLayer(layerName);
        tempAmmo3.layer = LayerMask.NameToLayer(layerName);
    }
    
    IEnumerator FiringTimer()
    {
        hasFired = true; //sets as true to ensure the update function doesn't repeatedly start it
        //waits for the amount of time determined by the designer
        yield return new WaitForSeconds(fireRate); //timer for designer time for how often the user can fire
        
        hasFired = false; //sets as false until the item is destroyed again

        if (!triedToFire)
        {
            triedToFire = true; //sets it only once
        }
    }

    private void TurnFiringPoint()
    {
        rayPointer = Camera.main.ScreenPointToRay(Input.mousePosition); //creates a raycast to where the mouse is

        if (plane.Raycast(rayPointer, out rayDistance)) //if the mouse is on a plane/on screen
        {
            Vector3 targetLocation = rayPointer.GetPoint(rayDistance); //create a location based off where the mouse is
            Vector3 directionToTurn = targetLocation - firingPoint.transform.position; //calculates if mouse is in a different position than the character

            //create a variable using the horizontal and vertical positioning of the mouse by converting it to a float using the Tan y/x
            //and finally converts it to degrees
            float rotateTowards = Mathf.Atan2(directionToTurn.x, directionToTurn.z) * Mathf.Rad2Deg;
            //firingPoint.transform.rotation = Quaternion.Euler(0, rotateTowards, 0); //turns the character based off quartenion angles
        }
    }
}

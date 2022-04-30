using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotBow : Weapon
{
    private Plane plane;
    private Ray rayPointer; //for tracking the mouse on screen
    private float rayDistance; //how far from the camera the ray is

    public Vector3 debugFireForward; //for drawing a debug ray directly from the firing point

    private void Start()
    {
        //gameObject.transform.SetParent(pawnCharacter.leftHand); //sets the parent of the bow to the left hand
        gameObject.transform.SetParent(pawnCharacter.transform.parent); //sets the parent of the bow to as the player controller
        //gameObject.transform.position = pawnCharacter.leftHand.transform.position;
    }

    private void Update()
    {
        //debugFireForward = firingPoint.transform.TransformDirection(Vector3.right) * 100;
        //Debug.DrawRay(firingPoint.transform.position, debugFireForward, Color.red);

        //TurnFiringPoint();
        if (pawnCharacter != null) //if there is a pawn character
        {
            //positions the bow where the left hand is
            gameObject.transform.position = pawnCharacter.leftHand.transform.position;
            //rotate the bow in accordance to the pawn character so it's always in front or to the side
            gameObject.transform.rotation = pawnCharacter.transform.rotation * Quaternion.Euler(0, -90, 0);
        }
    }

    public override void FireWeapon(string layerName) //overrides the FireWeapon function inside the weapon class
    {
        //Debug.Log("Bow was fired");
        //spawns arrow based off bow's firingPoint position
        //GameObject tempAmmo = Instantiate(weaponAmmo, firingPoint.position, Quaternion.Euler(0, 0, 0));

        //error check for AIs so when they fire the first time that it doesn't spawn the arrows at the AIs spawn location
        if ((triedToFire && pawnCharacter.GetComponent<AI_Controller>()) ||
            pawnCharacter == GameManager.instance.tempPlayerCharacter.GetComponent<Pawn>())
        {
            //spawns an arrow at the firingPoint position and rotation
            GameObject tempAmmo = Instantiate(weaponAmmo, firingPoint.position, firingPoint.GetComponent<Transform>().rotation);
            tempAmmo.layer = LayerMask.NameToLayer(layerName); //sets arrow layer as the same as whatever fired it
            //creates new forward vector based off firing points rotation
            tempAmmo.transform.forward = new Vector3(firingPoint.GetComponent<Transform>().transform.right.x,
                                                     firingPoint.GetComponent<Transform>().transform.right.y,
                                                     firingPoint.GetComponent<Transform>().transform.right.z);
            Arrow tempArrowVars = tempAmmo.GetComponent<Arrow>();
            tempArrowVars.damageDealt = damageOutput; //changes arrow damage to the damage given to the weapon
            tempArrowVars.pawnCharacter = pawnCharacter; //makes the arrows pawn character the same as the weapon's pawn character
        }
        StartCoroutine(FiringTimer()); //starts the timer
        //Debug.Log("Fired a singleshot bow"); //testing will replace with instantiating
        StopCoroutine(FiringTimer()); //stops the timer
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

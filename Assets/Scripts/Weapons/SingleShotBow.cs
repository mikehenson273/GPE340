using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotBow : Weapon
{
    private void Start()
    {
        gameObject.transform.SetParent(pawnCharacter.leftHand); //sets the parent of the bow to the left hand
    }

    public override void FireWeapon(string layerName) //overrides the FireWeapon function inside the weapon class
    {
        //spawns arrow based off bow's firingPoint position
        GameObject tempAmmo = Instantiate(weaponAmmo, firingPoint.position, Quaternion.Euler(0, 0, 0));
        tempAmmo.layer = LayerMask.NameToLayer(layerName); //sets arrow layer as the same as whatever fired it

        tempAmmo.transform.forward = new Vector3(pawnCharacter.GetComponent<Transform>().transform.forward.x,
                                                 pawnCharacter.GetComponent<Transform>().transform.forward.y,
                                                 pawnCharacter.GetComponent<Transform>().transform.forward.z);

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
    }
}

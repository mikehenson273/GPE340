using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotBow : Weapon
{
    public override void FireWeapon(string layerName) //overrides the FireWeapon function inside the weapon class
    {
        GameObject tempAmmo = Instantiate(weaponAmmo, firingPoint.position, Quaternion.Euler(0, 0, 0));
        tempAmmo.layer = LayerMask.NameToLayer(layerName);

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

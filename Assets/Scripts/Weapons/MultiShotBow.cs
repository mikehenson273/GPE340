using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotBow : Weapon
{
    public override void FireWeapon(string layerName) //overrides the FireWeapon function inside the weapon class
    {
        GameObject tempAmmo1 = Instantiate(weaponAmmo, firingPoint.position, Quaternion.Euler(0, 0, 0));

        GameObject tempAmmo2 = Instantiate(weaponAmmo, firingPoint.position + (firingPoint.forward * .5f), Quaternion.Euler(0, 0, 0));

        GameObject tempAmmo3 = Instantiate(weaponAmmo, firingPoint.position + (firingPoint.forward * -.5f), Quaternion.Euler(0, 0, 0));

        tempAmmo1.layer = LayerMask.NameToLayer(layerName);
        tempAmmo2.layer = LayerMask.NameToLayer(layerName);
        tempAmmo3.layer = LayerMask.NameToLayer(layerName);

        StartCoroutine(FiringTimer()); //starts the timer
        //Debug.Log("Fired a multishot bow"); //testing will replace with instantiating
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

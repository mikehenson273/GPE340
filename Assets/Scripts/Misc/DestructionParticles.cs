using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionParticles : MonoBehaviour
{
    //public GameObject particleEffect; //for when I add in particles for death/destruction

    public void Destroyed()
    {
        //GameObject effectClone = Instantiate(particleEffect, gameObject.transform.position, gameObject.transform.rotation);
        //Debug.Log(gameObject + " has been destroyed");
        //Destroy(effectClone, 1.5f);
    }

    public void AddScore()
    {

    }
}

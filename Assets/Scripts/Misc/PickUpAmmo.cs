using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    [SerializeField] private int addAmmo; //designer inputs how much health to heal

    // Start is called before the first frame update
    void Start()
    {
        if (addAmmo <= 0) //if the designer didn't add a health amount add one
        {
            addAmmo = 25;
        }
    }

    private void OnTriggerEnter(Collider other) //upon collision
    {
        if (other.gameObject.CompareTag("Player")) //if the colliding object is the player
        {
            other.gameObject.GetComponentInParent<Player_Controller>().AddAmmo(addAmmo); //add designer amount of points to health
            Destroy(gameObject); //destroys this object
        }
    }
}

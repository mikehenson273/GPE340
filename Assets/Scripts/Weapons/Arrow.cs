using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Weapon
{
    [SerializeField] private float arrowSpeed; //how fast the arrow travels
    private Rigidbody arrowBody; //holds the body of the arrow so it can interact
    private bool isDestroyed; //checks to see if the arrow has existed longer than it should

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false; //set as false so it doesn't disappear on spawn

        //sets the player controls as the parent if the arrow was shot by the player
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameObject.transform.parent = pawnCharacter.GetComponentInParent<Player_Controller>().transform;
        }

        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            gameObject.transform.parent = pawnCharacter.GetComponentInParent<ObjectSpawner>().transform;
        }
        
        if (arrowSpeed <= 0) //if there isn't a speed set then set a speed automatically
        {
            arrowSpeed = 10;
        }
        arrowBody = gameObject.GetComponent<Rigidbody>(); //sets the arrow body as the gameobjects rigidbody

        StartCoroutine(ExistTimer()); //starts how long the object can exist
    }

    // used to add own physics to arrow instead of bow physics
    void Update()
    {
        if (arrowBody != null) //if there is a rigidbody
        {
            //keep accelerating (will change to a more appropriate method of movement)
            //arrowBody.AddRelativeForce(0, 0, arrowSpeed * Time.deltaTime, ForceMode.Impulse);
            arrowBody.velocity = transform.TransformDirection(Vector3.forward * arrowSpeed);
        }

        if (isDestroyed == true) //once timer is up
        {
            Destroy(gameObject); //destroy arrow
        }
    }

    //upon colliding with an object
    private void OnCollisionEnter(Collision collision)
    {
        if (arrowBody != null) //if the rigidbody is not null
        {
            gameObject.GetComponent<Collider>().enabled = false; //disable collider to prevent additional collisions
            gameObject.transform.parent = collision.gameObject.transform; //set collided game object as parent of arrow
            Destroy(gameObject.GetComponent<Rigidbody>()); //destroy the rigidbody
        }
    }

    IEnumerator ExistTimer()
    {
        //waits for the amount of time determined by the designer
        yield return new WaitForSeconds(5); //timer for designer time for how often the user can fire

        isDestroyed = true; //sets as false until the item is destroyed again
    }
}

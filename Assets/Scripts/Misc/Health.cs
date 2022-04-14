using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private bool isDestroyed;

    private int maxHealth; //creates a private variable for the max amount of health the object is granted
    [SerializeField] private int objectHealth; //allows designers to set a max health

    [SerializeField] private GameObject canvas; //for holding a new canvas object tied to other objects
    private Slider healthSlider; //for tracking how much health an object or player has.

    public UnityEvent onDeath; //when object is destroyed
    private bool invokedDeath; //for error checking to ensure that it only became invoked once

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false; //sets default value to false
        invokedDeath = false; //sets default value to false

        if (gameObject.CompareTag("Player")) //if the script is attached to the player character
        {
            //searches for the component for Health
            healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
            if (objectHealth <= 0)
            {
                objectHealth = 100; //default health amount if none set
            }
        }

        else if (!gameObject.CompareTag("Player"))
        {
            if ((healthSlider == null || canvas == null))
            {
                //creates a clone of the canvas prefab and sets it as a child of the game objects parent
                //essentially makes the canvas a sibling of the game object under the spawner so it's easier
                //to track in the heirarchy
                canvas = Instantiate(canvas, gameObject.transform.parent);
                //rotates the health bar along the x axis making it easier to see
                canvas.transform.rotation = Quaternion.Euler(60, 0, 0);

                //searches the object for a Health Slider and sets it as the slider here
                healthSlider = canvas.GetComponentInChildren<Slider>();
            }

            if (objectHealth <= 0)
            {
                objectHealth = 100; //default health amount if none set
            }
        }

        maxHealth = objectHealth; //sets max health based off what the object health is
        healthSlider.value = objectHealth; //sets health slider to the amount
    }

    // Update is called once per frame
    void Update()
    {
        HealthTracker(); //for temporary testing

        if (!gameObject.CompareTag("Player") && canvas != null) //if the object isn't the player character
        {
            //set the health bar to track the game objects position if it's not the player and adjust it along the y axis to be above the object
            canvas.transform.position = (gameObject.transform.position + new Vector3(0, 1.29f, 0));
        }
    }

    public void HealthTracker()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameObject.CompareTag("Player")) //for testing right now
        {
            ReduceHealth(25); //decreases when a positive amount is put in
        }

        if (Input.GetKeyDown(KeyCode.J) && !gameObject.CompareTag("Player")) //for testing right now
        {
            ReduceHealth(25); //decreases when a positive amount is put in
        }

        if (objectHealth <= 0) //if the object has 0 or less health
        {
            //only runs through this if it hasn't run through the coroutine or used the event
            if (!isDestroyed && !invokedDeath)
            {
                if (!gameObject.CompareTag("Player")) //if the attached object isn't the player
                {
                    Destroy(canvas); //destroy it's health bar
                    gameObject.GetComponent<MeshRenderer>().enabled = false; //makes object invisible
                    foreach (Transform child in transform) //goes through each child of the game object
                    {
                        Destroy(child.gameObject); //destroys the children of gameobject
                    }
                }
                else if (gameObject.CompareTag("Player"))
                {
                    gameObject.GetComponent<Pawn>().enabled = false; //disables pawn movement script
                    foreach (Transform child in transform) //goes through each child of the player
                    {
                        Destroy(child.gameObject); //destroys the children of player on the chance that their mesh is located there
                    }
                }
                gameObject.GetComponent<Collider>().enabled = false; //disables colliders of the attached object
                Destroy(gameObject.GetComponent<Rigidbody>()); //destroys the rigidbody of the attached object

                onDeath.Invoke(); //do all functions after destruction
                invokedDeath = true; //ensures that this isn't run through again once it loops through

                StartCoroutine(DestroyingObject()); //starts a timer and keep hold here until timer is done
                StopCoroutine(DestroyingObject()); //once timer is done stop the coroutine
            }

            if (isDestroyed) //once timer is finished and triggers the boolean
            {
                if (gameObject.CompareTag("Player")) //if the object with health <= 0 is the player
                {
                    GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon = null;
                    GameManager.instance.tempPlayerCharacter = null; //set the tempPlayerCharacter in the game manager to null so it can respawn                    
                }
                Destroy(gameObject); //destroys whatever object this script is attached to
            }
        }
    }

    public void ReduceHealth(int amountToReduce) //passes in a value to reduce health value must be positive
    {
        objectHealth -= amountToReduce; //reduces health by the amount passed from the damaging object
        healthSlider.value = objectHealth; //set health slider value as the objects health
    }

    public void AddToHealth(int amountToAdd) //passes in amount to add to health via picking up health
    {
        objectHealth += amountToAdd; //adds an amount to the health from the item pickup

        if (objectHealth > maxHealth) //ensures that the player can't have more health than the max health
        {
            objectHealth = maxHealth; //if the health IS greater than the max health than set it equal to max health
        }
        healthSlider.value = objectHealth; //set health slider value as the objects health
    }

    public int GetMaxHealth() //for in case of out of bounds glitches so player can respawn
    {
        return maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy")) //if the colliding object is the player, will change to projectile
        {
            ReduceHealth(20); //reduce health by 20
            HealthTracker(); //placed here so it only checks if a collision happened with the player or projectile
        }

        else if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player")) //if the colliding object is the player, will change to projectile
        {
            ReduceHealth(15); //reduce health by 15
            HealthTracker(); //placed here so it only checks if a collision happened with the player or projectile
        }

        //if the ammo used collides with player or enemy then do damage based on the damage amount from the ammo
        else if (collision.gameObject.CompareTag("Ammo") && (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy")))
        {
            ReduceHealth(collision.gameObject.GetComponent<Weapon>().GetDamage()); //reduce health by ammo damage
        }
    }

    IEnumerator DestroyingObject()
    {
        //play sound and effect here
        yield return new WaitForSeconds(3); //waits 3 seconds, change to wait for sound or special fx to finish playing
        isDestroyed = true; //sets as true so Health Tracker can finish destroying objects
    }
}

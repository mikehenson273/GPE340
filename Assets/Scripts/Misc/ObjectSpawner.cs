using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float lowestWait, highestWait; //allows designers to set their own wait random wait time
    private bool waitingForRespawn; //private to keep it from being changed externally
    public GameObject itemSpawnLocation; //temporary, will change to spawn on the navmesh eventually
    public GameObject prefabToSpawn; //holds original copy
    [SerializeField] private GameObject tempPrefab; //holds instantiated copy for checking to see if it exists and if it doesn't spawn a new one

    // Start is called before the first frame update
    void Start()
    {
        waitingForRespawn = false; //sets default value to false


        if (lowestWait == 0) //if lowest allowed wait time wasn't set
        {
            lowestWait = 3; //set to 3
        }
        if (highestWait == 0) //if highest allowed wait time wasn't set
        {
            highestWait = 7; //set to 7
        }

        if (!prefabToSpawn.CompareTag("Enemy"))
        {
            SpawnNewItem(); //spawns clone of object
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.tempPlayerCharacter != null)
        {
            if (!waitingForRespawn) //checks if the object is waiting to respawn
            {
                if (tempPrefab == null) //if the object is gone from temp object
                {
                    if (prefabToSpawn != prefabToSpawn.GetComponent<PickUpWeapon>())
                    {
                        //if all other checks pass, check if the object to spawn is the same weapon that the player is holding
                        if (GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon != null)
                        {
                            StartCoroutine(WaitToRespawn()); //in case that the object is empty and it's not waiting to respawn then start the coroutine
                            StopCoroutine(WaitToRespawn()); //once coroutine is done stop it
                        }

                        else if (prefabToSpawn == prefabToSpawn.GetComponent<PickUpHealth>())
                        {
                            StartCoroutine(WaitToRespawn()); //in case that the object is empty and it's not waiting to respawn then start the coroutine
                            StopCoroutine(WaitToRespawn()); //once coroutine is done stop it
                        }
                    }
                }
                //this is mainly for if the player dies, or the enemy grabbed it and the item hasn't respawned yet
                //in case the tempPrefab is null and the prefabToSpawn is a weapon
                if (tempPrefab == null && !prefabToSpawn.CompareTag("Enemy"))
                {
                    //SpawnNewItem();
                    //in case there is a no weapon in the player controller then go ahead and respawn the item
                    if (GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon == null)
                    {
                        SpawnNewItem();
                    }
                }
            }
        }
 
    }

    void SpawnNewItem() //spawns in a clone of the object
    {
        if (!prefabToSpawn.CompareTag("Enemy")) //if the object is not an enemy
        {
            tempPrefab = Instantiate(prefabToSpawn, itemSpawnLocation.transform); //clones the prefab and makes it a child of the spawner
        }

        //if the object is an enemy and can spawn
        else if (prefabToSpawn.CompareTag("Enemy") && GameManager.instance.enemiesCanSpawn)
        {
            tempPrefab = Instantiate(prefabToSpawn, itemSpawnLocation.transform); //clones the prefab and makes it a child of the spawner

            if (tempPrefab.GetComponent<AI_Controller>()) //and if it has an AI_Controller
            {
                //make the parent transform equal to this gameobject
                tempPrefab.GetComponent<AI_Controller>().parentLocation = gameObject.transform;
            }
        }

        if (tempPrefab != null) //if an item was spawned
        {
            if (tempPrefab.CompareTag("Multishot") || tempPrefab.CompareTag("Singleshot")) //if the tempPrefab created is either a multi or single shot
            {
                tempPrefab.tag = "PickupWeapon"; //change tag to PickupWeapon
            }

            //adjusts the position of the cloned object to be half the height of the collider
            //of the object so it can spawn above the ground that the spawner is on
            tempPrefab.transform.position = itemSpawnLocation.transform.position +
                                            new Vector3(0, tempPrefab.GetComponent<Collider>().bounds.size.y / 2, 0);
        }
    }

    IEnumerator WaitToRespawn()
    {
        waitingForRespawn = true; //sets as true to ensure the update function doesn't repeatedly start it
        //waits for a random amount between a low and high as determined by the designer
        yield return new WaitForSeconds(Random.Range(lowestWait, highestWait));
        SpawnNewItem(); //respawns the item as another clone
        waitingForRespawn = false; //sets as false until the item is destroyed again
    }

    public void OnDrawGizmos()
    {
        if (prefabToSpawn.GetComponent<AI_Controller>()) //if the spawner is for AI
        {
            Gizmos.color = new Color(1, 1, 0, .5f); //make the gizmo color a yellowish color
            //draw a sphere at the spawners location and offset the y value by half of the prefab's collider
            //bounds size on the y axis
            Gizmos.DrawCube(new Vector3(gameObject.transform.position.x,
                                          prefabToSpawn.GetComponent<Transform>().position.y + 1,
                                          gameObject.transform.position.z),
                                          new Vector3(prefabToSpawn.GetComponent<Transform>().lossyScale.x,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.y * 2,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.z));
        }
        else if (prefabToSpawn.GetComponent<PickUpWeapon>()) //if the spawner is for weapons
        {
            Gizmos.color = new Color(0, 1, 1, .5f); //make the gizmo color a cyan color
            //draw a sphere at the spawners location and offset the y value by half of the prefab's collider
            //bounds size on the y axis
            Gizmos.DrawCube(new Vector3(gameObject.transform.position.x,
                                          prefabToSpawn.GetComponent<Transform>().position.y,
                                          gameObject.transform.position.z),
                                          new Vector3(prefabToSpawn.GetComponent<Transform>().lossyScale.x,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.y,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.z));
        }
        else if (prefabToSpawn.GetComponent<PickUpHealth>()) //if the spawner is for health
        {
            Gizmos.color = new Color(1, 0, 0, .5f); //make the gizmo color a red color
            //draw a sphere at the spawners location and offset the y value by half of the prefab's collider
            //bounds size on the y axis
            Gizmos.DrawCube(new Vector3(gameObject.transform.position.x,
                                          prefabToSpawn.GetComponent<Transform>().position.y,
                                          gameObject.transform.position.z),
                                          new Vector3(prefabToSpawn.GetComponent<Transform>().lossyScale.x,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.y,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.z));
        }
        else if (prefabToSpawn.GetComponent<PickUpAmmo>()) //if the spawner is for ammo
        {
            Gizmos.color = new Color(0, 1, 0, .5f); //make the gizmo color a green color
            //draw a sphere at the spawners location and offset the y value by half of the prefab's collider
            //bounds size on the y axis
            Gizmos.DrawCube(new Vector3(gameObject.transform.position.x,
                                          prefabToSpawn.GetComponent<Transform>().position.y,
                                          gameObject.transform.position.z),
                                          new Vector3(prefabToSpawn.GetComponent<Transform>().lossyScale.x,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.y,
                                          prefabToSpawn.GetComponent<Transform>().lossyScale.z));
        }
    }
}

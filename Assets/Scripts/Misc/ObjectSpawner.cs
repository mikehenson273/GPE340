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
        SpawnNewItem(); //spawns clone of object

        if (lowestWait == 0) //if lowest allowed wait time wasn't set
        {
            lowestWait = 3; //set to 3
        }
        if (highestWait == 0) //if highest allowed wait time wasn't set
        {
            highestWait = 7; //set to 7
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the object is gone, sees if it's not waiting to respawn, if the character has the weapon or not (via tag comparison),
        //AND if the player controller weapon isn't null
        if (tempPrefab == null &&
            !waitingForRespawn &&
            prefabToSpawn != prefabToSpawn.GetComponent<PickUpWeapon>())
        {
            //if all other checks pass, check if the object to spawn is the same weapon that the player is holding
            if (GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon != null &&
                GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon.tag != prefabToSpawn.tag)
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
        //in case there is a no weapon in the player controller then go ahead and respawn the item
        else if (tempPrefab == null && prefabToSpawn != prefabToSpawn.GetComponent<PickUpWeapon>())
        {
            if (GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon == null)
            {
                SpawnNewItem();
            }
        }
    }

    void SpawnNewItem() //spawns in a clone of the object
    {
        tempPrefab = Instantiate(prefabToSpawn, itemSpawnLocation.transform); //clones the prefab and makes it a child of the spawner
        //adjusts the position of the cloned object to be half the height of the object so it can spawn above the ground that the spawner is on
        tempPrefab.transform.position = itemSpawnLocation.transform.position + new Vector3(0, prefabToSpawn.transform.localScale.y / 2, 0);
    }

    IEnumerator WaitToRespawn()
    {
        waitingForRespawn = true; //sets as true to ensure the update function doesn't repeatedly start it
        //waits for a random amount between a low and high as determined by the designer
        yield return new WaitForSeconds(Random.Range(lowestWait, highestWait));
        SpawnNewItem(); //respawns the item as another clone
        waitingForRespawn = false; //sets as false until the item is destroyed again
    }
}

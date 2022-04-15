using UnityEngine;

public class PickUpHealth : PickUpBoosts
{
    [SerializeField] private int addHealth; //designer inputs how much health to heal

    // Start is called before the first frame update
    void Start()
    {
        if (addHealth <= 0) //if the designer didn't add a health amount add one
        {
            addHealth = 25;
        }
    }

    private void OnTriggerEnter(Collider other) //upon collision
    {
        if (other.gameObject.CompareTag("Player")) //if the colliding object is the player
        {
            other.gameObject.GetComponent<Health>().AddToHealth(addHealth); //add designer amount of points to health
            Destroy(gameObject); //destroys this object
        }
    }
}

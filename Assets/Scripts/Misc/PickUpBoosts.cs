using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBoosts : MonoBehaviour
{
    
    [SerializeField] protected float rotationSpeed; //allows the choosing of rotation speed
    protected Vector3 rotation; //allows manipulation so the item can rotate

    [SerializeField] protected float bobbingSpeed; //allows the item pickup to bob
    //allows manipulation to add a bobbing effect to the item
    protected Vector3 bobbingLocation1;
    protected Vector3 bobbingLocation2;
    protected bool bobbedUp; //check to see if the item has made it to its location

    protected void Awake() //used protected awake as it will still carry on down through the classes and allow additional effects through start if needed
    {
        rotation = Vector3.down; //sets the rotation vector as pointing down to enable spinning on the y axis
        BobbingLocations(); //sets up the bobbing locations so the item can go up and down to attract attention

        if (rotationSpeed <= 0) //if the designer didn't set a rotation speed, make one
        {
            rotationSpeed = 120;
        }

        if (bobbingSpeed == 0) //if the designer didn't set a bobbing speed, make one
        {
            bobbingSpeed = .5f;

        }
    }

    // Update is called once per frame
    protected void Update()
    {
        RotatePickup(); //rotates the item
        BobbingPickup(); //bobs the item up and down to make more noticeable
    }

    public void RotatePickup() //function for rotating the item
    {
        gameObject.transform.Rotate(rotation * rotationSpeed * Time.deltaTime); //rotates the object at a speed determined by the designer
    }

    public void BobbingPickup() //bobs the item up and down
    {
        //checks if the item has gone up to the second location
        if (!bobbedUp)
        {
            //if it hasn't then move it up
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, bobbingLocation2, bobbingSpeed * Time.deltaTime);
            if (gameObject.transform.localPosition.y >= bobbingLocation2.y) //if it has reached or passed the location
            {
                bobbedUp = true; //flip to true so it heads down
            }
        }

        else if (bobbedUp) //if it HAS reached the upper location
        {
            //start moving it down
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, bobbingLocation1, bobbingSpeed * Time.deltaTime);
            if (gameObject.transform.localPosition.y <= bobbingLocation1.y) //if it has reached or passed the location
            {
                bobbedUp = false; //flip to false so it heads up
            }
        }
    }

    public void BobbingLocations() //grabs the world space so it doesn't snap to a random location
    {
        //creates the first bobbing location in the world space as the object spawner that made this item and adds its local coords to the location
        bobbingLocation1 = gameObject.transform.parent.transform.position + new Vector3(0,
                                                                       gameObject.transform.localPosition.y,
                                                                       0);
        //creates the second bobbing location in the world space as the object spawner that made this item and adds its local coords to the location
        //as well as an additional amount to the local y so it has an upper location to move to
        bobbingLocation2 = gameObject.transform.parent.transform.position + new Vector3(0,
                                                                       gameObject.transform.localPosition.y + .5f,
                                                                       0);
    }
}

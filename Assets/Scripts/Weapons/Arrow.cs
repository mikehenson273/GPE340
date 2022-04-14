using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Weapon
{
    [SerializeField] private float arrowSpeed;
    private Rigidbody arrowBody;
    private bool isDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;

        gameObject.transform.forward = new Vector3(GameManager.instance.tempPlayerCharacter.transform.forward.x,
                                                   GameManager.instance.tempPlayerCharacter.transform.forward.y,
                                                   GameManager.instance.tempPlayerCharacter.transform.forward.z);

        gameObject.transform.parent = GameManager.instance.playerControls.transform;
        if (arrowSpeed <= 0) //if there isn't a speed set then set a speed automatically
        {
            arrowSpeed = 10;
        }
        arrowBody = gameObject.GetComponent<Rigidbody>();

        StartCoroutine(ExistTimer());
    }

    // Update is called once per frame
    new void Update()
    {
        if (arrowBody != null)
        {
            arrowBody.AddRelativeForce(0, 0, arrowSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        if (isDestroyed == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (arrowBody != null)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.transform.parent = collision.gameObject.transform;
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }

    IEnumerator ExistTimer()
    {
        //waits for the amount of time determined by the designer
        yield return new WaitForSeconds(5); //timer for designer time for how often the user can fire

        isDestroyed = true; //sets as false until the item is destroyed again
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Controller : MonoBehaviour
{
    [HideInInspector] public Transform parentLocation; //holds the parent of the AI (where it spawned) so everything associated with it can be found easier
    [HideInInspector] public Pawn pawn; //holds the character of the AI
    [HideInInspector] public GameObject weapon; //holds whatever weapon the character is using

    [HideInInspector] public NavMeshAgent moveAgent; //allows the AI to move according to the nav mesh
    [HideInInspector] public GameObject targetPosition; //looks for the current target game object

    [Header("Otherwise AI will never attack.")]
    [Header("Stop Distance should be smaller than attack distance.")]
    
    public float stopDistanceFromPlayer; //enables the designer to choose a stopping distance from the player
    public float distanceToAttackPlayer; //allows the designer to choose how far away to attack the player

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Pawn>())
        {
            pawn = gameObject.GetComponent<Pawn>(); //sets the Pawn as the gameobjects pawn script (if it exists)
        }
        if (gameObject.GetComponent<NavMeshAgent>())
        {
            moveAgent = GetComponent<NavMeshAgent>(); //sets the navmesh as the gameobjects navmesh agent component (if it exists)
        }
        if (stopDistanceFromPlayer <= 0) //if a distance wasn't set or if it was set to negative
        {
            stopDistanceFromPlayer = 5; //make default distance 5
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pawn != null)
        { 
            MovingAI(); //moves the AI
            AttackingAI(); //runs the Attacking script
            LookAtPlayer(); //turns the AI to face the player
        }

        //if the player is dead and controller pawn is null (to ensure the player isn't swarmed
        //upon spawn and allows them to get a weapon)
        if (GameManager.instance.playerControls.GetComponent<Player_Controller>().pawn == null) 
        {
            //kill it and activate the ragdoll effect
            gameObject.GetComponent<Health>().ReduceHealth(gameObject.GetComponent<Health>().GetMaxHealth());
        }
    }

    private void MovingAI() 
    {
        if (targetPosition == null && weapon == null) //if the target position is empty and the pawn has no weapon
        {
            if (GameObject.FindGameObjectWithTag("PickupWeapon")) //find a weapon
            {
                targetPosition = GameObject.FindGameObjectWithTag("PickupWeapon"); //set target location as the weapon
            }
            moveAgent.stoppingDistance = 0; //set the stopping distance to 0 so it picks up the weapon
        }
        else if (pawn.GetPawnWeapon() != null) //if the pawn has gotten a weapon
        {
            targetPosition = GameManager.instance.tempPlayerCharacter; //set new target position as the player
            if (moveAgent.stoppingDistance != stopDistanceFromPlayer) //if the stopping distance is not equal to the chosen stopping distance
            {
                moveAgent.stoppingDistance = stopDistanceFromPlayer; //change the stopping distance equal to the stop distance set by designer
            }
        }
        if (targetPosition != null) //if the target position is not equal to null
        {
            moveAgent.SetDestination(targetPosition.transform.position); //set the destination for the AI to move to
            pawn.MovePlayerCharacter(moveAgent.desiredVelocity); //move the pawn according to the agents desired velocity
        }
    }

    private void LookAtPlayer()
    {
        //if the AI has a weapon and the targetPosition (what it's heading for) exists then do this
        if (weapon != null && targetPosition != null && gameObject != null)
        {
            Vector3 targetRotation = targetPosition.transform.position - transform.position; //create a rotation vector
            float rotateSpeed = 1 * Time.deltaTime; //create a rotation speed
            //create a reference of where to turn to
            Vector3 newRotation = Vector3.RotateTowards(gameObject.transform.forward, targetRotation, rotateSpeed, 0.0f);
            //Debug.DrawRay(transform.position, targetRotation, Color.red); //draws a ray to the players location
            gameObject.transform.rotation = Quaternion.LookRotation(targetRotation); //rotate towards the player
        }
    }

    private void AttackingAI()
    {
        if (weapon != null) //if the AI has a weapon
        {
            if (moveAgent.remainingDistance <= distanceToAttackPlayer) //start firing when in range of double the stopping distance
            {
                if (!weapon.GetComponent<Weapon>().FiredWeapon()) //tests if the AI has fired and waits in between shots
                {
                    //Debug.Log("Enemy has fired");
                    pawn.anim.SetBool("FireBow", true); //sets the FireBow bool in animator to true
                    weapon.GetComponent<Weapon>().FireWeapon("Enemy"); //fires the weapon and passes in the name of the layer to add to ammo
                }
            }
            else if (moveAgent.remainingDistance > distanceToAttackPlayer) //if the AI is farther than double the stopping distance
            {
                pawn.anim.SetBool("FireBow", false); //set FireBow bool in animator to false
            }
        }
    }

    private void OnAnimatorMove() //whenever the navmesh moves
    {
        if (moveAgent != null) //as long as the move agent is not null
        {
            moveAgent.velocity = gameObject.GetComponent<Animator>().velocity; //make the velocity of the moveagent the same as animator velocity
        }
    }
}

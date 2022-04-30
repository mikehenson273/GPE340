using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //variables
    public float moveSpeed; //how fast the player can move
    private bool isSprinting; //determines if pawn is sprinting
    [SerializeField] private float sprintSpeed; //gives user a speed adjustment if they're sprinting
    
    [HideInInspector] public Animator anim; //player animator so that the character is animated... Also allows movement through root motion

    private Weapon pawnWeapon; //holds the weapon portion so those variables can be accessed easier
    
    public Transform leftHand, //holds where a weapon/item can go in the left hand
                     rightHand; //holds where a weapon/item can go in the right hand

    private Rigidbody[] childRigidBodies; //holds all the rigid bodies of the object
    private Collider[] childColliders; //holds all the colliders of the object
    private Rigidbody rootRigidBody; //holds the root rigid body of the object
    private Collider rootCollider; //holds the root collider of the object

    [HideInInspector] public Vector3 debugForward; //for drawing a debug ray in front of the pawn
    [HideInInspector] public Vector3 debugCamForward; //for drawing a debug ray from camera to mouse position (can't really get it working)

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //sets up variable with this objects corresponding component
        if (sprintSpeed <= 0)
        {
            sprintSpeed = 1.5f; //hard sets sprint value if one wasn't set
        }
        
        //checks to make sure that the game object that this scrip is attached to is the player and if it is then add the player's character movement
        //to the player controller object in the scene
        if (gameObject.tag == "Player")
        {
            //Debug.Log("Found Player Character"); //informs the player character was found
            //Looks for the move_controller in the game manager and associates the pawn movement with character movement. 
            GameManager.instance.playerControls.GetComponent<Player_Controller>().pawn = gameObject.GetComponent<Pawn>();
            //Debug.Log("Added Character Movement to Player Controller"); //informs the controls were added
        }
        //if the object that this is attached to is NOT the player then attach this objects character movement 
        //to the movement controller ON the object
        else if (gameObject.tag == "Enemy")
        {
            //make an enemy controller for this
            gameObject.GetComponent<AI_Controller>().pawn = gameObject.GetComponent<Pawn>();
        }

        //associates the root collider and rigidbody with these components
        rootCollider = gameObject.GetComponent<Collider>();
        rootRigidBody = gameObject.GetComponent<Rigidbody>();

        //get child object components off of all the children, includes the root ones as well
        childColliders = GetComponentsInChildren<Collider>();
        childRigidBodies = GetComponentsInChildren<Rigidbody>();

        StopRagdoll(); //in case the pawn is spawned in while being ragdolled
    }

    private void Update()
    {
        //only runs if playerWeapon doesn't exist but weapon in player controller DOES exist FOR THE PLAYER
        if (gameObject.CompareTag("Player"))
        {
            if (gameObject.GetComponentInParent<Player_Controller>().weapon != null && pawnWeapon == null)
            {
                pawnWeapon = gameObject.GetComponentInParent<Player_Controller>().weapon.GetComponent<Weapon>();
            }
        }
        //only runs if enemy has a weapon in their controller but not here
        if (gameObject.CompareTag("Enemy"))
        {
            if (gameObject.GetComponent<AI_Controller>().weapon != null && pawnWeapon == null)
            {
                pawnWeapon = gameObject.GetComponent<AI_Controller>().weapon.GetComponent<Weapon>();
            }
        }

        //debugForward = transform.TransformDirection(Vector3.forward) * 100;
        //Debug.DrawRay(transform.position, debugForward, Color.green);

        //debugCamForward = GameManager.instance.cam.transform.TransformDirection(Vector3.forward) * 100;
        //Debug.DrawRay(GameManager.instance.cam.transform.position, debugCamForward, Color.blue);
    }

    public void MovePlayerCharacter(Vector3 movementDirection) //moves the pawn based off input
    {
        //convert character direction from world space to local space
        movementDirection = transform.InverseTransformDirection(movementDirection); //moves the pawn based off world space

        //Clamp (sets vector3 magnitude to <1)
        //Debug.Log(movementDirection.magnitude);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f); // maxes out the direction so it's more uniform across platforms

        //multiply by speed -> we are now basing it off speed and not the direction magnitude
        movementDirection *= moveSpeed; //same as movementDirection = movementDirection * speed

        if (isSprinting) //if the player has some sprint and is holding the left shift key
        {
            movementDirection *= sprintSpeed; //speeds up the movement further by 1.5 times
        }

        //pass parameters to animator to determine which animation is playing
        anim.SetFloat("Right", movementDirection.x); //sets the float speed for animations
        anim.SetFloat("Forward", movementDirection.z); //sets the float speed for animations
    }
   

    public void SetSprinting(bool sprintKey) //function for setting the is sprinting boolean
    {
        isSprinting = sprintKey; //sets the sprint based on if the controller is sprinting
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (pawnWeapon != null)
        {
            if (anim.GetBool("FireBow")) //if the pawn is currently firing the bow then change it's positioning
            {
                if (pawnWeapon.leftHandGrip != null) //if there's a weapon spot associated with left hand
                {
                    anim.SetIKPosition(AvatarIKGoal.LeftHand, pawnWeapon.leftHandGrip.position); //moves left hand to the grip position on weapon
                    anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); //makes it a priority
                    anim.SetIKRotation(AvatarIKGoal.LeftHand, pawnWeapon.leftHandGrip.rotation); //rotates hand to rotation on weapon
                    anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f); //makes it a priority
                                                                           //GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon.GetComponent<Weapon>().leftHandGrip
                }
                if (pawnWeapon.rightHandGrip != null) //if there's a weapon spot associated with Right Hand
                {
                    //Debug.Log("Firing layer weight is " + anim.GetLayerWeight(anim.GetLayerIndex("Firing")));
                    //anim.SetIKPosition(AvatarIKGoal.RightHand, playerWeapon.rightHandGrip.position); //moves right hand to the grip position on weapon
                    //anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f); //makes it a priority
                    //anim.SetIKRotation(AvatarIKGoal.RightHand, playerWeapon.rightHandGrip.rotation); //rotates hand to rotation on weapon
                    //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f); //makes it a priority
                }
                if (anim.GetBool("FireBow")) //if the character is currently firing
                {
                    anim.SetLayerWeight(anim.GetLayerIndex("Firing"), 1); //set firing layer to activate
                }
            }
            else if (!anim.GetBool("FireBow")) //if the player isn't firing anymore then turn off the animated positioning
            {
                anim.SetLayerWeight(anim.GetLayerIndex("Firing"), 0);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
                //Debug.Log("Firing layer weight is " + pawn.anim.GetLayerWeight(pawn.anim.GetLayerIndex("Firing")));
            }
        }
        else
        {
            //if there is no weapon whatsoever then make it all zero
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        }
    }

    public void StartRagdoll()
    {
        anim.enabled = false; //disables animations

        foreach (Collider collider in childColliders) //for every child with a collider
        {
            collider.enabled = true; //allows the use of the child colliders so that they're interactable so they can be moved when they fall
        }

        foreach (Rigidbody rigidBodies in childRigidBodies) //for every child with a rigid body
        {
            rigidBodies.isKinematic = false; //allows the use of the child rigid bodies to apply some movement when they fall
        }

        rootCollider.enabled = false; //turns off the root collider so it can drop to the floor
        rootRigidBody.isKinematic = true; //becomes kinematic resulting in the large rigid body not being used
    }

    public void StopRagdoll()
    {
        anim.enabled = true; //allows animations to play

        foreach (Collider collider in childColliders) //for every child with a collider
        {
            collider.enabled = false; //turn off the collider so the small colliders aren't being used
        }

        foreach (Rigidbody rigidBodies in childRigidBodies) //for every child with a rigid body
        {
            rigidBodies.isKinematic = true; //make it kinematic so that they aren't being used
        }

        rootCollider.enabled = true; //turn on the collider in the root of the game object
        rootRigidBody.isKinematic = false; //allows use of main rigid body for the entire gameobject
    }

    public Weapon GetPawnWeapon()
    {
        return pawnWeapon;
    }
    public void DestroyPawnWeapon()
    {
        Destroy(pawnWeapon);
    }
}

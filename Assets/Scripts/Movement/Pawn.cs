using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : MonoBehaviour
{
    //variables
    public float moveSpeed; //how fast the player can move
    private bool isSprinting; //determines if pawn is sprinting
    [SerializeField] private float sprintSpeed; //gives user a speed adjustment if they're sprinting
    
    public Animator anim; //player animator so that the character is animated... Also allows movement through root motion

    private Weapon playerWeapon;
    public Transform leftHand,
                     rightHand;

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
    }

    private void Update()
    {
        if (gameObject.GetComponentInParent<Player_Controller>().weapon != null && playerWeapon == null) //only runs if playerWeapon doesn't exist
        {
            playerWeapon = gameObject.GetComponentInParent<Player_Controller>().weapon.GetComponent<Weapon>();
        }
    }

    public void MovePlayerCharacter(Vector3 movementDirection)
    {
        //convert character direction from world space to local space
        movementDirection = transform.InverseTransformDirection(movementDirection);

        //Clamp (sets vector3 magnitude to <1)
        //Debug.Log(movementDirection.magnitude);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

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
        if (playerWeapon != null)
        {
            if (playerWeapon.leftHandGrip != null)
            {
                anim.SetIKPosition(AvatarIKGoal.LeftHand, playerWeapon.leftHandGrip.position);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, playerWeapon.leftHandGrip.rotation);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
                //GameManager.instance.playerControls.GetComponent<Player_Controller>().weapon.GetComponent<Weapon>().leftHandGrip
            }
            if (playerWeapon.rightHandGrip != null && anim.GetBool("FireBow") == true)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("Firing"), 1);
                //Debug.Log("Firing layer weight is " + anim.GetLayerWeight(anim.GetLayerIndex("Firing")));
                //anim.SetIKPosition(AvatarIKGoal.RightHand, playerWeapon.rightHandGrip.position);
                //anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                //anim.SetIKRotation(AvatarIKGoal.RightHand, playerWeapon.rightHandGrip.rotation);
                //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            }            
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        }
    }
}

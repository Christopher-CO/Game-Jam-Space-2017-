using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace animationTest
{
    public class CameraControl : MonoBehaviour
    {
        public float movementSpeed = 10.0f;
        public float rotationSpeed = 3.0f;
        public float jumpSpeed = 8f; // Player jump speed
        public float vertSpeed = 0f; // current vertical velocity
        public float grav = 0.5f; // Gravity strength 
        private bool onGround = true;
        private Vector3 directionVector = new Vector3(0f, 0f, 0f);

        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

        void Awake()
        {
            // Set up references.
            anim = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!onGround)
            {
                vertSpeed -= grav;
            }
            bool isJump = CrossPlatformInputManager.GetButtonDown("Jump");
            //movement
            float verticleMovement = Input.GetAxis("Vertical") * movementSpeed;
            float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed;
            Vector3 direction = new Vector3(horizontalMovement, 0, verticleMovement);
            direction = transform.rotation * direction;
            CharacterController cc = GetComponent<CharacterController>();
            cc.SimpleMove(direction);
            //rotation
            float rotateH = Input.GetAxis("Mouse X") * rotationSpeed;
            //Debug.Log(Input.GetAxis("Mouse X"));
            float rotateV = Input.GetAxis("Mouse Y") * rotationSpeed;
            direction = transform.rotation * direction;
            this.transform.Rotate(0, rotateH, 0);
            Camera playerCam = this.GetComponentInChildren<Camera>();
            playerCam.transform.Rotate(-rotateV, 0, 0);
            // Animate the player.
            Animating(horizontalMovement, verticleMovement);
        }

        void Animating(float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool("isWalking", walking);
            //anim.SetFloat ("VertSpeed", vertSpeed); 
            //anim.SetBool ("OnGround", onGround); 

        }

    }
}

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace animationTest
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.
		public float jumpSpeed = 8f; 		// Player jump speed
		public float vertSpeed = 0f; 		// current vertical velocity 
		public float grav = -0.5f;			// Gravity strength 
		private bool onGround = true; 
		private Vector3 directionVector = new Vector3 (0f, 0f, 0f);
		public float rotSpeed = 1f; 


        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
#if !MOBILE_INPUT
        int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        float camRayLength = 100f;          // The length of the ray from the camera into the scene.
#endif

        void Awake ()
        {
#if !MOBILE_INPUT
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask ("Floor");
#endif

            // Set up references.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void FixedUpdate ()
        {
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

			if (!onGround) {
				vertSpeed -= grav; 
			}

			bool isJump = CrossPlatformInputManager.GetButtonDown ("Jump"); 

			/*if (isJump && onGround) {
				onGround = false; 
				vertSpeed = jumpSpeed; 
				anim.SetTrigger ("Jump"); 
				Debug.Log ("Jumped! "+onGround);
			}*/

            // Move the player around the scene.
            Move (h, vertSpeed, v);

            // Turn the player to face the mouse cursor.
			Turning (h, v);

            // Animate the player.
            Animating (h, v);
        }


        void Move (float h, float vertSpeed, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set (h, 0f, v);

			//Vector3 vertical = new Vector3 (0f, vertSpeed, 0f) * Time.deltaTime; 
			//Debug.Log (vertical.y); 
            
            // Normalise the movement vector and make it proportional to the speed per second.
			movement = movement.normalized * speed * Time.deltaTime; //+ vertical;

			/* no jumping right now 
			// Don't let player fall through the floor
			Vector3 newPos = transform.position + movement; 

			if (newPos.y <= 0) {
				newPos.y = 0;
				onGround = true;  
			}
			*/

            // Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);


        }


		void Turning (float h, float v)
        {
#if !MOBILE_INPUT
            /*// Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation (newRotatation);
            }*/

			//Vector3 currentDir = playerRigidbody.rotation.eulerAngles; 



			//Quaternion newRotation = Quaternion.LookRotation (Vector3.Lerp(currentDir,targetDir,0.5f));

			// Set the player's rotation to this new rotation.
			//playerRigidbody.MoveRotation (newRotation);

			 

			if (h*h > 0 || v*v > 0) { 

				// update directionVector axis values have changed 
				directionVector.x = h; 
				directionVector.y = 0f; 
				directionVector.z = v; 
			

			}
				
			Quaternion lerp = Quaternion.Lerp(playerRigidbody.rotation, Quaternion.LookRotation(directionVector), rotSpeed); 

			playerRigidbody.MoveRotation (lerp);

#else

            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
#endif
        }


        void Animating (float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool ("isWalking", walking);
			//anim.SetFloat ("VertSpeed", vertSpeed); 
			//anim.SetBool ("OnGround", onGround); 
			 
        }
    }
}
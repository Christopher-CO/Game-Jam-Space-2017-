using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MonsterBehavior : MonoBehaviour {

	private GameObject myHuman; 
	private GameObject monsterHand; 
	private bool humanIsSnapped;
	Animator animator; 
	public float thrust = 4f; 

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		humanIsSnapped = false; 
	}
	
	// Update is called once per frame
	void Update () {
	
		if (monsterHand == null) {
			monsterHand = GameObject.FindWithTag ("MonsterHand");
		} 

		if (monsterHand != null && myHuman != null) {
			
			//if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition (0)) {
				//if (AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo (0).IsName("Eat")) {	
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Eat")) {

				if (animator.GetCurrentAnimatorStateInfo (0).normalizedTime < 1.0f) {
					Debug.Log ("holding human"); 
					myHuman.transform.position = monsterHand.transform.position;
					myHuman.transform.rotation = monsterHand.transform.rotation * Quaternion.Euler (new Vector3 (90f, 0f, 0f)); 
					humanIsSnapped = true; 
				}
			} else if (humanIsSnapped) {
				Destroy (myHuman); 
				Debug.Log ("Human destroyed"); 
			}
				
		}

		if (!CrossPlatformInputManager.GetButtonDown("Jump") && CrossPlatformInputManager.GetButtonDown ("Fire3")) {
			animator.SetTrigger ("punchTrigger"); 
		}
		else if (CrossPlatformInputManager.GetButtonDown("Jump")) {
			animator.SetTrigger("eatTrigger");
		} 
	}

	bool AnimatorIsPlaying() {
		return animator.GetCurrentAnimatorStateInfo (0).length > animator.GetCurrentAnimatorStateInfo (0).normalizedTime;
	}

	void OnCollisionEnter(Collision col) {
		Debug.Log ("collided"); 
		if (!col.gameObject.CompareTag ("Floor")) {
			if (col.gameObject.CompareTag ("Human")) {
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Eat")) {
					
					if (myHuman == null) { 
						
						myHuman = col.gameObject; 
						myHuman.GetComponent<Rigidbody> ().detectCollisions = false; // disable collisions
					}
				} 
			}

			if (col.gameObject.CompareTag ("Human") || col.gameObject.CompareTag ("PhysObj")) {
				col.gameObject.GetComponent<Rigidbody> ().AddForce (transform.forward * thrust);

			}
		}
	}
}

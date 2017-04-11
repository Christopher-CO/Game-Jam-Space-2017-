// https://docs.unity3d.com/Manual/AnimationParameters.html

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AnimationUpdate : MonoBehaviour
{
	public float moveSpeed = .72F; // forward move speed
	public float rotateSpeed = 2.2F; // rotation speed
	public float wallBuffer = 1.2F; // distance of raycast from self. used for collision detection with walls
	private Quaternion target; // a randomized rotation away from the wall that the npc will gradually turn towrds to turn away 
	private float timeOfLastCollision; // stores the time that the npc last changed direction. used to avoid changing the rotation until x amount of time has passed since the last collison
	private string avoidLeftRightLeft = "..."; //welcome to the spaghetti code. I couldn't think of a better way to keep the npc from turning left right left or right left right and getting stuck in a corner
	List<Collider> colliders; //List to hold the references to all the removed nps

	Animator animator;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		target = transform.rotation;
		colliders = new List<Collider>();
	}

	// Update is called once per frame
	void Update()
	{
		//float h = Input.GetAxis("Horizontal");
		//float v = Input.GetAxis("Vertical");

		RaycastHit hit;
		int left = 1; // whether it turnns left or right
		float step = rotateSpeed * Time.deltaTime; // the rate at which to incrment the turn

		//if (Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) > 3 * step)
		// {
		//      transform.eulerAngles.Set(transform.eulerAngles.x, transform.eulerAngles.y + step * left, transform.eulerAngles.z);
		// }
		transform.rotation = Quaternion.Slerp(transform.rotation, target, step); // interpolates values for the turn
		transform.position += transform.forward * moveSpeed * Time.deltaTime; // forward motion
		animator.SetTrigger("walkStart");
		Debug.DrawRay(transform.position, transform.forward * wallBuffer, Color.red); // visual representiaion of the raycast in scene view

		if (Physics.Raycast(transform.position, transform.forward, out hit, wallBuffer))  // everything in here is just to randomize the target rotation
		{
			if (hit.collider.tag == "Wall" && Time.time - timeOfLastCollision > .75)
			{

				float wallRotation = hit.transform.eulerAngles.y;
				float minRotation = wallRotation;
				float maxRotation = minRotation + 180;
				if (maxRotation > 360)
					maxRotation -= 360;
				int randomDirection = Random.Range(0, 2);

				if (randomDirection < 1)
					left = -1;
				avoidLeftRightLeft += left;
				avoidLeftRightLeft = avoidLeftRightLeft.Substring(avoidLeftRightLeft.Length - 4);
				Debug.Log(avoidLeftRightLeft);
				float randomDegreeOfRotation = Random.Range(minRotation - 50F, (maxRotation + 1) + 50F);
				if ((!avoidLeftRightLeft.Equals("-11-1") && !avoidLeftRightLeft.Equals("1-11")) || Time.time - timeOfLastCollision > 2.5)
					randomDegreeOfRotation = randomDegreeOfRotation * left;
				Debug.Log(randomDegreeOfRotation);
				target.eulerAngles = new Vector3(0.0F, randomDegreeOfRotation, 0.0F);
				timeOfLastCollision = Time.time;
			}
		}

	}
	void OnCollisionEnter(Collision col) // to be completed
	{   

		if (col.gameObject.tag == "Player")
		{
				animator.SetTrigger("Die");
			//this.active = false;

		}
	}
}




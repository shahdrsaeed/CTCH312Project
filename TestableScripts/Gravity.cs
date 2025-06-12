using UnityEngine;

public class Gravity : MonoBehaviour
{
	Rigidbody body;
	Vector3 upDirection;
	bool isGrounded = false;

	public GameObject planet;
	public float g = 9.81f;

   void Start()
   {
	body = GetComponent<Rigidbody>();
   }

   void Update()
   {
	upDirection = this.transform.position - planet.transform.position;
	upDirection.Normalize();
	if(!isGrounded) { body.AddForce(-upDirection*g, ForceMode.Acceleration); }
   }

   void OnCollisionStay()
   {
	if(!isGrounded) {
		if(body.linearVelocity.magnitude < 0.05) { isGrounded = true; }
	}
   }

   void OnCollisionExit()
   {
	isGrounded = false;
   }

}

using UnityEngine;

public class Gravity : MonoBehaviour
{
	Rigidbody body;
	Vector3 upDirection;

	public GameObject planet;
	public float g = 9.81f;
	public Vector3 initialVelocity;

   void Start()
   {
	body = GetComponent<Rigidbody>();
	body.AddForce(initialVelocity, ForceMode.VelocityChange);
   }

   void FixedUpdate()
   {
	upDirection = this.transform.position - planet.transform.position;
	upDirection.Normalize();
	body.AddForce(-upDirection*g, ForceMode.Acceleration);
   }
}
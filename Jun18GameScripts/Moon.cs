using UnityEngine;

public class Moon : MonoBehaviour
{
	Rigidbody body;
	Vector3 upDirection;

	public GameObject planet;
	public float g = 9.81f;
	public Vector3 initialVelocity;
	public Vector3 initialAngularVelocity;

   void Start()
   {
	body = GetComponent<Rigidbody>();
	body.AddRelativeForce(initialVelocity, ForceMode.VelocityChange);
	body.AddRelativeTorque(initialAngularVelocity, ForceMode.VelocityChange);
   }

   void FixedUpdate()
   {
	upDirection = this.transform.position - planet.transform.position;
	upDirection.Normalize();
	body.AddForce(-upDirection*g, ForceMode.Acceleration);
   }
}
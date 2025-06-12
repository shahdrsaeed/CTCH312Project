using UnityEngine;

public class Gem : MonoBehaviour
{
	Vector3 upDirection;
	Vector3 zeroPosition;
	public float phase = 0f;
	public string planetName = "Planet";

	void Start()
	{
		zeroPosition = this.transform.position;
        	GameObject planet = GameObject.Find(planetName);
		upDirection = zeroPosition - planet.transform.position;
		upDirection.Normalize();
		Quaternion rotation = Quaternion.FromToRotation(this.transform.up, upDirection);
		this.transform.rotation = rotation*this.transform.rotation;
		phase *= Mathf.PI/180;		//Mathf.Sin takes radians
	}

	void FixedUpdate()
	{
        	this.transform.position = zeroPosition + upDirection*Mathf.Sin(3*Time.fixedTime + phase)/2;
		this.transform.Rotate(0, 1.5f, 0);
	}
}

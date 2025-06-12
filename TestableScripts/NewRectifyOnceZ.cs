using UnityEngine;

public class NewRectifyOnceZ : MonoBehaviour
{
    void Start()
    {
	GameObject planet = GameObject.Find("Planet");
	Vector3 upDirection = this.transform.position - planet.transform.position;
	upDirection.Normalize();
	Quaternion rotation = Quaternion.FromToRotation(this.transform.forward, upDirection);
	this.transform.rotation = rotation*this.transform.rotation;
    }
}
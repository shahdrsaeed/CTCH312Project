using UnityEngine;

public class RectifyOnce : MonoBehaviour
{
    void Start()
    {
	GameObject planet = GameObject.Find("Planet");
	Vector3 upDirection = this.transform.position - planet.transform.position;
	upDirection.Normalize();
	Quaternion rotation = Quaternion.FromToRotation(this.transform.up, upDirection);
	this.transform.rotation = rotation*this.transform.rotation;
    }
}

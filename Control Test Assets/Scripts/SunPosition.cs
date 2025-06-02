using UnityEngine;

public class SunPosition : MonoBehaviour
{
    public float rate = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Rotate(0, rate, 0, Space.World);
    }
}

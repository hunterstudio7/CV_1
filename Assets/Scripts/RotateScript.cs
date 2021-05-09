
using UnityEngine;

public class RotateScript : MonoBehaviour {
    public float speed = 5f;
    private Transform rotator;
    private GameObject myObj;

    private void Start()
    {
        rotator = GetComponent<Transform>();
    }
    private void Update()
    {
      
        


            rotator.Rotate(0, speed * Time.deltaTime, 0);
        
    }

}


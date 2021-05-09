using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCube : MonoBehaviour

{
    public GameObject restartBut, explosion;
    private bool collisonSet;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cube"&& !collisonSet)
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f,Vector3.up,5f);
                child.SetParent(null);
            }
            restartBut.SetActive(true);
           
            Camera.main.gameObject.AddComponent<CameraShake>();
            Camera.main.transform.localPosition -= new Vector3(0, 0, 5f);
            GameObject newVfx = Instantiate(explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            Destroy(newVfx, 2.5f);
            Destroy(collision.gameObject);
            collisonSet = true;

        }
    }
}

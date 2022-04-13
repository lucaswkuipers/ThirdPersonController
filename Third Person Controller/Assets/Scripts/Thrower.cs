using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public float velocity;
    public GameObject prefab;
    public Camera cam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Vector3 dir = ray.GetPoint(1) - ray.GetPoint(0);

            GameObject throwedObject = Instantiate(prefab, ray.GetPoint(2), Quaternion.LookRotation(dir));

            throwedObject.GetComponent<Rigidbody>().velocity = throwedObject.transform.forward * velocity;
            Destroy(throwedObject, 3);
        }
    }
}

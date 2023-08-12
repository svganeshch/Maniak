using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairPos : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;

        if (Physics.Raycast(ray, out hit))
            transform.position = hit.point;
        else
            transform.position = ray.origin + ray.direction * 1000.0f;
    }
}
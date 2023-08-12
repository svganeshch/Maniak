using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMechanics : MonoBehaviour
{
    public Transform shootOriginPoint;
    public GameObject bullet;
    public float bulletSpeed = 100f;

    private Ray ray;
    private RaycastHit hit;
    public void Shoot()
    {
        GameObject bulletInstance = Instantiate(bullet, shootOriginPoint.position, Quaternion.identity);
        if (bulletInstance != null)
        {
            if (bulletInstance.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.AddForce(Camera.main.transform.forward * bulletSpeed);
        }

        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;
        
        Physics.Raycast(ray, out hit, 1000f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Obstacle"))
                Destroy(bulletInstance);

            Destroy(bulletInstance, 5);
            Debug.Log("bullet fired");
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
    }
}

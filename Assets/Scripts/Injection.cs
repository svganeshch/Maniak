using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injection : MonoBehaviour
{
    public Material dissolveEffect;

    public float damage = 10f;

    private Rigidbody rb;

    private bool isThrown = false;
    private bool isAttached = false;

    public float torque = 0.1f;

    private HealthController healthController;

    private Transform target = null;

    private void Awake()
    {
        //SetTarget(target);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if ((!isThrown && !isAttached) && target != null)
            transform.LookAt(target.position);
    }

    public GameObject InstantiateInj(GameObject InjPrefab, Transform InjHoldPos, Transform target)
    {
        GameObject InjectionObj;

        InjectionObj = Instantiate(InjPrefab, InjHoldPos.position, Quaternion.identity, InjHoldPos);
        InjectionObj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        InjectionObj.layer = InjHoldPos.gameObject.layer;

        InjectionObj.TryGetComponent<Injection>(out Injection injection);
        injection.target = target;

        return InjectionObj;
    }

    public void Throw(Transform target, float throwForce)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.isKinematic = false;
        rb.AddForce(direction * throwForce);
        //rb.AddTorque(torque, 0, 0);

        isThrown = true;
    }

    //public void SetTarget(Transform target)
    //{
    //    this.target = target;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
            {
                GameObject collisonObj = collision.gameObject;

                ContactPoint contactPoint = collision.contacts[0];

                //Debug.Log("Hit with inj pos : " + contactPoint.point);
                //Debug.Log("Collided with : " + collision.collider.name);
                //if (contactPoint.point == GetComponentInChildren<Tip>().transform.position)
                //{
                    transform.parent = collisonObj.transform;
                    transform.position = contactPoint.point;
                //}
                rb.isKinematic = true;

                //var mesh = collisonObj.GetComponent<MeshRenderer>();
                //mesh.material = dissolveEffect;

                // Health
                collisonObj.transform.root.TryGetComponent<HealthController>(out healthController);
                if (healthController != null) healthController.TakeDamage(damage);
                isAttached = true;
            }
            //else if (/* isThrown && !isAttached */ true)
            //{
            //    Destroy(gameObject, 3);
            //}
            Destroy(gameObject, 3);
        }
    }
}

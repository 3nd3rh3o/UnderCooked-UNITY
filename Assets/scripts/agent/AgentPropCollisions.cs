using UnityEngine;

public class AgentPropCollisions : MonoBehaviour
{
    public float pushForce = 10f;
    public float maxPushSpeed = 3f;


    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.transform.parent == transform? null : other.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;
        Vector3 dir = (rb.position - transform.position).normalized;


        if (rb.linearVelocity.magnitude < maxPushSpeed)
        {
            rb.AddForce(dir * pushForce, ForceMode.Acceleration);
        }
    }
}

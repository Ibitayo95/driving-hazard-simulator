using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public void HitByVehicle(Vector3 direction, float force)
    {
        // Disable the Animator
        animator.enabled = false;
        
        // Enable all the rigidbodies
        foreach (var body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = false;
            body.AddForce(direction * force, ForceMode.Impulse);
        }

       
    }
}

using UnityEngine;

public class Baby : MonoBehaviour
{

    public Transform baby;
    public Rigidbody babyrb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("logT"))
        {
            babyrb.isKinematic = true;
            baby.SetParent(null);
        }
    }

}

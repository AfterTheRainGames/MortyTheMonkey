using UnityEngine;

public class WallRun : MonoBehaviour
{

    public Transform player;
    public Transform wall;
    private Rigidbody rb;
    public bool isWallRunning = false;
    public float bounceForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
            if (isWallRunning)
            {
            Vector3 directionToWall = new Vector3(player.position.x - wall.position.x, 0f, 0f);
            rb.AddForce(directionToWall * 1f);
            }
        Debug.Log(isWallRunning);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("WR"))
        {
            isWallRunning = true;
            rb.useGravity = false;
        }

        if (other.CompareTag("Bounce"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
            rb.AddForce(player.up * bounceForce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("WR"))
        {
            isWallRunning = false;
            rb.useGravity = true;
        }
    }

}

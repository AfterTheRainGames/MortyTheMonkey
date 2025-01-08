using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("GetComps")]
    private Rigidbody rb;
    private Transform player;
    public CamControl camControlScript;

    [Header("Speeds")]
    private float maxSpeed = 5f;


    [Header("Camera")]
    private Transform cam;

    [Header("Bools")]
    public bool controlGiven;
    public bool isGrounded;
    public bool end = false;

    private Vector3 currentSpeed;

    void Start()
    {
        //GetComps
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        cam = Camera.main.transform;

        //Initalize Bools
        rb.freezeRotation = true;
        controlGiven = false;
        currentSpeed = Vector3.zero;
    }

    void Update()
    {

        if (controlGiven == true)
        {
            PlayerMovement();
            Jump();
        }

    }

    void PlayerMovement()
    {
        Vector3 forwardDirection = cam.forward;
        Vector3 rightDirection = cam.right;
        forwardDirection.y = 0;
        rightDirection.y = 0;

        float forwardPos = Input.GetAxisRaw("Vertical");
        float rightPos = Input.GetAxisRaw("Horizontal");


        Vector3 moveDirection = (forwardDirection * forwardPos + rightDirection * rightPos).normalized;

        if (moveDirection.magnitude > 0)
        {
            currentSpeed = moveDirection * maxSpeed;
        }
        else
        {
            currentSpeed = Vector3.zero;
        }

        Vector3 targetPosition = rb.position + currentSpeed * Time.deltaTime; //Add Input to Current Pos
        rb.MovePosition(targetPosition); //Move to Input Pos
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(player.up * 200);
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if(other.CompareTag("Ground2"))
        {
            isGrounded = true;
            maxSpeed = 15f;
        }
        else if (other.CompareTag("END"))
        {
            camControlScript.gameLive = false;
            end = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (other.CompareTag("Ground2"))
        {
            isGrounded = true;
            maxSpeed = 5f;
        }
    }

}

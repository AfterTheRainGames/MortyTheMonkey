using UnityEngine;

public class Grapple : MonoBehaviour
{

    [Header("References")]
    private LineRenderer grapple; //Grapple Itself
    public Transform grappleSpawn;
    private Transform cam;
    private Transform player;
    public LayerMask grabLayer;
    public CamControl camControlScript;

    [Header("Swinging")]
    private float grappleRange = 20f;
    private Vector3 grappleAttach;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;

    [Header("Input")]
    private KeyCode swingKey = KeyCode.Mouse0;
    private KeyCode pullKey = KeyCode.Mouse1;

    [Header("Customs")]
    private float maxSwing;
    private float minSwing;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    private float predictionSphereCastRadius = 2f;
    public Transform predictionPoint;

    public bool isGrappling = false;

    public float spring;

    public AudioSource audioSource;
    public AudioClip latch;



    void Start()
    {
        player = GetComponent<Transform>();
        cam = Camera.main.transform;
        grapple = GetComponent<LineRenderer>();
        predictionPoint.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isGrappling)
        {
            if(Input.GetKeyDown(swingKey))
            {
                maxSwing = .2f;
                minSwing = .1f;
                spring = 2f;
                StartGrapple();
            }
            else if(Input.GetKeyDown(pullKey))
            {
                maxSwing = .1f;
                minSwing = .01f;
                spring = 5f;
                StartGrapple();
            }
        }

        if (Input.GetKeyUp(swingKey) || Input.GetKeyUp(pullKey))
        {
            StopGrapple();
        }

        CheckForSwingPoints();

    }

    void LateUpdate()
    {
        DrawRope();

        if (isGrappling)
        {
            grappleSpawn.LookAt(grappleAttach);
        }
        else
        {
            grappleSpawn.LookAt(predictionPoint);
        }
    }


    void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, grappleRange, grabLayer);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, grappleRange, grabLayer);

        Vector3 realHitPoint;

        //Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;
        //Radius Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;
        //Miss
        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
            camControlScript.hand.gameObject.SetActive(true);
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
            camControlScript.hand.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;

    }

    void StartGrapple()
    {

        if (predictionHit.point == Vector3.zero) return;
        {

            audioSource.PlayOneShot(latch);

            isGrappling = true;

            grappleAttach = predictionHit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grappleAttach;

            float distanceFromAttach = Vector3.Distance(player.position, grappleAttach);

            joint.maxDistance = distanceFromAttach * maxSwing;
            joint.minDistance = distanceFromAttach * minSwing;

            joint.spring = spring;
            joint.damper = 1f;
            joint.massScale = 4.5f;

            grapple.positionCount = 2;
            currentGrapplePosition = grappleSpawn.position;
        }

    }

    void StopGrapple()
    {

        isGrappling = false;
        grapple.positionCount = 0;
        Destroy(joint);

    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grappleAttach, Time.deltaTime * 8f);

        grapple.SetPosition(0, grappleSpawn.position);
        grapple.SetPosition(1, grappleAttach);
    }
}
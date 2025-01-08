using UnityEngine;
using TMPro;

public class CamControl : MonoBehaviour
{

    private Camera cam;
    private Transform camPos;
    public Transform introPlayer;
    public Transform player;
    public Transform hand;
    public Transform mom;
    public Transform dad;
    public GameObject baby;
    public Transform babyCam;
    public Transform snake;

    public Animator animatorParent1;
    public Animator animatorParent2;
    public Animator animatorSnake;

    public Transform endPos;

    public Movement movementScript;
    public MainMenu mainMenuScript;

    public bool intro;
    public bool gameLive;
    public bool moveCam;

    private float offsetX = 2;
    private float offsetY = 2;
    private float offsetZ = 1;

    public float sens = 1f;
    private float xRotation = 0f;

    private bool timerOn = false;
    private float timerTime = 0f;
    public TextMeshProUGUI timerText;

    void Start()
    {
        cam = Camera.main;
        camPos = cam.transform;
        intro = true;
        gameLive = false;
        camPos.position = new Vector3(introPlayer.position.x + offsetX, introPlayer.position.y + offsetY, introPlayer.position.z + offsetZ);
        hand.gameObject.SetActive(false);
    }

    void Update()
    {

        if(intro)
        {
            camPos.LookAt(mom);
        }
        else if(gameLive)
        {
            camPos.position = player.position; 
            camPos.localRotation = Quaternion.LookRotation(player.forward);
            CameraRotation();
            mom.position = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z - 1f);
            mom.rotation = Quaternion.Euler(0, 270, 0);
            dad.position = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z + 1f);
            dad.rotation = Quaternion.Euler(0, 270, 0);
            baby.transform.position = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z);
            snake.transform.position = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z);
            baby.SetActive(false);
            animatorParent1.Play("Idle_A");
            animatorParent2.Play("Idle_A");
            timerOn = true;
        }
        else if(movementScript.end)
        {
            camPos.position = new Vector3(endPos.position.x - 4, endPos.position.y + 1, endPos.position.z);
            camPos.rotation = Quaternion.Euler(0, 90, 0);
            player.gameObject.SetActive(false);
            baby.SetActive(true);
            animatorParent1.Play("Spin");
            animatorParent2.Play("Spin");
            animatorSnake.Play("Roll");
            timerOn = false;
        }

        if(moveCam)
        {
            hand.SetParent(camPos);
            hand.localPosition = new Vector3(.2f, -.1f, .2f); // Adjust these values as needed
            hand.localRotation = Quaternion.identity;
            moveCam = false;
        }

        if(timerOn)
        {
            timerTime += Time.deltaTime;
            timerText.text = timerTime.ToString("F3");
        }

    }
    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens;
        float mouseY = Input.GetAxis("Mouse Y") * sens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camPos.localRotation = Quaternion.Euler(xRotation, camPos.localRotation.eulerAngles.y, camPos.localRotation.eulerAngles.z);

        player.Rotate(Vector3.up * mouseX);
    }
}

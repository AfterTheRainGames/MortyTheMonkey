using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public CamControl camScript;
    public TextureScroller textureScript;
    public Movement movementScript;
    public MomCheck momScript;

    public TextMeshProUGUI canvasText;
    public Button start;
    public bool cutscene = false;
    public bool cutsceneDone = false;
    public bool gameStart = false;
    public bool gameEnd = true;

    public Transform mom;
    public Transform dad;

    public Image blackUI;
    public TextMeshProUGUI talk;
    public RawImage monkey;
    public RawImage snake;

    public AudioSource audioSource;
    public AudioSource audioSource2;

    public AudioClip bonk;
    public AudioClip endMusic;
    public AudioClip music;

    void Start()
    {

        blackUI.gameObject.SetActive(false);
        monkey.gameObject.SetActive(false);
        snake.gameObject.SetActive(false);
        start.onClick.AddListener(startButton);

    }

    // Update is called once per frame
    void Update()
    {

        if(cutscene)
        {
            mom.position = new Vector3(mom.position.x, mom.position.y, mom.position.z + .02f);
            dad.position = new Vector3(dad.position.x, dad.position.y, dad.position.z + .02f);
        }

        else if(momScript.cutsceneDone)
        {
            StartCoroutine(HandleGameStart());
        }

        else if(gameStart)
        {
            camScript.gameLive = true;
            camScript.moveCam = true;
            movementScript.controlGiven = true;
            camScript.intro = false;
            StartCoroutine(Talking());
            gameStart = false;
            audioSource2.Play();
        }

        if(movementScript.end && gameEnd)
        {
            audioSource2.Stop();
            audioSource.PlayOneShot(endMusic);
            gameEnd = false;
            talk.text = "Morty has made it home!";
        }

    }

    void startButton()
    {

        canvasText.enabled = false;
        start.gameObject.SetActive (false);
        cutscene = true;

        textureScript.scrollSpeed = 0f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator HandleGameStart()
    {
        momScript.cutsceneDone = false;
        blackUI.gameObject.SetActive(true);
        audioSource.PlayOneShot(bonk);
        gameStart = true;
        yield return new WaitForSeconds(2);
        blackUI.gameObject.SetActive(false);
    }

    IEnumerator Talking()
    {
        yield return new WaitForSeconds(2);
        talk.text = "Hey I'm Snake. You fell off your parents and they didnt notice";
        snake.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        snake.gameObject.SetActive(false);
        talk.text = "Hey I'm Morty. I dont know how to get home can you help me";
        monkey.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        monkey.gameObject.SetActive(false);
        talk.text = "Sure! We can use my long body to grab onto branches to get you home. Where are you from?";
        snake.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        snake.gameObject.SetActive(false);
        talk.text = "I'm FROM THE TOP of the large tree";
        monkey.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        monkey.gameObject.SetActive(false);
        talk.text = "";
    }

}

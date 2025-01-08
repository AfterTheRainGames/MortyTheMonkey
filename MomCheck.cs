using UnityEngine;

public class MomCheck : MonoBehaviour
{
    public bool cutsceneDone = false;
    public MainMenu mmScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("T"))
        {
            cutsceneDone = true;
            mmScript.cutscene = false;
        }
    }
}

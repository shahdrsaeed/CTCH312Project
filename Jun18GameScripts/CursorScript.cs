using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public GameObject mainControl;
	MainScreenScript mainScript;

    void Start()
    {
        mainScript = mainControl.GetComponent<MainScreenScript>();
    }

   void OnTriggerEnter(Collider trigger)
   {
	if(trigger.name == "StartButton") {
		mainScript.touchStart = true;
	}
	if(trigger.name == "CreditsButton") {
		mainScript.touchCredits = true;
	}
	if(trigger.name == "ExitButton") {
		mainScript.touchExit = true;
	}
	if(trigger.name == "EasyButton") {
		mainScript.touchEasy = true;
	}
	if(trigger.name == "NormButton") {
		mainScript.touchNorm = true;
	}
	if(trigger.name == "HardButton") {
		mainScript.touchHard = true;
	}
   }

   void OnTriggerExit(Collider trigger)
   {
	if(trigger.name == "StartButton") {
		mainScript.touchStart =  false;
	}
	if(trigger.name == "CreditsButton") {
		mainScript.touchCredits = false;
	}
	if(trigger.name == "ExitButton") {
		mainScript.touchExit = false;
	}
	if(trigger.name == "EasyButton") {
		mainScript.touchEasy = false;
	}
	if(trigger.name == "NormButton") {
		mainScript.touchNorm = false;
	}
	if(trigger.name == "HardButton") {
		mainScript.touchHard = false;
	}
   }
}

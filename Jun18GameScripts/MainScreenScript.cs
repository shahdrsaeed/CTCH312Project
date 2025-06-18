using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenScript : MonoBehaviour
{
	//Scene thisScene;
	public GameObject mainScreenHolder;
	bool stillLoading = true;

	//Scene shipScene;
	//Scene tutorialScene;
	//Scene planet2Scene;
	//Scene planet3Scene;
	GameObject tutorialHolder;
	GameObject planet2Holder;
	GameObject planet3Holder;
	GameObject shipHolder;

	public GameObject cursor;
	public float mouseXSensitivity;
	public float mouseYSensitivity;
	Vector3 cursorSpeed;
	float cursorX = 0;
	float cursorY = 0;
	public float cursorXLimit = 5;
	public float cursorYLimit = 5;

	public bool touchStart = false;
	bool isPlaying = false;
	public bool touchCredits = false;
	bool viewingCredits = false;
	public GameObject creditsObject;
	public bool touchExit = false;

	public int[] numGems;

	public int difficulty = 1;		//0 = easy, 1 = normal, 2 = hard
	public GameObject selectEasy;
	public bool touchEasy = false;
	public GameObject selectNorm;
	public bool touchNorm = false;
	public GameObject selectHard;
	public bool touchHard = false;

	GameObject inventoryCanvas;
	public GameObject resumeButton;

	bool hasWon = false;
	public GameObject winScreen;
	public bool isAlive = true;

	public int currentWorld = -1;		//-1 is ship, 0 is planet 1, 1 is planet 2, etc.

	public Material blackSky;
	public Material atmoSky;
	public Material starSky;

    void Start()
    {
	numGems = new int[3];
	for(int i = 0; i < 3; i++) {
		numGems[i] = 0;
	}

	//thisScene = SceneManager.GetSceneByName("MainScreen");
	//shipScene = SceneManager.GetSceneByName("ShipScene");
	//tutorialScene = SceneManager.GetSceneByName("tutorialScene");
	//planet2Scene = SceneManager.GetSceneByName("Planet2Scene");
	//planet3Scene = SceneManager.GetSceneByName("Planet3Scene");
	tutorialHolder = GameObject.Find("TutorialHolder");
	planet2Holder = GameObject.Find("Planet2Holder");
	planet3Holder = GameObject.Find("Planet3Holder");
	shipHolder = GameObject.Find("ShipSceneHolder");

	inventoryCanvas = GameObject.Find("InventoryCanvas");

	Cursor.lockState = CursorLockMode.Locked;		//Not working?
	Cursor.visible = false;
    }

    void Update()
    {
	if(stillLoading) {
		stillLoading = false;
        	tutorialHolder.SetActive(false);
		planet2Holder.SetActive(false);
		planet3Holder.SetActive(false);
		shipHolder.SetActive(false);
		inventoryCanvas.SetActive(false);
	}
	else {
		if(isPlaying) {
			if(Input.GetKeyUp(KeyCode.Escape) && isAlive) {
				if(hasWon) { EndGame(); }
				else {
					isPlaying = false;
					inventoryCanvas.SetActive(false);
					mainScreenHolder.SetActive(true);
					//SceneManager.SetActiveScene(thisScene);
					RenderSettings.skybox = blackSky;
					switch(currentWorld)
					{
					case 0:
						tutorialHolder.SetActive(false);
						break;
					case 1:
						planet2Holder.SetActive(false);
						break;
					case 2:
						planet3Holder.SetActive(false);
						break;
					default:
						shipHolder.SetActive(false);
						break;
					}
				}
			}
		}
		else {
			if(Input.GetMouseButtonDown(0)) {
				if(viewingCredits) {
					viewingCredits = false;
					creditsObject.SetActive(false);
				}
				else {
					if(touchStart) {
						isPlaying = true;
						resumeButton.SetActive(true);
						switch(currentWorld)
						{
						case 0:
							tutorialHolder.SetActive(true);
							//SceneManager.SetActiveScene(tutorialScene);
							RenderSettings.skybox = atmoSky;
							break;
						case 1:
							planet2Holder.SetActive(true);
							//SceneManager.SetActiveScene(planet2Scene);
							RenderSettings.skybox = atmoSky;
							break;
						case 2:
							planet3Holder.SetActive(true);
							//SceneManager.SetActiveScene(planet3Scene);
							RenderSettings.skybox = starSky;
							break;
						default:
							shipHolder.SetActive(true);
							//SceneManager.SetActiveScene(shipScene);
							RenderSettings.skybox = blackSky;
							break;
						}
						inventoryCanvas.SetActive(true);
						mainScreenHolder.SetActive(false);
					}
					else if(touchCredits) {
						viewingCredits = true;
						creditsObject.SetActive(true);
					}
					else if(touchExit) { EndGame(); }

					else if(touchEasy) {
						selectEasy.SetActive(true);
						selectNorm.SetActive(false);
						selectHard.SetActive(false);
						difficulty = 0;		//tell levels
					}
					else if(touchNorm) {
						selectEasy.SetActive(false);
						selectNorm.SetActive(true);
						selectHard.SetActive(false);
						difficulty = 1;
					}
					else if(touchHard) {
						selectEasy.SetActive(false);
						selectNorm.SetActive(false);
						selectHard.SetActive(true);
						difficulty = 2;
					}

				}
			}
			cursorX += Input.GetAxis("Mouse X")*mouseXSensitivity;
			if(cursorX > cursorXLimit) { cursorX = cursorXLimit; }
			else if(cursorX < -cursorXLimit) { cursorX = -cursorXLimit; }
			cursorY += Input.GetAxis("Mouse Y")*mouseYSensitivity;
			if(cursorY > cursorYLimit - 10) { cursorY = cursorYLimit - 10; }
			else if(cursorY < -cursorYLimit - 10) { cursorY = -cursorYLimit - 10; }
			cursor.transform.position = new Vector3(cursorX, cursorY, 5); 
		}
	}
	if(Input.GetMouseButtonUp(0)) {					//Cursor Lock
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	if(Input.GetKeyUp(KeyCode.P)) {					//Cursor Unlock
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
    }

   public void EndGame() { Application.Quit(); }	//UnityEditor.EditorApplication.isPlaying = false;

   public void WinGame() {
	hasWon = true;
	mainScreenHolder.SetActive(true);
	//SceneManager.SetActiveScene(thisScene);
	RenderSettings.skybox = blackSky;
	shipHolder.SetActive(false);
	winScreen.SetActive(true);
   }

}

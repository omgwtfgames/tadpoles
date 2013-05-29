using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public enum State { Playing, Paused, Menu, GameOver };
	public State state = State.Menu;
	
	public GameObject tadpolePrefab;
	public GUISkin guiSkin;
	public int startButtonWidth;
	public GameObject title;
	public GameObject restartButton;
	
	public Color[] tadpoleColors;
	public int[] tadpoleValues;
	public float leftOfScreen;
	public float rightOfScreen;
	public static int[] playerScores;
	
	float levelTime = 60f;
	public static float countdown;
	
	Vector3 lastSpawnPos = new Vector3(999f,999f,999f);
	
	void Awake() {
		playerScores = new int[] {0, 0};
		//tadpoleValues = new int[tadpoleColors.Length];
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Menu) {
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
				StartGame();
			}
		}
	}
	
	void FixedUpdate() {
		if (state == State.Playing) {
			countdown -= Time.deltaTime;
			if (countdown <= 0f) {
				GameOver();
			}
		}
	}
	
	public static void AddScore(int playerId, int score) {
		playerScores[playerId - 1] += score;
	}
	
	void SpawnTadpole() {
		GameObject go = Instantiate(tadpolePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
		Tadpole t = go.GetComponent<Tadpole>();
		int vi = Random.Range(0, tadpoleColors.Length);
		t.SetFinColor(tadpoleColors[vi]);
		t.tadpoleValue = tadpoleValues[vi];
		//Debug.Log(vi);
		t.speed = (t.tadpoleValue / 2) + 10f; //Random.Range(5f, 50f); // 
		Vector3 pos;
		if (Random.value < 0.5f) {
			t.direction = new Vector3(1f, 0f, 0f);
			pos = new Vector3(leftOfScreen, Random.Range(-10f, 40f), 0f);
			while (Vector3.Distance(lastSpawnPos, pos) < 10) {
				pos = new Vector3(leftOfScreen, Random.Range(-10f, 40f), 0f);
			}
			t.transform.position = pos;
			t.transform.Rotate(new Vector3(0f, 180f, 0f));
		} else {
			t.direction = new Vector3(-1f, 0f, 0f);
			pos = new Vector3(rightOfScreen, Random.Range(-10f, 40f), 0f);
			while (Vector3.Distance(lastSpawnPos, pos) < 10) {
				pos = new Vector3(rightOfScreen, Random.Range(-10f, 40f), 0f);
			}
			t.transform.position = pos;
		}			
		lastSpawnPos = new Vector3(pos.x, pos.y, pos.z);
	}
	
	void OnGUI() {
		GUI.skin = guiSkin;
		int labelWidth = 200;
		if (state == State.Playing) {
			GUI.Label(new Rect(labelWidth - 150, 0, labelWidth, 110), "$"+playerScores[0], guiSkin.customStyles[0]);
			GUI.Label(new Rect(Screen.width - labelWidth - 50, 0, labelWidth, 110), "$"+playerScores[1], guiSkin.customStyles[1]);
			GUI.Label(new Rect((Screen.width - labelWidth)/ 2, 0, labelWidth, 110), Mathf.Round(countdown).ToString());
		}
		if (state == State.Menu) {
			bool startClicked = GUI.Button (new Rect( (Screen.width - startButtonWidth)/2, Screen.height - 200, startButtonWidth, 150), "", guiSkin.customStyles[2]);
			if (startClicked) {
				StartGame();
			}
		}
		if (state == State.GameOver) {
			GUI.Label(new Rect(labelWidth - 150, 0, labelWidth, 110), "$"+playerScores[0], guiSkin.customStyles[0]);
			GUI.Label(new Rect(Screen.width - labelWidth - 50, 0, labelWidth, 110), "$"+playerScores[1], guiSkin.customStyles[1]);
			string message = "";
			if (playerScores[0] > playerScores[1]) {
				message = "Red wins !";
				StartCoroutine(ShowRestartButton());
			} else if (playerScores[1] > playerScores[0]) {
				message = "Blue wins !";
				StartCoroutine(ShowRestartButton());
			} else if (playerScores[0] == playerScores[1]) {
				message = "It's a tie - extra time !";
				StartCoroutine(DoExtraTime());
			}
			GUI.Label(new Rect((Screen.width - labelWidth*2)/ 2, 100, labelWidth*2, 110), message);
		}
	}
	
	public void StartGame() {
		state = State.Playing;
		restartButton.SetActive(false);
		title.SetActive(false);
		playerScores[0] = 0;
		playerScores[1] = 0;
		
		// remove fish from hook from last game
		GameObject.Find("HookP1").transform.position = new Vector3(-40f, 40f, 0f);
		GameObject.Find("HookP2").transform.position = new Vector3(40f, 40f, 0f);
		GameObject onHookP1 = GameObject.Find("HookP1").GetComponent<Hook>().onHook;
		GameObject onHookP2 = GameObject.Find("HookP2").GetComponent<Hook>().onHook;
		if (onHookP1 != null) {
			Destroy(onHookP1);
		}
		if (onHookP2 != null) {
			Destroy(onHookP2);
		}
		
		countdown = levelTime;
		InvokeRepeating("SpawnTadpole", 1.0f, 1.0f);
	}
	
	void GameOver() {
		state = State.GameOver;
	}
	
	IEnumerator DoExtraTime() {
		yield return new WaitForSeconds(1.5f);
		countdown = 10f;
		state = State.Playing;
	}
	
	IEnumerator ShowRestartButton() {
		yield return new WaitForSeconds(1.5f);
		restartButton.SetActive(true);
	}	
}
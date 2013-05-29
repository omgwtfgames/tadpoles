using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {
	
	public int player = 1;
	float downSpeed = 30f;
	float upSpeed = 30f;
	float loadedUpSpeed = 15f;
	float lrSpeed = 30f;
	public bool striking = false;
	public GameObject onHook;
	public Transform anchorPoint;
	LineRenderer lineRenderer;
	GameManager gm;
	
	float strikeCounter = 0f;
	
	void Awake () {
		gm = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		
		lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, new Vector3(anchorPoint.position.x, 70f, 0f));

	}
	
	// Use this for initialization
	void Start () {
		if (player == 1) {
			lineRenderer.renderer.material.color = Color.red;
		}
		if (player == 2) {
			lineRenderer.renderer.material.color = Color.blue;
		}
	}
	
	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition(0, new Vector3(anchorPoint.position.x, 70f, 0f));
		lineRenderer.SetPosition(1, anchorPoint.position);
		
		if (gm.state == GameManager.State.Playing) {
			if (player == 2) {
				if (Input.GetKey(KeyCode.DownArrow)) {
					goDown();
				}
				if (Input.GetKey(KeyCode.UpArrow)) {
					goUp();
				}
				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					striking = true;
				}
				if (Input.GetKeyUp (KeyCode.UpArrow)) {
					strikeCounter = 0f;
					striking = false;
				}
				if (Input.GetKey(KeyCode.LeftArrow)) {
					goLeft();
				}
				if (Input.GetKey(KeyCode.RightArrow)) {
					goRight();
				}
			}
			if (player == 1) {
				if (Input.GetKey(KeyCode.S)) {
					goDown();
				}
				if (Input.GetKey(KeyCode.W)) {
					goUp();
				}
				if (Input.GetKeyDown(KeyCode.W)) {
					striking = true;
				}
				if (Input.GetKeyUp (KeyCode.W)) {
					strikeCounter = 0f;
					striking = false;
				}
				if (Input.GetKey(KeyCode.A)) {
					goLeft();
				}
				if (Input.GetKey(KeyCode.D)) {
					goRight();
				}
			}
		}
		// keep hook within bounds
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -80f, 80f),
			                             Mathf.Clamp(transform.position.y, -40f, 50f),
			                             transform.position.z);
	}
	
	void goDown() {
		transform.position += new Vector3(0f, -downSpeed*Time.deltaTime, 0f);
	}
	
	void goUp() {
		if (onHook == null) {
			strikeCounter += Time.deltaTime;
			float strikeSpeedup = 1.0f;
			if (strikeCounter < 0.04f) {
				strikeSpeedup = 5f;
			} else {
				striking = false;
			}
			transform.position += new Vector3(0f, strikeSpeedup*upSpeed*Time.deltaTime, 0f);
		} else {
			transform.position += new Vector3(0f, loadedUpSpeed*Time.deltaTime, 0f);
		}
	}
	
	void goLeft() {
		transform.position += new Vector3(-lrSpeed*Time.deltaTime, 0f, 0f);
	}
	
	void goRight() {
		transform.position += new Vector3(lrSpeed*Time.deltaTime, 0f, 0f);
	}
}

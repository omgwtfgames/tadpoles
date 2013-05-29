using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {
	GameManager gm;
	// Use this for initialization
	void Start () {
		gm = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = gm.guiSkin;
		int labelWidth = 200;
		if (gm.state == GameManager.State.GameOver) {
			if (GUI.Button(new Rect((Screen.width - labelWidth*4)/ 2, 200, labelWidth*4, 110), "", gm.guiSkin.customStyles[3])) {
				gm.StartGame();
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using SmoothMoves;

public class Weed : MonoBehaviour {
	public string animationToPlay = "Wave";
	BoneAnimation boneAnimation;
	
	void Awake() {
		boneAnimation = GetComponent<BoneAnimation>();
		animationToPlay = "Wave" + Random.Range(1, 3);
		// use reversed animation for flipped weed sprites
		if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 180) {
			animationToPlay += "R";
		}
		boneAnimation.Play(animationToPlay);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

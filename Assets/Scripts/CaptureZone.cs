using UnityEngine;
using System.Collections;

public class CaptureZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.CompareTag("Fish")) {
			StartCoroutine(c.GetComponent<Tadpole>().Die());
		}
	}
}

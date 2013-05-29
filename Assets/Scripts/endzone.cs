using UnityEngine;
using System.Collections;

public class endzone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.CompareTag("Fish")) {
			Destroy(c.gameObject);
		}
	}
}

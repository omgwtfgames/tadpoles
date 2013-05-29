using UnityEngine;
using System.Collections;
using SmoothMoves;

public class Tadpole : MonoBehaviour {
	
	public GameObject tadpoleAnimPrefab;
	public int tadpoleValue = 0;
	GameObject tadpoleAnim;
	public Vector3 tadpoleAnimOffset = new Vector3(2.19f, -19.2f, 0f);
	BoneAnimation boneAnimation;
	public enum State { Moving, Caught, Dying };
	public State state = State.Moving;
	public Vector3 direction = new Vector3(-1f, 0f, 0f);
	public float speed = 10f;
	public int caughtByPlayer;
	
	void Awake() {
		tadpoleAnim = Instantiate(tadpoleAnimPrefab, 
			                                 transform.position+tadpoleAnimOffset, 
			                                 Quaternion.identity) as GameObject;
		tadpoleAnim.transform.parent = transform;
		boneAnimation = tadpoleAnim.GetComponent<BoneAnimation>();
		boneAnimation.Play("Swim");
	}
	
	// Use this for initialization
	void Start () {
		//SetFinColor(Color.cyan);
		//SetFinColor(new Color(Random.value, Random.value, Random.value, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Moving) {
			transform.position += direction * speed * Time.deltaTime;
		}
	}
	
	//void OnTriggerEnter(Collider c) { // we use Stay instead for striking
	void OnTriggerStay(Collider c) {
		if (c.CompareTag("Hook")) {
			Hook hook = c.GetComponent<Hook>();
			if (hook.onHook == null && hook.striking) {
				hook.onHook = gameObject;
				caughtByPlayer = hook.player;
				StartCoroutine(Caught(hook));
			}
		}
	}
	
	IEnumerator Caught(Hook hook) {
		if (state == State.Caught) yield break;
		state = State.Caught;
		transform.parent = hook.transform;
		SoundManager.Instance.Play("stab");
		iTween.RotateBy(tadpoleAnim, new Vector3(0, 0, -0.25f), 0.5f);
		//iTween.MoveTo(gameObject, hook.transform.position + new Vector3(0f, 16.5f, 0f), 0.5f);
		transform.position = hook.transform.position + new Vector3(0f, 16.5f, 0f);
		boneAnimation.CrossFade("Dying");
		yield return new WaitForSeconds(0.5f);
		//boneAnimation.CrossFade("Dying");

	}
	
	public void SetFinColor(Color c) {
		//boneAnimation.SetMeshColor(c);
		boneAnimation.SetBoneColor("Fin", c, 1.0f);
	}
	
	public IEnumerator Die() {
		if (state == State.Dying) yield break;
		state = State.Dying;
		GameManager.AddScore(caughtByPlayer, tadpoleValue);
		transform.parent = null;
		int side = caughtByPlayer == 1 ? -1 : 1;
		SoundManager.Instance.Play("coin");
		iTween.MoveTo(gameObject, new Vector3(side*100f, transform.position.y+5f, transform.position.z), 1f);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}

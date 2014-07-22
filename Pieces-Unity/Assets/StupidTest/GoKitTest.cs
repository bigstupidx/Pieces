using UnityEngine;
using System.Collections;

public class GoKitTest : MonoBehaviour {
	public GameObject obj;
	public Vector3 initialPos;

	// Use this for initialization
	void Start () {
		initialPos = obj.transform.position;
		Go.defaultEaseType = GoEaseType.SineInOut;
		StartCoroutine(Play());
	}

	IEnumerator Play() {
		GoTween tween = Go.to (obj.transform, 1f, new GoTweenConfig().vector3Prop("position", new Vector3(50, 50, 0), true));
		yield return StartCoroutine(tween.waitForCompletion());

		tween = Go.to (obj.transform, 0.5f, new GoTweenConfig().eulerAngles(new Vector3(0, 0, 135), true));
		yield return StartCoroutine(tween.waitForCompletion());

		obj.GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.5f);

		tween = Go.to (obj.transform, 1.5f, new GoTweenConfig()
		               .shake(new Vector3(25, 25, 0), GoShakeType.Position)
		               .shake(new Vector3(0, 0, 90), GoShakeType.Eulers)
		               .shake(new Vector3(0.5f, 5, 0), GoShakeType.Scale, 2));
		yield return StartCoroutine(tween.waitForCompletion());

		Debug.Log("done!");
	}

	// Update is called once per frame
	void Update () {
	
	}
}

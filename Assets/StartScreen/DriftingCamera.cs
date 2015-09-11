using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class DriftingCamera : MonoBehaviour {

  public float transitionDuration = 10f;
  public float xMin = 0f;
  public float xMax = 1f;
  public float yMin = 0f;
  public float yMax = 1f;

  private Vector3 target;
  private bool moving = false;

  IEnumerator Transition() {
    this.moving = true;
    float t = 0.0f;
    Vector3 startingPos = transform.position;
    while (t < 1.0f) {
      t += Time.deltaTime * (Time.timeScale/transitionDuration);

      transform.position = Vector3.Lerp(startingPos, target, t);
      yield return 0;
    }

    this.moving = false;
  }

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if (!moving) {
      float x = Random.Range(this.xMin, this.xMax);
      float y = Random.Range(this.yMin, this.yMax);
      this.target = new Vector3(x, y, -10f);
      StartCoroutine(Transition());
    }
	}
}

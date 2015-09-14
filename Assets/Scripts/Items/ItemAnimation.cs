using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAnimation : MonoBehaviour {
  public float duration = 0.5f;
  public float moveSpeed = .05f;
  public string infoText = "";

  private float elapsed = 0;

  // Use this for initialization
  void Start () {

    Text display = this.GetComponentInChildren<Text> ();

    display.text = infoText;
    display.color = new Color(255, 100, 0, 255);
    display.CrossFadeAlpha (0, this.duration, false);
  }

  // Update is called once per frame
  void Update () {
    this.elapsed += Time.deltaTime;

    this.transform.Translate (0, (float) (Mathf.Sqrt (this.elapsed - Time.deltaTime) * this.moveSpeed), 0);

    if (this.elapsed > this.duration) {
      Destroy (this.gameObject);
    }
  }
}

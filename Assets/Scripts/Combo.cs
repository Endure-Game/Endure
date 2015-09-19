using UnityEngine;
using System.Collections;

public class Combo {
	private KeyCode[] combo;
	private int index;

	public Combo(KeyCode[] code) {
		this.combo = code;
	}

	public bool GetCombo () {
		if (Input.GetKeyDown (this.combo [this.index])) {
			this.index++;

			if (this.index == this.combo.Length) {
				this.index = 0;
				return true;
			}
		} else if (Input.anyKeyDown) {
			this.index = 0;
		}

		return false;
	}
}

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class BiomeChange : MonoBehaviour {
	public RoomManager roomManager;

	public AudioMixerSnapshot none;
	public AudioMixerSnapshot mountain;
	public AudioMixerSnapshot beach;
	public AudioMixerSnapshot plains;
	public AudioMixerSnapshot desert;
	public AudioMixerSnapshot forest;
	public AudioMixerSnapshot snow;

	public float transitionTime = 3;

	private int current = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int biome = this.GetCurrentBiome ();

		if (biome != current) {
			if (this.current == -1) {
				this.Play (biome);
			} else {
				this.TransitionTo (biome);
			}

			this.current = biome;
		}
	}

	int GetCurrentBiome ()
	{
		var roomWidth = this.roomManager.columns;
		var roomHeight = this.roomManager.rows;

		var pos = PlayerController.instance.transform.position + new Vector3 (roomWidth / 2, roomHeight / 2);

		int x = (int) pos.x;
		int y = (int) pos.y;

		try {
			return this.roomManager.tileMap[x, y].biome;
		} catch (UnityException e) {
			return -1;
		}
	}
	
	void Play (int biome)
	{
		BiomeSnapshot (biome).TransitionTo (0);
	}

	void TransitionTo (int biome)
	{
		BiomeSnapshot (biome).TransitionTo (this.transitionTime);
	}

	AudioMixerSnapshot BiomeSnapshot (int biome) {
		switch (biome) {
		case 0:
			return forest;
		case 1:
			return desert;
		case 2:
			return plains;
		case 3:
			return mountain;
		case 4:
			return snow;
		case 5:
			return beach;
		default:
			return none;
		}
	}
}

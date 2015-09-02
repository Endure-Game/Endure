using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class BeachTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	
	public GameObject getGroundTile() {
		return this.groundTiles[Random.Range(0, this.groundTiles.Length)];
	}
	
	public GameObject getBlockingTile() {
		return this.blockingTiles[Random.Range(0, this.blockingTiles.Length)];
	}
}


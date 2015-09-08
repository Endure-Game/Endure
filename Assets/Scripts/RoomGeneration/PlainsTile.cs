using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlainsTile : BiomeTile
{

	public GameObject flatGroundTile;

	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);

	public const int BiomeNumber = 2;

	public override void RandomBlocking(List<Tile> region) {

		Destroy (this.tileHolder);
		// Place elevation tiles
		base.RandomBlocking(region);

		for (int num = 0; num < bloomNum; num++) {

			Tile randomTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(randomTile.x,
			                  randomTile.y,
			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1),
			                  new TilePlacer(this.placeBlockingTile));
		}
	}

	private GameObject tileHolder;
	public override GameObject getGroundTile() {

		if (this.tileHolder != null) {
			Destroy (this.tileHolder);
		}
		this.tileHolder = new GameObject();

		for (int i = 0; i < 4; i++) {
			GameObject randomSprite = this.groundTiles[Random.Range(0, this.groundTiles.Length)];
			GameObject tile = Instantiate(randomSprite,
																		new Vector3(0f, (float)i / 4, (float)i / 4),
		                               	Quaternion.identity) as GameObject;
			tile.transform.parent = this.tileHolder.transform;
		}

		return this.tileHolder;
	}

	public GameObject getFlatGroundTile() {
		return this.flatGroundTile;
	}

	public override int getBiomeNumber() {
		return 2;
	}
}
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SnowTile : BiomeTile
{
	public GameObject snowMan;
	public GameObject ice;
	public GameObject crackedIce;

	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);

	public int iceBloomNum = 2;
	public RoomManager.Count iceBloomSize = new RoomManager.Count(7, 8);

	public const int BiomeNumber = 4;

	public override void RandomBlocking(List<Tile> region) {

		base.RandomBlocking(region);

		// Place ice tiles
		for (var num = 0; num < iceBloomNum; num++) {
			Tile iceTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(iceTile.x,
			                  iceTile.y,
			                  Random.Range (iceBloomSize.minimum, iceBloomSize.maximum + 1),
			                  new TilePlacer(this.placeIceLakeTile));
		}

		// Place blocking tiles
		for (int num = 0; num < bloomNum; num++) {

			Tile randomTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(randomTile.x,
			                  randomTile.y,
			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1),
			                  new TilePlacer(this.placeBlockingTile));
		}

		// Place snow man
		if (Random.Range (0, 1) < .5) {
			Tile snowManTile = region[Random.Range(0, region.Count)];
			while (snowManTile.item != null) {
				snowManTile = region[Random.Range(0, region.Count)];
			}
			this.GetComponent<RoomManager>().PlaceItem(snowMan, snowManTile.x, snowManTile.y);
		}
	}

	public void placeIceLakeTile(int x, int y) {
		GameObject sprite;
		if (Random.Range(0, 10) < 2)  {
			sprite = this.crackedIce;
		} else {
			sprite = this.ice;
		}
		this.GetComponent<RoomManager>().SetGroundTile(sprite, x, y);
	}

	public override int getBiomeNumber() {
		return 4;
	}
}

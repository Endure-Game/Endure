using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathFinding : MonoBehaviour {

	public GameObject world;
	public int rows;
	public int columns;
	private RoomManager roomManager;
	

	public void Start (){
		this.roomManager = this.world.GetComponent<RoomManager> ();
		this.rows = roomManager.rows;
		this.columns = roomManager.columns;
	}

	public List<Vector3> PathFinder (Vector3 current, Vector3 destination) {

		List<Vector3> finalPath = new List<Vector3> ();

		while (Mathf.Round(current.x) != Mathf.Round (destination.x) && Mathf.Round(current.y) != Mathf.Round (destination.y)) {
			List<Vector3> posPaths = this.OpenTiles (current);
			Vector3 temp = current;
			temp.z = 10000f;
			for(int i = 0; i < posPaths.Count - 1; i++){
				posPaths[i] = distance(posPaths[i], destination);
				if(posPaths[i].z < temp.z){
					temp = posPaths[i];
				}
			}
			finalPath.Add(temp);
			current = temp;

		}
		return finalPath;
	}

	private Vector3 distance (Vector3 position, Vector3 endPosition){
		float x = position.x;
		float y = position.y;
		float endX = endPosition.x;
		float endY = endPosition.y;

		float distance = Mathf.Sqrt (Mathf.Pow(Mathf.Abs(x - endX),2) + Mathf.Pow(Mathf.Abs(y - endY),2));

		position.z = distance;

		return position;
	}

	private List<Vector3> OpenTiles (Vector3 currentPos) {

		float x = currentPos.x;
		float y = currentPos.y;
		Vector3 north;
		Vector3 northEast;
		Vector3 east;
		Vector3 southEast;
		Vector3 south;
		Vector3 southWest;
		Vector3 west;
		Vector3 northWest;

		if (y + 1f < columns - 2f && y + 1f > 0f) {
			north = new Vector3 (x, y + 1f, 0f);
		}else {
			north = new Vector3 (1f,1f,0f);
		}
		if (x + 1f < rows - 2f && y + 1f < columns - 2f && x + 1f > 0f && y + 1f > 0f) {
			northEast = new Vector3 (x + 1f, y + 1f, 0f);
		} else {
			northEast = new Vector3 (1f,1f,0f);
		}
		if (x + 1f < rows - 2f && x + 1f > 0f) {
			east = new Vector3 (x + 1f, y, 0f);
		}else {
			east = new Vector3 (1f,1f,0f);
		}
		if (x + 1f < rows - 2f && y - 1f < columns - 2f && x + 1f > 0f && y - 1f > 0f) {
			southEast = new Vector3 (x + 1f, y - 1f, 0f);
		}else {
			southEast = new Vector3 (1f,1f,0f);
		}
		if (y - 1f < columns - 2f && y - 1f > 0f) {
			south = new Vector3 (x, y - 1f, 0f);
		}else {
			south = new Vector3 (1f,1f,0f);
		}
		if (x - 1f < rows - 2f && y - 1f < columns - 2f && x - 1f > 0f && y - 1f> 0f) {
			southWest = new Vector3 (x - 1f, y - 1f, 0f);
		}else {
			southWest = new Vector3 (1f,1f,0f);
		}
		if (x - 1f < rows - 2f && x - 1f > 0f) {
			west = new Vector3 (x - 1f, y, 0f);
		}else {
			west = new Vector3 (1f,1f,0f);
		}
		if (x - 1f < rows - 2f && y + 1f < columns - 2f && x - 1f > 0f && y + 1f > 0f) {
			northWest = new Vector3 (x - 1f, y + 1f, 0f);
		}else {
			northWest = new Vector3 (1f,1f,0f);
		}

		Vector3[] possibleMoves = new Vector3[8] {north, northEast, east, southEast, south, southWest, west, northWest};
		List<Vector3> nonBlockedTiles = new List<Vector3>();

		for (int i = 0; i < possibleMoves.Length; i++) {
//			if(this.roomManager.checkForBlock(possibleMoves[i]) != false){
//				float tempX = possibleMoves[i].x;
//				float tempY = possibleMoves[i].y;
//				if (nonBlockedTiles.Count > 0){
//					for(int j = 0; j < nonBlockedTiles.Count - 1; j++){
//						if(nonBlockedTiles[j].x == tempX && nonBlockedTiles[j].y == tempY){
//							//do nothing
//						}else{
//							nonBlockedTiles.Add(possibleMoves[i]);
//						}
//					}
//				} else {
//					nonBlockedTiles.Add (possibleMoves[i]);
//				}
//
//					
//			}
			nonBlockedTiles.Add (possibleMoves[i]);
		}
		return nonBlockedTiles;

	}
	

}

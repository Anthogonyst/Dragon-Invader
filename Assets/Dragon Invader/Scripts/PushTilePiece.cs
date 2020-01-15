using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class PushTilePiece : MonoBehaviour {

	public float speed = 2f;
	public float time = 0f;
	public int necessaryActiveTiles = 3;
	public GameObject player;
	public GameObject firstTile;
	public GameObject[] tilePieces;
	public float cameraBounds = 5f;
	
	public int TileCount {
		get { return _count; }
		set { ; }
	}
	
	private bool gamePlaying = true;
	private Quaternion rot;
	private Queue<GameObject> activeScenes;
	private float interval = 0f;
	private int _count = 0;
	private bool unpaused = false;
	
	
	void Start() {
		if (necessaryActiveTiles == 0 || tilePieces.Length == 0) {
			Debug.Log("Error initializing due to zero value. " + ToString());
			Destroy(this);
		}
		
		for (int i = 0; i < tilePieces.Length; i++) {
			if (tilePieces[i].GetComponent<TilePieceController>() == null) {
				Debug.Log("Error initializing due to bad entry. " + ToString());
				Destroy(this);
			}
		}
		
		activeScenes = new Queue<GameObject>();
		rot = this.gameObject.transform.rotation;
		
		GameObject first = Instantiate(firstTile, Vector3.zero, Quaternion.identity, transform);
		activeScenes.Enqueue(first);
		interval += 2 * first.GetComponent<TilePieceController>().SpriteHeight;
		
		QueueNewScene();
		
		if (player != null)
			Instantiate(player, Vector3.zero, Quaternion.identity);
		
		StartCoroutine("ScrollScreen");
		time = 0f;
		unpaused = true;
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log(ToString());
		#endif
	}
	
	void LateUpdate() {
		if (unpaused) {
			if ((int) Mathf.Floor(time / 600) > _count) {
				unpaused = false;
				
				GameObject oldestScene = activeScenes.Peek();
				if (oldestScene.transform.position.y + oldestScene.GetComponent<TilePieceController>().SpriteHeight <= -cameraBounds) {
					_count++;
					QueueNewScene();
				}
				else {
					time -= 100f;
					unpaused = true;
				}
			}
		}
	}
	
	public void QueueNewScene() {
		int rand = Random.Range(0, tilePieces.Length);
		GameObject newScene = Instantiate(tilePieces[rand], transform.position, Quaternion.identity, transform);
		newScene.SetActive(false);
		
		TilePieceController oldestScene = activeScenes.Peek().GetComponent<TilePieceController>();
		TilePieceController upcomingScene = newScene.GetComponent<TilePieceController>();
		upcomingScene.IterationCount = _count;
		
		float dist = interval - oldestScene.SpriteHeight + upcomingScene.SpriteHeight;
		interval += 2 * upcomingScene.SpriteHeight;
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log("Adding component of height: " + upcomingScene.SpriteHeight);
		Debug.Log("Interval: " + interval + " | Piece Height: " + dist);
		#endif
		
		float finalRot = rot.eulerAngles.z % 360 == 0f ? 0f : Mathf.Cos(rot.eulerAngles.z % 360 * Mathf.Deg2Rad) / Mathf.Sin(rot.eulerAngles.z % 360 * Mathf.Deg2Rad);
		Vector3 finalPosition = new Vector3(oldestScene.SpriteCenter.x + dist * finalRot, 
								oldestScene.SpriteCenter.y + dist, 0);
		newScene.transform.position = finalPosition;
		newScene.SetActive(true);
		
		activeScenes.Enqueue(newScene);
		unpaused = true;
		
		if (activeScenes.Count > necessaryActiveTiles)
			DequeueOldScene();
	}
	
	private void DequeueOldScene() {
		TilePieceController oldestScene = activeScenes.Peek().GetComponent<TilePieceController>();
		interval -= 2 * oldestScene.SpriteHeight;
		activeScenes.Dequeue();
		Destroy(oldestScene.gameObject);
	}
	
	private IEnumerator ScrollScreen() {
		while (gamePlaying) {
			Vector3 moving = rot * (new Vector3(speed * Time.deltaTime, 0, 0));
			time += 1f;

			foreach (GameObject g in activeScenes) {
				g.transform.position += moving;
			}
			
			
			yield return new WaitForEndOfFrame();
		}
	}
	
/*	public override string ToString () {    //Return a string representing the class
		return string.Format ("***Class PushTilePiece*** Player: {0} | Speed: {1} | Time: {2} | Direction: {3} | Game Playing: {8} | # of Tile Pieces: {4} | # of Active Scenes: {5} | # of Actors: {6} | # of inactive objects: {7}",
				player.name, speed, time, rot, tilePieces.Length, activeScenes.Count, npcs.Count, garbageCollection.Count, gamePlaying); 
	}*/
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class TilePieceController : MonoBehaviour {
	
	public PushTilePiece tileSpawner;
	public TileHelper[] enemySpawners;
	public bool spawnEnemies = true;
	
	public int IterationCount { 
		get {
			if (hasTileControllerParented()) {
				_iterationCount = tileSpawner.TileCount;
			} else _iterationCount = 0;
			
			return _iterationCount;
		}
		set { _iterationCount = value; }
	}
	
	private Bounds _bounds;
	private float _height;
	private float _width;
	private Vector3 _dimensions;
	private int _iterationCount;

	// Use this for initialization
	public float SpriteHeight {
		get {
			return _height;
		}
		set { ; }
	}
	
	public float SpriteWidth {
		get {
			return _width;
		}
		set { ; }
	}
	
	public Vector3 SpriteCenter {
		get {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("Center of sprite in world space: " + transform.position);
			#endif
			return transform.position;
		}
		set { ; }
	}
	
	public Vector3 SpriteDimensions {
		get {
			return _dimensions;
		}
		set { ; }
	}
	
	void Awake() {
		if (GetComponent<SpriteRenderer>() != null) {
			_bounds = GetComponent<SpriteRenderer>().bounds;
			_height = _bounds.extents.y;
			_width = _bounds.extents.x;
			_dimensions = _bounds.size;
			
		} else {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log(ToString());
			#endif
		}
	}
	
	void Start() {
		hasTileControllerParented();
		enemySpawners = gameObject.GetComponentsInChildren<TileHelper>();
		
		if (spawnEnemies)
			Invoke("beginSpawning", 1f);
	}
	
	private bool beginSpawning() {
		foreach (TileHelper g in enemySpawners) {
			g.spawnEntities(_iterationCount);
		}
		if (enemySpawners.Length > 0)
			return true;
		else return false;
	}
	
	private bool hasTileControllerParented() {
		if (tileSpawner != null)
			return true;
		if (transform.parent != null) {
			if (transform.parent.GetComponent<PushTilePiece>() != null) {
				tileSpawner = transform.parent.GetComponent<PushTilePiece>();
				return true;
			}
		}
		
		GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
		foreach (GameObject obj in objs) {
			if (obj.GetComponent<PushTilePiece>() == null)
				continue;
			
			tileSpawner = obj.GetComponent<PushTilePiece>();
		}
		
		if (tileSpawner == null)
			return false;
		else return true;
	}
	
	public override string ToString () {    //Return a string representing the class
		if (GetComponent<SpriteRenderer>() == null) {
			return string.Format ("***Class TilePieceController*** Name: {0} | SpriteRenderer present: False",
				this.gameObject.name);
		}
		else return string.Format ("***Class TilePieceController*** Name: {0} | SpriteRenderer present: True",
				this.gameObject.name);
	}
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class Character : MonoBehaviour {
	
	#region Variables
	public int health = 3;
	public float speed = 5;
	public float sightRange = 5f;
	public GameObject[] items;
	public LayerMask bitmap;
	public GameObject enemyTarget, deathParticle;
	
	protected Transform eventManager;
	protected List<LessGenericList> activeStates;
	protected string[] stateNames = { "Stand", "Walk", "Attack", "Special", "Interrupt", "Die" };
	protected enum states { Idle, Walk, Attack, Special, Interrupt, Dead };
	protected enum activity { Movement, Action, Interrupted };
	protected bool busy, interrupt = false;
	
	protected bool debug = false;
	#endregion
	
	#region Initialization
	public void Start() {
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is initializing.");
		#endif
		
		findEventManager();
		
		activeStates = new List<LessGenericList>();
		
		for (int i = 0; i < stateNames.Length; i++) {
			LessGenericList lgl = new LessGenericList(stateNames[i]);
			activeStates.Add(lgl);
		}
		
		Coinflip();
	}
	#endregion
	
	#region State Progression and Randomizer
	protected bool Coinflip() {
		return Coinflip(activity.Movement);
	}
	
	protected bool Coinflip(activity parameter) {
		int goToState = 0;
		
		switch (parameter) {
			case activity.Movement: {
				if (!busy) {
					goToState = Random.Range(0, 2);
					return stateChange(goToState);
				} else stateChange(goToState);
				return true;
			}
			case activity.Action: {
				if (!busy || interrupt) {
					goToState = Random.Range(2, 4);
					return stateChange(goToState);
				} else stateChange(goToState);
				return true;
			}
			default: return false;
		}
	}
	#endregion
	
	#region Movement States
	protected virtual IEnumerator Stand() {
		bool _busy = false;
		bool _interrupt = true;
		checkStatus(_busy, _interrupt);
		
		float frames = 50;
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is idle.");
		#endif
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		finishExistingState("Stand");
		Coinflip(activity.Movement);
		Coinflip(activity.Action);
	}
	
	protected virtual IEnumerator Walk() {
		bool _busy = false;
		bool _interrupt = true;
		checkStatus(_busy, _interrupt);
		
		float frames = 50;
		float dir = Random.Range(0, 360);
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is walking.");
		#endif
		
		while (frames > 0) {
			Vector3 mov = new Vector3(speed * Mathf.Cos(dir) * Time.deltaTime, speed * Mathf.Sin(dir) * Time.deltaTime, 0);
			transform.position += mov;
		
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		finishExistingState("Walk");
		Coinflip(activity.Movement);
		Coinflip(activity.Action);
	}
	#endregion
	
	#region Action States
	protected virtual IEnumerator Attack() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = 20;
		
		//if((mask.value & 1<<c.gameObject.layer) == 1<<c.gameObject.layer)
		#if SHOW_DEBUG_MESSAGES
		if (debug) {
			if (enemyTarget != null)
				Debug.Log(gameObject.name + " is attacking the " + enemyTarget.name + "!");
			else Debug.Log(gameObject.name + " is looking for something to attack.");
		}
		#endif
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		makeAttackHitbox();
		
		frames = 20;
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}	
		makeAttackHitbox();
		
		frames = 10;
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Attack");
		Coinflip(activity.Action);
	}
	
	protected virtual IEnumerator Special() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = 50;
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is doing something special.");
		#endif
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Special");
		Coinflip(activity.Action);
	}
	#endregion
	
	#region Interrupts
	protected virtual IEnumerator Interrupt(float f) {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = f;
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is interrupted.");
		#endif
		//StopAllCoroutines();
		
		foreach (LessGenericList gl in activeStates) {
			if (gl.callName == "Interrupt") {
				continue;
			}
			
			StopCoroutine(gl.callName);
			gl.callActive = false;
		}
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Interrupt");
		Coinflip(activity.Movement);
	}
	
	protected virtual IEnumerator Interrupt(string[] _stateName, float f) {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = f;
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is interrupted.");
		#endif
		//StopAllCoroutines();
		
		foreach (LessGenericList gl in activeStates) {
			if (gl.callName == "Interrupt") {
				continue;
			}
			
			bool found = false;
			for (int i = 0; i < _stateName.Length; i++) {
				if (gl.callName == _stateName[i]) {
					found = true;
					continue;
				}
			}
			if (found)
				continue;
			
			StopCoroutine(gl.callName);
			gl.callActive = false;
		}
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Interrupt");
		Coinflip(activity.Movement);
	}
	
	protected virtual IEnumerator Die() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		#if SHOW_DEBUG_MESSAGES
		if (debug)
			Debug.Log(gameObject.name + " is dying.");
		#endif
		
		if (deathParticle != null)
			Instantiate(deathParticle, transform.position, Quaternion.identity);
		
		yield return new WaitForEndOfFrame();
		Destroy(this.gameObject);
	}
	#endregion
	
	#region Helper Scripts
	protected bool checkStatus(bool _busy, bool _interrupt) {
		busy = busy && _busy;
		interrupt = interrupt || _interrupt;
		return true;
	}
	
	protected bool makeAttackHitbox() {
		Vector3 rotationalVector;
		
		if (enemyTarget != null && items[0] != null) {
			rotationalVector = transform.position - enemyTarget.transform.position;
			if (!(rotationalVector.magnitude > sightRange || Physics.Linecast(transform.position, enemyTarget.transform.position, bitmap, QueryTriggerInteraction.Ignore))) {
				Vector3 relative = transform.InverseTransformPoint(enemyTarget.transform.position);
				float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
				Instantiate(items[0], transform.position, Quaternion.Euler(0, 0, angle), eventManager.transform);
				return true;
			} // else fallthrough return false;
		} else {
			enemyTarget = GameObject.FindWithTag("Player");
			if (enemyTarget == null)
				return false;
			else return makeAttackHitbox();
		}
		
		return false;
	}
	
	public void ModifyHealth(int n) {
		health += n;
		if (health <= 0) {
			IEnumerator corout = Interrupt(new string[] { "Die" }, 2);
			startExistingState("Die");
			//StartCoroutine(corout);
		}
	}
	
	protected bool findEventManager() {
		if (eventManager != null && eventManager != transform)
			return true;
		
		eventManager = transform;
		GameObject[] temp = GameObject.FindGameObjectsWithTag("GameController");
		foreach (GameObject g in temp) {
			if (g.GetComponent<CrapMenu>() == null)
				continue;
			else eventManager = g.transform;
		}
		if (eventManager == transform) {
			eventManager = Instantiate(new GameObject(), transform.position, Quaternion.identity).transform;
			eventManager.gameObject.tag = "GameController";
			eventManager.gameObject.AddComponent<CrapMenu>();
			Debug.Log("No event manager found upon instantiating.");
			return false;
		}
		return true;
	}
	#endregion
	
	#region State Transitions
	protected bool stateChange(int n) {
		if (n < 0 || n > stateNames.Length)
			return false;
		else return startExistingState(stateNames[n]);
	}
	
	protected bool startExistingState(string s) {
		#if SHOW_DEBUG_MESSAGES
		if (!debug)
			Debug.Log("Changing state to " + s);
		#endif
		
		LessGenericList gl = matchExistingState(s);
		if (gl != null) {
			if (gl.callActive == false) {
				gl.callActive = true;
				StartCoroutine(s);
				return true;
			}
		}
		
		return false;
	}
	
	protected bool checkExistingState(string s) {
		LessGenericList gl = matchExistingState(s);
		if (gl != null)
			return gl.callActive;
		else return false;
	}
	
	protected bool stopExistingState(string s) {
		LessGenericList gl = matchExistingState(s);
		if (gl != null) {
			if (gl.callActive == true) {
				StopCoroutine(s);
				gl.callActive = false;
				return true;
			}
		}
		
		return false;
	}
	
	protected bool finishExistingState(string s) {
		LessGenericList gl = matchExistingState(s);
		if (gl != null) {
			gl.callActive = false;
			return true;
		}
		else return false;
	}
	
	protected LessGenericList matchExistingState(string s) {
		foreach(LessGenericList gl in activeStates) {
			if (gl.callName == s)
				return gl;
			else continue;
		}
		
		return null;
	}
	#endregion
}
}
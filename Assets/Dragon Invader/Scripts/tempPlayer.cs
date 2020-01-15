using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayer : DragonInvader.Character {
	
	new public void Start() {
		#if SHOW_DEBUG_MESSAGES
		Debug.Log(gameObject.name + " is initializing.");
		#endif
		
		findEventManager();
		
		activeStates = new List<DragonInvader.LessGenericList>();
		
		for (int i = 0; i < stateNames.Length; i++) {
			DragonInvader.LessGenericList lgl = new DragonInvader.LessGenericList(stateNames[i]);
			activeStates.Add(lgl);
		}
	}
	
	protected override IEnumerator Die() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log(gameObject.name + " is dying.");
		#endif
		
		findEventManager();
        yield return new WaitForSeconds(1f);
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log("Restarting...");
		#endif
		
		if (eventManager != null) {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("Scene restarted!");
			#endif
			
			eventManager.GetComponent<CrapMenu>().LoadScene(0);
		} else StartCoroutine("Die");
	}
}

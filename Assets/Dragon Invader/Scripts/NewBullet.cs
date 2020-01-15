using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBullet : MonoBehaviour {

	public float speed = 2f;
	public float time = 50f;

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 moving = this.gameObject.transform.rotation * (new Vector3(speed * Time.deltaTime, 0, 0));
		this.transform.position += moving;
		time -= 1f;

		if (time < 0)
			Destroy(this.gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFeetColliderScript : MonoBehaviour {

	// Returns whether the obj is a floor, platform, or wall
	bool isPlatform(GameObject obj) {
		return obj.layer == LayerMask.NameToLayer ("Platform") || obj.layer == LayerMask.NameToLayer ("Player") || obj.layer == LayerMask.NameToLayer ("Enemy");
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (isPlatform(coll.gameObject)) {
			HumanPlayerController player = GetComponentInParent<HumanPlayerController>();
			if (player != null)
				player.OnGroundContactEnter();
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (isPlatform(coll.gameObject)) {
			HumanPlayerController player = GetComponentInParent<HumanPlayerController>();
			if (player != null)
				player.OnGroundContactExit();
		}
	}

}
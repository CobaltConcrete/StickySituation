using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFeetColliderScript : MonoBehaviour
{
    bool isPlatform(GameObject obj)
    {
		return obj.layer == LayerMask.NameToLayer ("Platform") || obj.layer == LayerMask.NameToLayer ("Player") || obj.layer == LayerMask.NameToLayer ("Enemy");
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (isPlatform(coll.gameObject))
        {
            SlimePlayerController player = GetComponentInParent<SlimePlayerController>();
            if (player != null)
            {
                player.feetContact = true;
                player.OnPlatformContactEnter();
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (isPlatform(coll.gameObject))
        {
            SlimePlayerController player = GetComponentInParent<SlimePlayerController>();
            if (player != null)
            {
                player.feetContact = false;
                player.OnPlatformContactExit();
            }
        }
    }
}
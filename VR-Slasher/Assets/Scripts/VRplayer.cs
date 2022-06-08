using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRplayer : MonoBehaviour
{
    public NextLevel Nextlevel;
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        // if player collides with obj and level is not null advance to nextLevel
        if (other.gameObject.CompareTag("Portal") || other.gameObject.CompareTag("Agent"))
        {
            Nextlevel.Load();
        }
    }
}

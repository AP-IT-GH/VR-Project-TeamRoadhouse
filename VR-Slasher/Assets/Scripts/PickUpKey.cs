using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    public int KeyCount;
    private void OnTriggerEnter(Collider other)
    {
        //check if player touches the gameobject with this script
        if (other.gameObject.tag == "Player")
        {
            // if Collider object is a trashbag then play sound, addpoint , destroy trashbag
            if (this.gameObject.tag == "Key")
            {
                // play sound at point before gameobject is destoryed otherwise you the sound won't play
                //AudioSource.PlayClipAtPoint(PickUpSound, transform.position);
                KeyCount++;
                KeyInventoryManager.instance.AddPoint();
                Destroy(gameObject);

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<PlayerInput>().isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<PlayerInput>().isOnLadder = false;
        }
    }
}

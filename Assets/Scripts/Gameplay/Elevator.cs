using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[RequireComponent( typeof(Collider2D) )]
public class Elevator : MonoBehaviour, IInteractable {
    public int connectedWithArea = 0;
    public int connectedWithElevator = 0;
    public List<int> traversedAreas;
    public bool active = true;


    private Vector3 initialScale;
    private Transform hint;

    private void Awake() {
        if(!active){
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        GetComponent<SpriteRenderer>().color = new Color(0,0,0,0.1f);

        var spr = GetComponentsInChildren<SpriteRenderer>();

        foreach(var sr in spr){
            if(sr.gameObject != gameObject){
                hint = sr.transform;
                initialScale = hint.localScale;
                hint.localScale = Vector3.zero;
            }
        }
    }

    public void Interact(Player player)
    {
        GameController.Instance.ElevatorTo(connectedWithArea, connectedWithElevator, traversedAreas);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(this);
            hint.DOScale(initialScale, 0.2f);            
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponent<Player>().SetInteractable(null);
            hint.DOScale(Vector3.zero, 0.2f);  
        }
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
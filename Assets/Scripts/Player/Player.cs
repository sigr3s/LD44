using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    //Manager should inject self here
    public IGameController gameController;


    [Header("Settings")]
    public int initialHP = 3;
    public int maxHP = 5;
    public float attackRadius = 2f;
    public float attackCD = 0.5f;
    public int damage = 1;
    public int hpTradePerAction = 1;
    public int hpTradeModifier = 10;
    public float invTime = 0.2f;
    public IInteractable interactableItem;

    [Header("Player Data")]
    public PlayerData playerData;

    private float playerInvTime = 0f;

    private Vector3 playerLocalScale;

    private void Awake() {
        playerLocalScale = transform.localScale;        
    }

    private void Update() {
        if(playerData.attackCD > 0){
            playerData.attackCD -= Time.deltaTime;
        }

        if(playerInvTime > 0){
            playerInvTime -= Time.deltaTime;
        }
    }

    public void Interact()
    {
        if(interactableItem != null){
            interactableItem.Interact(this);
        }
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        playerData.lastCheckPoint = checkpoint.id;
        playerData.checkPointArea = checkpoint.mapArea.area;
    }

    public void Respawn()
    {
        playerData.hp = initialHP;
        playerData.attackCD = 0f;
        transform.localScale = playerLocalScale;
        GetComponent<PlayerController>().facingDirection = FacingDirection.Right;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactableItem = interactable;
    }

    public bool Hability()
    {
        return gameController.TradeHpForTime(hpTradePerAction, hpTradeModifier, this);
    }

    public void Attack()
    {
        playerData.attackCD = attackCD;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject){
                Enemy enemy = colliders[i].GetComponent<Enemy>();

                if(enemy != null){
                    if(enemy.TakeDamage(damage)){
                        playerData.retievedSouls ++;
                        playerData.hp += enemy.playerRecoverOnKill;

                        playerData.hp = Mathf.Min(playerData.hp, maxHP);
                    }
                }
            }
        }
    }

    public void TakeDamage(int ammount)
    {
        if(playerInvTime > 0){

        }
        else
        {
            playerInvTime = invTime;
            playerData.hp -= ammount;

            transform.DOKill();
            transform.DOPunchScale( new Vector3(0,1,0) * 0.5f ,invTime, 4);

            GetComponent<PlayerInput>().TakeDamage();
        }
    }

    public void AdquiereItem(string item){
        if(playerData.collectedObjects == null) playerData.collectedObjects = new List<string>();
        
        playerData.collectedObjects.Add(item);
    }

    public void AnimationControlled(bool controlled)
    {
        GetComponent<PlayerInput>().enabled = !controlled;

        Collider2D[] playerColliders = GetComponents<Collider2D>();
        
        foreach(Collider2D cd in playerColliders){
            cd.enabled = !controlled;
        }

        GetComponent<Rigidbody2D>().simulated = !controlled;
    }
}
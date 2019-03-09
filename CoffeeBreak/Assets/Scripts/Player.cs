using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    public float moveSpeed;
    public float throwSpeed;
    public Transform coinPrefab;

    private int coins;
    private Transform coinToss = null;
    private Vector3 coinTarget;

    private Animator playerAnimator;
    private bool hasCup = false, hasCard = false;
    private GameObject interactiveObject;
    private bool playerMoving;
    private Vector2 lastMove;
    private Rigidbody2D rb;

   
    private void Start() {
        coins = GameManager.instance.initialPlayerCoins;
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        PlayerMovement();

        if(Input.GetButtonDown("Fire1") && coins > 0 && coinToss == null) TossCoin();

        if(coinToss != null){
            float step = throwSpeed * Time.deltaTime;
            coinToss.position = Vector2.MoveTowards(coinToss.position, coinTarget, step);
            if(coinToss.position == coinTarget){
                coinToss.tag = "Coin";
                coinToss = null;
            }
        }
        
    }


    private void TossCoin(){
        coinToss = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coinToss.tag = "CoinToss";
        coinTarget = transform.position + Vector3.Scale(new Vector3(5, 5, 0), lastMove);
        coins--;
    }


    private void PlayerMovement(){
        playerMoving = false;
        if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f){
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            if (Input.GetButton("SlowMovement")){
                playerAnimator.speed = 0.5f;
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f) * moveSpeed / 2f * Time.deltaTime);
            }
            else{
                playerAnimator.speed = 1f;
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f) * moveSpeed * Time.deltaTime);
            }
        }

        if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f){
            playerMoving = true;
            lastMove = new Vector2(0f,Input.GetAxisRaw("Vertical"));
            if (Input.GetButton("SlowMovement")){
                playerAnimator.speed = 0.5f;
                transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f) * moveSpeed/2f * Time.deltaTime);
            }
            else{
                playerAnimator.speed = 1f;
                transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f) * moveSpeed * Time.deltaTime);
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            if(interactiveObject != null){
                if(interactiveObject.CompareTag("Doors") && hasCard)
                    interactiveObject.SendMessage("Interact");

                if(interactiveObject.CompareTag("TrashBin"))
                    interactiveObject.SendMessage("Interact");

                //extendable to the coffee machine
            }
        }

        playerAnimator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        playerAnimator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        playerAnimator.SetBool("PlayerMoving", playerMoving);
        playerAnimator.SetFloat("LastMoveX", lastMove.x);
        playerAnimator.SetFloat("LastMoveY", lastMove.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Coin")){
            Destroy(other.gameObject);
            coins++;
        }

        if(other.gameObject.CompareTag("Cup")){
            Destroy(other.gameObject);
            hasCup = true;
        }
        if (other.gameObject.CompareTag("Card")){
            Destroy(other.gameObject);
            hasCard = true;
        }

        if (other.gameObject.CompareTag("Doors") || other.gameObject.CompareTag("TrashBin"))
        {
            interactiveObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Doors") || collision.gameObject.CompareTag("TrashBin"))
        {
            interactiveObject = null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static TrashBinController;
using static DoorController;
using static HighlightController;

public class Player : MonoBehaviour {

    public float throwForce;
    public Transform coinPrefab;
    public float moveSpeed;

    private int coins;
    private Transform coinToss = null;
    private float stopThreshold = 0.5f;
    private Animator playerAnimator;
    private bool hasCup = false, hasCard = false;
    private GameObject interactiveObject;
    private bool playerMoving;
    private Vector2 lastMove;
    private Rigidbody2D rb;
    private bool isStealth = false;
    // private Text coinsLabel, cupLabel, cardLabel;

    private void Start () {
        coins = GameManager.instance.initialPlayerCoins;
        playerAnimator = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody2D> ();
        // TODO change ot current HUD
        // coinsLabel = GameObject.Find ("CoinsLabel").GetComponent<Text> ();
        // cupLabel = GameObject.Find ("CupLabel").GetComponent<Text> ();
        // cardLabel = GameObject.Find ("CardLabel").GetComponent<Text> ();
    }

    private void Update () {
        HandlePlayerMovement ();
        HandleCoinToss ();
    }

    private void HandleCoinToss () {
        if (coinToss != null) {
            Vector2 coinVelocity = coinToss.GetComponent<Rigidbody2D> ().velocity;
            if (Mathf.Abs (coinVelocity.x) <= stopThreshold && Mathf.Abs (coinVelocity.y) <= stopThreshold) {
                coinToss.GetComponent<CircleCollider2D> ().isTrigger = true;
                coinToss = null;
            }
        } else if (Input.GetButtonDown ("Fire1") && coins > 0) {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            Vector3 direction = ray.origin - transform.position;
            coinToss = Instantiate (coinPrefab, transform.position + direction.normalized, Quaternion.identity);
            coinToss.GetComponent<CircleCollider2D> ().isTrigger = false;
            coinToss.tag = "ThrownCoin";
            coinToss.GetComponent<Rigidbody2D> ().AddForce (2 * Vector3.Scale (new Vector3 (throwForce, throwForce, 0), direction.normalized), ForceMode2D.Impulse);
            coins--;
            // coinsLabel.text = "Coins: " + coins;
        }
    }

    private void HandlePlayerMovement () {
        playerAnimator.ResetTrigger ("playerLeft");
        playerAnimator.ResetTrigger ("playerRight");

        if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) {
            lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                playerAnimator.SetTrigger ("playerLeft");
            } else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                playerAnimator.SetTrigger ("playerRight");
            }

            if (Input.GetButton ("SlowMovement")) {
                transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal"), 0f, 0f) * moveSpeed / 2f * Time.deltaTime);
                isStealth = true;
            } else {
                transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal"), 0f, 0f) * moveSpeed * Time.deltaTime);
                isStealth = false;
            }
        }

        if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) {
            lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
            if (Input.GetButton ("SlowMovement")) {
                transform.Translate (new Vector3 (0f, Input.GetAxisRaw ("Vertical"), 0f) * moveSpeed / 2f * Time.deltaTime);
                isStealth = true;
            } else {
                transform.Translate (new Vector3 (0f, Input.GetAxisRaw ("Vertical"), 0f) * moveSpeed * Time.deltaTime);
                isStealth = false;
            }
        }

        if (Input.GetButtonDown ("Interact")) {
            if (interactiveObject != null) {

                if (interactiveObject.CompareTag ("Doors") && hasCard) {
                    DoorController doorController = (DoorController) interactiveObject.GetComponent ("DoorController");
                    doorController.Interact ();
                }

                if (interactiveObject.CompareTag ("TrashBin")) {
                    TrashBinController binController = (TrashBinController) interactiveObject.GetComponent ("TrashBinController");
                    binController.DropBin ();
                }

                //extendable to the coffee machine
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Coin") || other.gameObject.CompareTag ("ThrownCoin")) {
            Destroy (other.gameObject);
            coins++;
            // coinsLabel.text = "Coins: " + coins; //TODO change to work with current HUD
        }

        if (other.gameObject.CompareTag ("Cup")) {
            Destroy (other.gameObject);
            hasCup = true;
            // cupLabel.text = "Cup picked up!"; //TODO change to work with current HUD
        }

        if (other.gameObject.CompareTag ("Card")) {
            Destroy (other.gameObject);
            hasCard = true;
            // cardLabel.text = "Card picked up!"; //TODO change to work with current HUD
        }

        if (other.gameObject.CompareTag ("Doors")){
            interactiveObject = other.gameObject;
        } 

        if(other.gameObject.CompareTag ("TrashBin")) {
            interactiveObject = other.gameObject;
            HighlightController lightController = (HighlightController) interactiveObject.GetComponentsInChildren<HighlightController> () [0];
            StartCoroutine (lightController.FlashNow ());
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.CompareTag ("Doors") || collision.gameObject.CompareTag ("TrashBin")) {
            interactiveObject = null;
        }
    }

    public bool IsStealth () {
        return isStealth;
    }
}
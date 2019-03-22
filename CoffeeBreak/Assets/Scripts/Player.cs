using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static TrashBinController;
using static DoorController;
using static HighlightController;
using static TipController;
public class Player : MonoBehaviour {

    public float throwForce;

    public float stealthSlowPerc;
    public Transform coinPrefab;
    public float moveSpeed;
    public Vector3 respawnPoint;

    private AudioSource actionSound;

    private float initialVolume;

    public AudioClip coinPickSound;
    public AudioClip coinThrowSound;

    public AudioClip cupPickSound;

    public AudioClip cardPickSound;

    private int coins;
    private Transform coinToss = null;
    private float stopThreshold = 0.5f;
    private Animator playerAnimator;
    private bool hasCup = false, hasCard = false;
    private GameObject interactiveObject;
    private bool playerMoving;
    private Vector2 lastMove;
    private Rigidbody2D rb;

    private int lives;

    private bool isStealth = false;
    private Text coinsLabel;
    private Image cardHUD, cupHUD;

    private void Start () {
        lives = 3;
        actionSound = gameObject.GetComponent<AudioSource> ();
        initialVolume = actionSound.volume;
        actionSound.volume = initialVolume * GameManager.instance.sfxMultiplier;
        coins = GameManager.instance.initialPlayerCoins;
        playerAnimator = GetComponent<Animator> ();
        respawnPoint = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D> ();
        coinsLabel = GameObject.Find ("CoinsLabel").GetComponent<Text> ();
        cardHUD = GameObject.Find ("CardImage").GetComponent<Image> ();
        cupHUD = GameObject.Find ("CupImage").GetComponent<Image> ();
        coinsLabel.text = "x " + coins;
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
            actionSound.clip = coinThrowSound;
            actionSound.Play ();
            coinToss.GetComponent<Rigidbody2D> ().AddForce (2 * Vector3.Scale (new Vector3 (throwForce, throwForce, 0), direction.normalized), ForceMode2D.Impulse);
            coins--;
            coinsLabel.text = "x " + coins;
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
                transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal"), 0f, 0f) * moveSpeed * stealthSlowPerc * Time.deltaTime);
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

                if (interactiveObject.CompareTag ("CoffeeMachine")) {
                    if ((hasCup && coins >= 25) || coins >= 30) {
                        FindObjectOfType<GameManager>().WinGame();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Coin") || other.gameObject.CompareTag ("ThrownCoin")) {
            Destroy (other.gameObject);
            coins++;
            actionSound.volume = 0.05f * GameManager.instance.sfxMultiplier;
            actionSound.clip = coinPickSound;
            actionSound.Play ();
            coinsLabel.text = "x " + coins; //TODO change to work with current HUD
        }

        if (other.gameObject.CompareTag ("Cup")) {
            Destroy (other.gameObject);
            hasCup = true;
            actionSound.volume = 0.5f;
            actionSound.clip = cupPickSound;
            actionSound.Play ();
            cupHUD.enabled = true;
        }

        if (other.gameObject.CompareTag ("Card")) {
            Destroy (other.gameObject);
            hasCard = true;
            actionSound.volume = 0.5f * GameManager.instance.sfxMultiplier;
            actionSound.clip = cardPickSound;
            actionSound.Play ();
            cardHUD.enabled = true;
        }

        if (other.gameObject.CompareTag ("Tip")) {
            TipController tip = (TipController) other.gameObject.GetComponent ("TipController");
            tip.handleView ();
        }

        if (other.gameObject.CompareTag ("Doors") || other.gameObject.CompareTag ("CoffeeMachine")) {
            interactiveObject = other.gameObject;
        }

        if (other.gameObject.CompareTag ("TrashBin")) {
            interactiveObject = other.gameObject;
            HighlightController lightController = (HighlightController) interactiveObject.GetComponentsInChildren<HighlightController> () [0];
            StartCoroutine (lightController.FlashNow ());
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.CompareTag ("Doors") || collision.gameObject.CompareTag ("TrashBin")) {
            interactiveObject = null;
        }

        if (collision.gameObject.CompareTag ("Tip")) {
            TipController tip = (TipController) collision.gameObject.GetComponent ("TipController");
            tip.handleView ();
        }
    }

    public bool IsStealth () {
        return isStealth;
    }

    public void looseLife () {
        if (lives <= 0) return;
        GameObject.Find ("Excuse" + lives).GetComponent<Image> ().enabled = false;
        lives--;
        if (lives == 0) {
            FindObjectOfType<GameManager> ().EndGame ();
        }
    }
}
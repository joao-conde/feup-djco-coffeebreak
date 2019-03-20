using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;


public class SpawnController : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            Player player = (Player) other.gameObject.GetComponent ("Player");
            player.respawnPoint = gameObject.transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        float randomposX = 0f;
        float randomposY = 0f;
        PhotonNetwork.Instantiate(player.name, new Vector2(player.transform.position.x + randomposX, player.transform.position.y + randomposY), Quaternion.identity);
    }
}

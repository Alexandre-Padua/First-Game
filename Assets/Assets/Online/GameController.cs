using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;

    public Text pingText;

    public Text spawnTimer;
    public GameObject respawnUI;

    public float totalRespawnTime = 5f;

    private float respawnTime;
    private bool startRespawn = false;

    //Player do cliente atual
    [HideInInspector]public GameObject localPlayer;

    public static GameController instance;

    public bool isAlive;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();

        respawnTime = totalRespawnTime;
    }

    void Update()
    {
        if(startRespawn)
        {
            StartRespawn();
        }

        pingText.text = PhotonNetwork.GetPing().ToString();
    }

    public void SpawnPlayer()
    {
        float randomposX = Random.Range(-6, 1);
        float randomposY = Random.Range(-4, 3);

        PhotonNetwork.Instantiate(player.name, new Vector2(player.transform.position.x + randomposX, player.transform.position.y + randomposY), Quaternion.identity);
    }

    #region Respawn Functions
    void StartRespawn()
    {
        isAlive = false;

        respawnTime -= Time.deltaTime;
        spawnTimer.text = "RESPAWN IN: " + respawnTime.ToString("F0");

        if(respawnTime <= 0)
        {
            respawnUI.SetActive(false);            
            localPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
            PlayerRespawnPos();
            isAlive = true;
            startRespawn = false;
        }
    }

    public void PlayerRespawnPos()
    {
        float randomPosX = Random.Range(-6, 1);
        float randomPosY = Random.Range(-4, 3);
        localPlayer.transform.localPosition = new Vector2(randomPosX, randomPosY);
    }

    public void EnableRespawn()
    {
        respawnTime = totalRespawnTime;
        startRespawn = true;
        respawnUI.SetActive(true);
    }
    #endregion

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}
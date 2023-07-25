using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public GameObject connectedScreen;
    public GameObject disconnectedScreen;

    //Conecta ao servidor
    public void ConnectBt()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //Entra no lobby
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //Conex�o falha
    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectedScreen.SetActive(true);
    }

    //Ap�s login no lobby
    public override void OnJoinedLobby()
    {
        connectedScreen.SetActive(true);
    }
}

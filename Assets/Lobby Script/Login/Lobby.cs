using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviourPunCallbacks
{
    public InputField createRoom;
    public InputField joinRoom;

    public void JoinRoomBt()
    {
        PhotonNetwork.JoinRoom(joinRoom.text, null);
    }
    
    public void CreateRoomBt()
    {
        PhotonNetwork.CreateRoom(createRoom.text, new RoomOptions{MaxPlayers = 4}, null);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room failed! " + returnCode + " Message " + message);
    }
}
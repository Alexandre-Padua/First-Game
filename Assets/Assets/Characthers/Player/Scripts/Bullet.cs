using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("Destroy", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    private void Destroy()
    {
        Destroy(gameObject, 2f);
    }
}

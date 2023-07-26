using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    [Header("Movement Parameters")]
    public float moveSpeed;
    [HideInInspector] public Rigidbody2D rig;
    [HideInInspector] public Vector2 direction;
    private Vector2 clientPos;
    private Animator anim;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 1);
        }
        else
        {
            anim.SetInteger("transition", 0);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //Minha movimentação
            ProcessInput();
        }
        else
        {
            //Sincroniza outros players
            SmoothMovement();
        }
    }

    #region MyClient
    private void ProcessInput()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.MovePosition(rig.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("shooting");
        }

        if (movement > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            this.photonView.RPC("ChangeRight", RpcTarget.OthersBuffered);
        }

        if (movement < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            this.photonView.RPC("ChangeLeft", RpcTarget.OthersBuffered);
        }
    }
    #endregion

    #region RPCs Functions
    [PunRPC]
    private void ChangeLeft()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    [PunRPC]
    private void ChangeRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    [PunRPC]
    private void Shoot()
    {
        anim.SetTrigger("shooting");
    }
    #endregion

    #region OthersClient
    private void SmoothMovement()
    {
        rig.position = Vector2.MoveTowards(rig.position, clientPos, Time.fixedDeltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rig.position);
            stream.SendNext(rig.velocity);
        }
        else
        {
            clientPos = (Vector2)stream.ReceiveNext();
            rig.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            clientPos += rig.velocity * lag;
        }
    }
    #endregion
}
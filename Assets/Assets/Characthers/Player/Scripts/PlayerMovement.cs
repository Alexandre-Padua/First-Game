using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    [Header("Movement Parameters")]
    public float moveSpeed;
    public Rigidbody2D rig;
    [HideInInspector] public Vector2 direction;

    private Vector2 clientPos;
    private Animator anim;
    private Health health;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public Text nickName;

    public Transform canvas;

    private float movement;

    void Awake()
    {
        if (photonView.IsMine == true)
        {
            GameController.instance.localPlayer = this.gameObject;
            nickName.text = PhotonNetwork.NickName;
        }
        else
        {
            nickName.text = photonView.Owner.NickName;
        }
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        /*direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 1);
        }
        else
        {
            anim.SetInteger("transition", 0);
        }*/        
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine && GameController.instance.isAlive)
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
        movement = Input.GetAxis("Horizontal");

        rig.MovePosition(rig.position + direction.normalized * moveSpeed * Time.fixedDeltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("shooting");
            photonView.RPC("Shoot", RpcTarget.Others);
        }

        if (movement > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            canvas.eulerAngles = new Vector2(0, 0);
            this.photonView.RPC("ChangeRight", RpcTarget.Others);
        }

        if (movement < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
            canvas.eulerAngles = new Vector2(0, 0);
            this.photonView.RPC("ChangeLeft", RpcTarget.Others);
        }

        //Animações de movimento
        this.photonView.RPC("Animation", RpcTarget.Others);

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
    #endregion

    #region RPCs Functions
    [PunRPC]
    private void ChangeLeft()
    {
        transform.eulerAngles = new Vector2(0, 180);
        canvas.eulerAngles = new Vector2(0, 0);
    }

    [PunRPC]
    private void ChangeRight()
    {
        transform.eulerAngles = new Vector2(0, 0);
        canvas.eulerAngles = new Vector2(0, 0);
    }

    [PunRPC]
    private void Shoot()
    {
        //anim.SetTrigger("shooting");
        GameObject b = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);

        if (movement < 0)
        {
            b.GetComponent<PhotonView>().RPC("MoveLeft", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Animation()
    {
        //direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (photonView.IsMine)
        {
            if (direction.sqrMagnitude > 0)
            {
                anim.SetInteger("transition", 1);
            }
            else
            {
                anim.SetInteger("transition", 0);
            }
        }
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
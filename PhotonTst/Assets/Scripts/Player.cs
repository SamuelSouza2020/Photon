using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviour
{
    public Rigidbody rbody;

    public GameObject canoTk;

    public Camera cam;

    public PhotonView photonView;

    //public GameObject carga;
    public Transform bulletSpawnPoint;

    public int qBullet = 1, contD = 0;

    public GameObject bazukPlayer, shild;

    public Material mat;

    [SerializeField]
    Animator anim;

    void Start()
    {
        shild.SetActive(false);
        contD = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            if(Input.GetKeyDown(KeyCode.B))
            {
                photonView.RPC("ComShil", RpcTarget.All);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                photonView.RPC("SemShil", RpcTarget.All);
            }

            //trocar textura
            if (contD == 1)
            {
                photonView.RPC("AnimDano", RpcTarget.All);
            }
            else if(contD >= 2)
            {
                photonView.RPC("SemShil", RpcTarget.All);
            }

            cam = gameObject.transform.GetChild(0).GetComponent<Camera>();

            cam.depth = 1;

            float inputRotation = Input.GetAxis("Horizontal");
            float inputSpeed = Input.GetAxis("Vertical");

            Quaternion rot = rbody.rotation * Quaternion.Euler(0, inputRotation * Time.deltaTime * 90, 0);//60
            rbody.MoveRotation(rot);

            Vector3 force = rot * Vector3.forward * inputSpeed * 5000 * Time.deltaTime;//1000
            rbody.AddForce(force);

            if (rbody.velocity.magnitude > 4)
            {
                rbody.velocity = rbody.velocity.normalized * 4;
            }

            if (Input.GetKey(KeyCode.J))
            {
                photonView.RPC("EsqCan", RpcTarget.All);
            }
            if (Input.GetKey(KeyCode.L))
            {
                photonView.RPC("DirCan", RpcTarget.All);
            }


            if (Input.GetKeyDown(KeyCode.Space) && qBullet == 1)// && carga.activeSelf)
            {
                qBullet = 1;
                //carga.SetActive(false);
                photonView.RPC("Fire", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bulletPrefab = Resources.Load("BulletTk") as GameObject;
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, canoTk.transform.rotation) as GameObject;
        bulletObject.GetComponent<GameBullet>().Shoot(lag);
    }
    [PunRPC]
    public void SemShil(PhotonMessageInfo info)
    {
        shild.SetActive(false);
    }
    [PunRPC]
    public void ComShil(PhotonMessageInfo info)
    {
        shild.SetActive(true);
    }
    [PunRPC]
    public void AnimDano(PhotonMessageInfo info)
    {
        anim.Play("ShildDano");
    }
    [PunRPC]
    public void EsqCan(PhotonMessageInfo info)
    {
        canoTk.transform.eulerAngles += new Vector3(0, -100 * Time.deltaTime, 0);
    }
    [PunRPC]
    public void DirCan(PhotonMessageInfo info)
    {
        canoTk.transform.eulerAngles += new Vector3(0, 100 * Time.deltaTime, 0);
    }

    public void Died()
    {
        //talvez mudar para o random
        //rbody.MovePosition(new Vector3(0, 1, 0));
        contD = 0;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("BulletTk"))
        {
            if(!shild.activeSelf)
            {
                //Debug.Log(other.gameObject.GetComponent<Rigidbody>().velocity);
            }
        }
        /*if(Physics.Raycast(transform.position, )
        {

        }*/
    }
}

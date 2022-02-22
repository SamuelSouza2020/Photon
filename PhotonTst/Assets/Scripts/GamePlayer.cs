using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GamePlayer : MonoBehaviour
{
    public Rigidbody rbody;

    //public GameObject carga;
    public Transform bulletSpawnPoint;

    public PhotonView photonView;

    public Camera cam;

    public Text vic, mort;

    bool mortP = false;

    float qBullet = 0;

    public GameObject bazukPlayer;

    GameManager gm;

    void Start()
    {
        //carga.SetActive(false);
        photonView.RPC("ChangeColor", RpcTarget.All, Random.Range(0.0f, 1.0f));
        StartCoroutine(Limp());
        vic = GameObject.Find("ChatLogText").GetComponent<Text>();
        mort = GameObject.Find("Morreu").GetComponent<Text>();
        //cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        photonView.RPC("ChangeBazukaT", RpcTarget.All);
        qBullet = 0;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        //esse if garente que essa programação só rode no player que controle
        if(photonView.IsMine)
        {
            //qtdBullet = 
            cam = gameObject.transform.GetChild(0).GetComponent<Camera>();

            cam.depth = 1;

            /*if (Input.GetKeyDown(KeyCode.K))
            {
                PhotonNetwork.Instantiate("GamePlayerlvl2", transform.position, Quaternion.identity);
            }*/

            if (Input.GetKeyDown(KeyCode.K))
            {
                photonView.RPC("DestPlay", RpcTarget.All);
                PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
            }

            if (mortP)
            {
                mort.enabled = true;
                StartCoroutine(Limp());
            }
            else
            {
                mort.enabled = false;
            }

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
            if (Input.GetKeyDown(KeyCode.Space) && qBullet == 1)// && carga.activeSelf)
            {
                qBullet = 0;
                //carga.SetActive(false);
                photonView.RPC("Fire", RpcTarget.All);
                //Teste Cano
                photonView.RPC("ChangeBazukaT", RpcTarget.All);

            }

            /*if (Input.GetKeyDown(KeyCode.C))
            {
                photonView.RPC("ChangeColor", RpcTarget.All, Random.Range(0.0f, 1.0f));
            }*/
        }
    }

    //função rpc, chamado de procedimentos remotos
    [PunRPC]
    public void Fire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bulletPrefab = Resources.Load("Bullet") as GameObject;
        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation) as GameObject;
        bulletObject.GetComponent<GameBullet>().Shoot(lag);
    }
    [PunRPC]
    public void ChangeColor(float hue, PhotonMessageInfo info)
    {
        Color newColor = Color.HSVToRGB(hue, 1, 1);
        GetComponent<Renderer>().material.color = newColor;
    }
    [PunRPC]
    public void ChangeBazukaT(PhotonMessageInfo info)
    {
        bazukPlayer.SetActive(false);
        //bp3 = BarrPri.transform.GetChild(2).GetComponent<Image>();
    }
    [PunRPC]
    public void ChangeBazukaP(PhotonMessageInfo info)
    {
        bazukPlayer.SetActive(true);
        //bp3 = BarrPri.transform.GetChild(2).GetComponent<Image>();
    }
    [PunRPC]
    public void DestPlay(PhotonMessageInfo info)
    {
        Destroy(gameObject);
        //bp3 = BarrPri.transform.GetChild(2).GetComponent<Image>();
    }
    public void Died()
    {
        //carga.SetActive(false);
        qBullet = 0;
        //talvez mudar para o random
        rbody.MovePosition(new Vector3(0,1,0));
        photonView.RPC("ChangeBazukaT", RpcTarget.All);
        mortP = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Bazuka"))
        {
            photonView.RPC("ChangeBazukaP", RpcTarget.All);
            qBullet = 1;
            //carga.SetActive(true);
            Destroy(other.gameObject);
            gm.contBall--;
        }
    }
    IEnumerator Limp()
    {
        yield return new WaitForSeconds(2);
        vic.text = "";
        mortP = false;
    }
}

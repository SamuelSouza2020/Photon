using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    public int contBall = 0;
    public Text jogadores;

    void Start()
    {
        contBall = 0;
    }

    // Update is called once per frame
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //criar uma quantidade limite
            StartCoroutine(SpawnBazuka());
        }
    }
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //criar uma quantidade limite
            StartCoroutine(SpawnBazuka());
        }
    }
    public IEnumerator SpawnBazuka()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            Vector3 pos = Random.insideUnitSphere * 50;
            pos.y = 1.4f;

            Quaternion rot = Quaternion.Euler(90, Random.Range(0, 360), 0);
            if(contBall < 15)
            {
                PhotonNetwork.Instantiate("Bazuka", pos, rot);
                contBall++;
            }
        }
        /*do
        {
            Debug.Log("EntrouAK");
            yield return new WaitForSeconds(Random.Range(1, 3));
            Vector3 pos = Random.insideUnitSphere * 30;
            pos.y = 1.4f;

            Quaternion rot = Quaternion.Euler(90, Random.Range(0, 360), 0);
            PhotonNetwork.Instantiate("Bazuka", pos, rot);
            contBall++;
        } while(contBall < 10);*/
    }
}

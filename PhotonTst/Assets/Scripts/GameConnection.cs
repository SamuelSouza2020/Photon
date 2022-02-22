using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class GameConnection : MonoBehaviourPunCallbacks
{
    public Text chatLog, nickP;//, jogadores;
    public Button confirm;
    public GameObject pConec, pJogador;
    GameManager gm;


    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pConec.SetActive(true);
        pJogador.SetActive(false);
        confirm.onClick.AddListener(Entrou);
        //chatLog.text += "\nConectando ao servidor...";
        //PhotonNetwork.LocalPlayer.NickName = "Prime_" + Random.Range(0, 1000);
        PhotonNetwork.LocalPlayer.NickName = nickP.text;
    }
    void Entrou()
    {
        PhotonNetwork.ConnectUsingSettings();
        pConec.SetActive(false);
        pJogador.SetActive(true);
        gm.jogadores.text += "Nickname: " + nickP.text;
    }

    public override void OnConnectedToMaster()
    {

        //chatLog.text += "\nConectado ao Servidor!";

        if(PhotonNetwork.InLobby == false)
        {
            //chatLog.text += "\nEntrando no lobby...";
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        //chatLog.text += "\nEntrei no lobby!";
        PhotonNetwork.JoinRoom("Prime");
        //chatLog.text += "\nEntrando na sala Prime...";//
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //chatLog.text += "\nErro ao entrar na sala " + message + " return codigo " + returnCode;

        
        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            RoomOptions room = new RoomOptions { MaxPlayers = 20 };
            PhotonNetwork.CreateRoom("Prime", room, null);
            //chatLog.text += "\nCriando sala Prime!";
        }
    }
    public override void OnJoinedRoom()
    {
        //chatLog.text += "\nVoce entrou na sala Prime! Seu NickName é: " + PhotonNetwork.LocalPlayer.NickName;
        //aqui voce deve instanciar o player na tela
        Vector3 pos = new Vector3(Random.Range(-40.0f, 40.0f), 1, Random.Range(-40.0f, 40.0f));
        //Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f), 1, Random.Range(-10.0f, 10.0f));
        Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0.0f, 360.0f));
        PhotonNetwork.Instantiate("GamePlayer", pos, rot);
    }
    public override void OnLeftRoom()
    {
        //chatLog.text += "\nVoce saiu da sala Prime";
    }
    /*public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //chatLog.text += "\nUm jogador entrou na sala Prime! O NickName dele é: " + newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        //chatLog.text += "\nUm jogador saiu na sala Prime! O NickName dele é: " + newPlayer.NickName;
    }*/
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        //chatLog.text += "\nAconteceu um erro! " + errorInfo.Info;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBullet : MonoBehaviour
{

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //se quem ele colidiu contem bazuka no nome
        if(other.name.Contains("Bazuka"))
        {
            //colocar colider na bazuka do player para ele perder a bazuka
            //Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            gm.contBall--;
        }
        //else if(other.name.Contains("Enemy"))
        else if(other.name.Contains("GamePlayer(Clone)"))
        {
            Debug.Log(other.gameObject);
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(0, 100), ForceMode.Impulse);
            Debug.Log("Foi");
            other.GetComponent<GamePlayer>().Died();
        }
        else if(other.name.Contains("Player(Clone)"))
        {
            Debug.Log(other.gameObject);
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0,500), ForceMode.Impulse);
            //Debug.Log("Foi");
            //other.GetComponent<GamePlayer>().Died();
        }
        Destroy(gameObject);
    }
    public void Shoot(float lag)
    {
        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.velocity = transform.forward * 15;
        //para nao perder a velocidade, corrigir com o lag
        rbody.position += rbody.velocity * lag;
    }
}

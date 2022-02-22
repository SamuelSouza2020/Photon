using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Shild : MonoBehaviour
{

    [SerializeField]
    Player py;


    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("BulletTk"))
        {
            py.contD++;
            Destroy(other.gameObject);
        }
        /*if (other.name.Contains("Bullet"))
        {
            Destroy(other.gameObject);
            contD++;
        }*/
    }
}

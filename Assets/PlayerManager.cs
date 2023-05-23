using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]PhotonView PV;

    GameObject controller;
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    public void CreateController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("Photon", "PlayerController"), new Vector3(Random.Range(-10,10),0,Random.Range(-10,10)), Quaternion.identity, 0, new object[] { PV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}

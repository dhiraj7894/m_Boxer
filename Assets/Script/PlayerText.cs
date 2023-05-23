using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerText : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] private TextMeshProUGUI _playerText;
    public void SetUpName(Player _player){
        player = _player;
        _playerText.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}

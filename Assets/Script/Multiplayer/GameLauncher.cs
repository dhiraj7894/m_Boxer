using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class GameLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createRoomInput;
    [SerializeField] private TMP_InputField _findRoomInput;
    [SerializeField] private TextMeshProUGUI _ErrorMessageInLobby;
    [SerializeField] private TextMeshProUGUI _ErrorMessageInRE;
    [SerializeField] private TextMeshProUGUI _OnJoindRoomName;
    
    [SerializeField] private TextMeshProUGUI _loadingText;

    [SerializeField] private Transform _playerListContent;
    [SerializeField] private GameObject _playerListPrefab;
    [SerializeField] private GameObject _startGameButton;
    private void Start()
    {
        if(!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();

        //DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        _loadingText.text = "Connected";
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 9999).ToString("0000");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _ErrorMessageInRE.text = "<color = red>"+ message + "</color>";
        MenuManager.Instance.OpenMenu("roomError");

    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        _OnJoindRoomName.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerListPrefab, _playerListContent).GetComponent<PlayerText>().SetUpName(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerListPrefab, _playerListContent).GetComponent<PlayerText>().SetUpName(newPlayer);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_createRoomInput.text))
        {
            _ErrorMessageInLobby.text = "<color=red>Create room input filed is Empty.</color>";
            LeanTween.delayedCall(1, () => { _ErrorMessageInLobby.text = ""; });
            return;
        }
        //PhotonNetwork.CreateRoom(CreateRoomInput.text);

        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(_createRoomInput.text, roomOption, TypedLobby.Default);
        
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_findRoomInput.text))
        {
            _ErrorMessageInLobby.text = "<color=red>Join room input filed is Empty.</color>";
            return;
        }
        PhotonNetwork.JoinRoom(_findRoomInput.text);
    }

    

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}

 
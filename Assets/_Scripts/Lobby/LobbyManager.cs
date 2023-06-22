using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _IIPlayerPanel;
        [SerializeField] private GameObject _createPanel;
        [SerializeField] private GameObject _joinPanel;
        [SerializeField] private RoomList _roomList;
        [SerializeField] private GameObject _inRoomPanel;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _roomInputField;
        
        private void Awake()
        {
            PhotonNetwork.OfflineMode = false;
            if (PhotonNetwork.IsConnected)
            {
                OpenPanel(_mainPanel);
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            
        }

        private void Start()
        {
            InitPlayerPref();
        }

        private void InitPlayerPref()
        {
            PlayerPrefs.SetInt("level",1);
            PlayerPrefs.SetInt("live",2);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.AutomaticallySyncScene = true;
            OpenPanel(_mainPanel);
        }

        public void OnIIPlayerButton()
        {
            if (PhotonNetwork.InLobby)
            {
                OpenPanel(_IIPlayerPanel);
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public void OnHostButton()
        {
            SetNickNamePlayer();
            OpenPanel(_createPanel);
        }

        public void OnCreateRoomButton()
        {
            string roomName;
            if (string.IsNullOrEmpty(_roomInputField.text))
            {
                roomName = _roomInputField.placeholder.GetComponent<TMP_Text>().text;
            }
            else
            {
                roomName = _roomInputField.text;
            }

            PhotonNetwork.CreateRoom(roomName, new RoomOptions {IsVisible = true, MaxPlayers = 2});
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (PhotonNetwork.OfflineMode == true)
            {
                return;
            }
            OpenPanel(_inRoomPanel);
        }

        public void OnJoinRoomButton()
        {
            SetNickNamePlayer();
            OpenPanel(_joinPanel);
        }

        private void SetNickNamePlayer()
        {
            if (string.IsNullOrEmpty(_nameInputField.text))
            {
                PhotonNetwork.NickName = _nameInputField.placeholder.GetComponent<TMP_Text>().text;
                return;
            }
            PhotonNetwork.NickName = _nameInputField.text;
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            OpenPanel(_IIPlayerPanel);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            _roomList.UpdateList(roomList);
        }

        public void OpenPanel(GameObject panel)
        {
            _loadingPanel.SetActive(false);
            _mainPanel.SetActive(false);
            _IIPlayerPanel.SetActive(false);
            _createPanel.SetActive(false);
            _joinPanel.SetActive(false);
            _inRoomPanel.SetActive(false);
            panel.SetActive(true);
        }

        public void OnIPlayerButton()
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(StartOfflineMode());
        }

        IEnumerator StartOfflineMode()
        {
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }

            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.JoinOrCreateRoom("Single",new RoomOptions(){MaxPlayers = 1,IsVisible = false,IsOpen = false},TypedLobby.Default);
            PhotonNetwork.LoadLevel("Scenes/Game");
        }

        public void OnBackInRoomButton()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnStartGameButton()
        {
            PhotonNetwork.LoadLevel("Scenes/Game");
        }
    }
}
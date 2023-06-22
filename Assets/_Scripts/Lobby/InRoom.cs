using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Lobby
{
    public class InRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _player1;
        [SerializeField] private TMP_Text _player2;
        [SerializeField] private Button _startButton;
        [SerializeField] private GameObject _waitingPanel;
        

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            SetName();
            SetStartGame();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            SetName();
            SetStartGame();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            if (otherPlayer.IsMasterClient)
            {
                return;
            }
            SetName();
            SetStartGame();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            SetName();
            SetStartGame();
        }

        private void SetStartGame()
        {
            _startButton.gameObject.SetActive(false);
            _waitingPanel.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                _startButton.gameObject.SetActive(true);
                bool isStartGame = PhotonNetwork.PlayerList.Length > 1;
                _startButton.interactable = isStartGame;
                _startButton.GetComponent<Image>().color = isStartGame? Color.green : Color.red;

            }
            else
            {
                _waitingPanel.SetActive(true);
            }
        }

        
        private void SetName()
        {
            _roomName.text = PhotonNetwork.CurrentRoom.Name;
            _player1.text = "Waiting PlayerTank...";
            _player2.text = "Waiting PlayerTank...";
            _player1.text = PhotonNetwork.PlayerList[0].NickName;
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                _player2.text = PhotonNetwork.PlayerList[1].NickName;
            }
        }
    }
}
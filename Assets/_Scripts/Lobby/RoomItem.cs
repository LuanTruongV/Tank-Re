using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Lobby
{
    public class RoomItem : MonoBehaviour

    {
        [SerializeField] private TMP_Text _nameRoom;
        [SerializeField] private TMP_Text _count;
        [SerializeField]private Button _joinButton;

        public void SetItem(RoomInfo roomInfo)
        {
            _nameRoom.text = roomInfo.Name;
            _count.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
            if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
            {
                _joinButton.gameObject.SetActive(false);
                return;
            }
            _joinButton.onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(roomInfo.Name);
            }); 
        }

        
    }
}
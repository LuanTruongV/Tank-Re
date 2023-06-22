using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace _Scripts.InGame.Manager
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            SceneManager.LoadScene("Scenes/Lobby");
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            PhotonNetwork.LeaveRoom();
        }
    }
}
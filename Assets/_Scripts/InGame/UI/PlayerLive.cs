using Photon.Pun;
using UnityEngine;

namespace _Scripts.InGame.UI
{
    public class PlayerLive : MonoBehaviour
    {
        private void Awake()
        {
            InstancePlayerLive(PhotonNetwork.PlayerList.Length);
        }

        private void InstancePlayerLive(int playerCount)
        {
            for (int i=0;i<PhotonNetwork.PlayerList.Length;i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<PlayerItem>().Initialize(PhotonNetwork.PlayerList[i]);
            }
        }
    }
}
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.InGame
{
    public class Spawn : MonoBehaviourPunCallbacks
    {
        //Animation Event
        public void SpawnPlayer()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            PhotonNetwork.Instantiate("Player", transform.position, quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
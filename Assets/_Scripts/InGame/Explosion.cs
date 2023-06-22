using System;
using Photon.Pun;

namespace _Scripts.InGame
{
    public class Explosion : MonoBehaviourPunCallbacks
    {
        //animation event
        public void DestroyGo()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
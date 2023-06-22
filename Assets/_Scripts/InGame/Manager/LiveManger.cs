using System;
using _Scripts.InGame.Message;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.InGame
{
    public class LiveManger : MonoBehaviour
    {
        private bool _isEnded=false;
        private void Start()
        {
            SetProperties();
            PlayerTank.OnPlayerDie += HandlePlayerDie;
        }

        private void OnDestroy()
        {
            PlayerTank.OnPlayerDie -= HandlePlayerDie;
        }

        private void HandlePlayerDie(Player player)
        {
            if (!Equals(PhotonNetwork.LocalPlayer, player))
            {
                return;
            }
            if (_isEnded)
            {
                return;
            }
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("live",out object liveObj);
            int live = (int) liveObj;
            if (live == 0)
            {
                MessageBroker.Default.Publish(new PlayerEnd{player = PhotonNetwork.LocalPlayer});
                _isEnded = true;
                return;
            }
            live--;
            Hashtable hash = new Hashtable();
            hash.Add("live",(object)live);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        private void SetProperties()
        {
            int live = PlayerPrefs.GetInt("live", 2);
            Hashtable hashtable = new Hashtable();
            hashtable.Add("live",live);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
    }
}
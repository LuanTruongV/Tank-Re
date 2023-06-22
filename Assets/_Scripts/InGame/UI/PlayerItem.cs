using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace _Scripts.InGame.UI
{
    public class PlayerItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _liveText;
        private Player _player;
        public void Initialize(Player player)
        {
            _player = player;
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (Equals(targetPlayer, _player) && changedProps.ContainsKey("live"))
            {
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            if (_player.CustomProperties.TryGetValue("live", out object liveCount))
            {
                _liveText.text = liveCount.ToString();
            }
        }
    }
}
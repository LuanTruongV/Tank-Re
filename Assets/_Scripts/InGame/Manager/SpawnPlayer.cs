using System;
using _Scripts.InGame.Message;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UniRx;

namespace _Scripts.InGame
{
    public class SpawnPlayer : MonoBehaviour
    {
        public static Action<Player> OnPlayerSpawn;
        [SerializeField] private LiveManger _liveManger; 
        [SerializeField]private Transform _playerISpawnPoint;
        [SerializeField]private Transform _playerIISpawnPoint;
        private bool _isEnded=false;


        private void Awake()
        {
            MessageBroker.Default.Receive<PlayerEnd>().Where(x => Equals(x.player, PhotonNetwork.LocalPlayer))
                .Subscribe(x => _isEnded = true).AddTo(this);
        }

        private void Start()
        {
            Spawn();
            PlayerTank.OnPlayerDie += HandlePlayerDie;
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
            Spawn();
        }
        private void OnDestroy()
        {
            PlayerTank.OnPlayerDie -= HandlePlayerDie;
        }

        private void Spawn()
        {
            Transform spawnPoint = PhotonNetwork.IsMasterClient ? _playerISpawnPoint : _playerIISpawnPoint;
            PhotonNetwork.Instantiate("Spawn", spawnPoint.position, quaternion.identity);
            OnPlayerSpawn?.Invoke(PhotonNetwork.LocalPlayer);
        }
        
    }
}
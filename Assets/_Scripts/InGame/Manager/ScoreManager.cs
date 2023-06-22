using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.InGame
{
    public class ScoreManager : MonoBehaviourPunCallbacks
    {
        private Dictionary<EnemyType, int> _scorePlayer1 = new Dictionary<EnemyType, int>();
        public Dictionary<EnemyType, int> ScorePlayer1 => _scorePlayer1;
        private Dictionary<EnemyType, int> _scorePlayer2 = new Dictionary<EnemyType, int>();
        public Dictionary<EnemyType, int> ScorePlayer2 => _scorePlayer2;
        private void Awake()
        {
            InitDictionary();
        }

        


        private void Start()
        {
            Enemy.OnEnemyDie += HandleEnemyDie;
        }

        private void OnDestroy()
        {
            Enemy.OnEnemyDie -= HandleEnemyDie;
        }

        private void HandleEnemyDie(EnemyType enemyType, int id)
        {
            photonView.RPC(nameof(SetScore),RpcTarget.AllBuffered,(int)enemyType,id);
        }

        [PunRPC]
        public void SetScore(int enemyTypeInt, int idPlayer)
        {
            EnemyType enemyType = (EnemyType) enemyTypeInt;
            if (idPlayer == 0)
            {
                _scorePlayer1[enemyType]++;
            }
            else
            {
                _scorePlayer2[enemyType]++;
            }
        }
        private void InitDictionary()
        {
            _scorePlayer1.Add(EnemyType.Basic,0);
            _scorePlayer1.Add(EnemyType.Fast,0);
            _scorePlayer1.Add(EnemyType.Power,0);
            _scorePlayer1.Add(EnemyType.Armor,0);
            _scorePlayer2.Add(EnemyType.Basic,0);
            _scorePlayer2.Add(EnemyType.Fast,0);
            _scorePlayer2.Add(EnemyType.Power,0);
            _scorePlayer2.Add(EnemyType.Armor,0);
        }
    }
}
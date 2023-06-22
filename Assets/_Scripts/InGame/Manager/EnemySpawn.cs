using System.Collections;
using ExitGames.Client.Photon;
using _Scripts.InGame.Message;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace _Scripts.InGame.UI
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Transform[] _spawnPoint;
        private EnemyType[] _enemies;
        private int _lastPointSpawn;
        private int _countInMap;
        private int _point;
        private bool _isQueue;
        private float _timeDelaySpawn = 1f;
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _enemies = _enemyData.GetEnemies(PlayerPrefs.GetInt("level", 1));
                _lastPointSpawn = 0;
                _point = 0;
                _countInMap = 0;
                StartCoroutine(SpawnEnemy());
                Enemy.OnEnemyDie += HandleOnEnemyDie;
            }

            
        }

        private void OnDestroy()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Enemy.OnEnemyDie -= HandleOnEnemyDie;
            }
        }

        private void HandleOnEnemyDie(EnemyType enemyType,int id)
        {
            if (_point == 20)
            {
                MessageBroker.Default.Publish(new EndGame{isVictory = true});
                return;
            }

            _countInMap--;
            if (!_isQueue)
            {
                StartCoroutine(SpawnEnemy());
            }
        }

        IEnumerator SpawnEnemy()
        {
            _isQueue = true;
            while (_countInMap<4)
            {
                GameObject go=PhotonNetwork.Instantiate("SpawnEnemy", _spawnPoint[_lastPointSpawn].position,
                    Quaternion.identity);
                go.GetComponent<SpawnEnemyObject>().Init(_enemies[_point].ToString());
                _lastPointSpawn = (_lastPointSpawn == _spawnPoint.Length - 1) ? 0 : _lastPointSpawn+1;
                _countInMap++;
                _point++;
                SetMasterPropertiesEnemyCount();
                yield return new WaitForSeconds(_timeDelaySpawn);
            }
            _isQueue = false;
        }

        private void SetMasterPropertiesEnemyCount()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Hashtable hash = new Hashtable();
                hash.Add("enemyCount", 20 - _point);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }
    }
}
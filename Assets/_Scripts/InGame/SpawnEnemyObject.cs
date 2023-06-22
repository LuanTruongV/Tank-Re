using Photon.Pun;
using UnityEngine;

namespace _Scripts.InGame
{
    public class SpawnEnemyObject : MonoBehaviour
    {
        private string _enemyType;

        public void Init(string enemyType)
        {
            _enemyType = enemyType;
        }
        //AnimationEvent
        public void Spawn()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(_enemyType, transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
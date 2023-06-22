using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.InGame.UI
{
    public class EnemyImage : MonoBehaviourPunCallbacks
    {
        private const int ENEMY_COUNT_IN_MAP = 20;
        [SerializeField] private GameObject _enemyImagePrefab;
        private Stack<GameObject> _stack = new Stack<GameObject>();
        [SerializeField]private Transform _transformParent;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("enemyCount",ENEMY_COUNT_IN_MAP);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            }
            CreateEnemyImage();
        }

        

        private void CreateEnemyImage()
        {
            for (int i = 0; i < ENEMY_COUNT_IN_MAP; i++)
            {
                _stack.Push(Instantiate(_enemyImagePrefab,_transformParent));
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
            if (targetPlayer.IsMasterClient && changedProps.TryGetValue("enemyCount",out object count))
            {
                UpdateImage((int) count);
            }
        }

        private void UpdateImage(int count)
        {
            if (count < 20)
            {
                Destroy(_stack.Pop());
            }
        }
    }
}
using System;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Scripts.InGame
{
    public class Enemy : MonoBehaviourPunCallbacks, IDamageable ,IEnemy
    {
        [SerializeField] private EnemyType _enemyType;
        public static Action<EnemyType,int> OnEnemyDie;
        [Header("Moving")] [SerializeField] private Moving _moving;
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _checkLayer;
        [Header("Health")] [SerializeField] private int _maxHealth;
        [SerializeField] private Animator _animator;


        [Header("Attack")] [SerializeField] private Attacker _attacker;
        [SerializeField] private int _dame;
        [SerializeField] private float _speedBullet;

        private int _currentHealth;
        private Vector2[] directionArray = new Vector2[4] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
        private Vector2 _moveDirection;
        private static readonly int Health = Animator.StringToHash("Health");
        private int _idPlayer;

        public void TakenDame(int dame)
        {
            photonView.RPC(nameof(TakenDameRPC), RpcTarget.MasterClient, dame);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                _currentHealth = _maxHealth;
                _moveDirection = Vector2.down;
            }
        }

        private void RandomDirection()
        {
            _moveDirection = directionArray[Random.Range(0, 4)];
        }

        private bool CheckDirection()
        {
            Vector3 square = 0.3f * _moveDirection;
            return !Physics2D.Raycast(transform.position, _moveDirection, 0.9f, _checkLayer) &&
                   !Physics2D.Raycast(transform.position + square, _moveDirection, 0.9f, _checkLayer) &&
                   !Physics2D.Raycast(transform.position - square, _moveDirection, 0.9f, _checkLayer);
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            Move();
            _attacker.Shoot(_dame,transform.up,_speedBullet);
        }

        private void Move()
        {
            if (CheckDirection())
            {
                _moving.TryMove(_moveDirection, _speed);
            }
            else
            {
                RandomDirection();
            }
        }

        [PunRPC]
        public void TakenDameRPC(int dame)
        {
            _currentHealth--;
            _animator.SetFloat(Health,_currentHealth);
            if (_currentHealth <= 0)
            {
                OnEnemyDie?.Invoke(_enemyType,_idPlayer);
                
                PhotonNetwork.Destroy(gameObject);
                GameObject score=PhotonNetwork.Instantiate("Score" + _enemyType.ToString(), transform.position, Quaternion.identity);
                DOVirtual.DelayedCall(1f, () =>
                {
                    PhotonNetwork.Destroy(score);
                });
                PhotonNetwork.Instantiate("BigEx", transform.position, Quaternion.identity);
            }
        }

        public void SetPlayer(Player player)
        {
            int id = player.IsMasterClient ? 0 : 1;
            photonView.RPC(nameof(SetPlayerRPC),RpcTarget.MasterClient,id);
        }

        [PunRPC]
        public void SetPlayerRPC(int id)
        {
            _idPlayer = id;
        }
    }
}
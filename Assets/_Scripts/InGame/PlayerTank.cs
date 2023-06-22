using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace _Scripts.InGame
{
    public class PlayerTank : MonoBehaviourPunCallbacks,IDamageable
    {
        public static Action<Photon.Realtime.Player> OnPlayerDie;
        [SerializeField] private Moving _moving;
        [SerializeField] private Level _level;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            SetColor();
        }

        private void SetColor()
        {
            if (photonView.IsMine != PhotonNetwork.IsMasterClient)
            {
                _spriteRenderer.color=Color.green;
            }
            
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _moving.TryMove(moveVector,_level.GetSpeed());
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _attacker.Shoot(_level.GetDame(),transform.up,_level.GetSpeedBullet());
            } 
            
        }

        public void TakenDame(int dame)
        {
            photonView.RPC(nameof(TakenDameRPC),RpcTarget.AllBuffered,dame);
        }

        [PunRPC]
        public void TakenDameRPC(int dame)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            OnPlayerDie?.Invoke(photonView.Owner);
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Instantiate("BigEx", transform.position, Quaternion.identity);
        }
    }
}
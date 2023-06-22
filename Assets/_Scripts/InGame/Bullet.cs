
using System;
using _Scripts.InGame.Message;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Scripts.InGame
{
    public class Bullet : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
    
        private int _dame;
        
        private bool _isDestroyed=false;
        public void Init(int dame, LayerMask layer,Vector2 direction,float speed)
        {
            _dame = dame;
            gameObject.layer = layer;
            transform.up = direction;
            _rigidbody2D.velocity = direction * speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (other.gameObject.TryGetComponent<IEnemy>(out IEnemy enemy))
            {
                enemy.SetPlayer(photonView.Owner);
            }
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakenDame(_dame);
            }
            if (other.gameObject.CompareTag("Brick"))
            {
                DestroyMap(other.gameObject);
            }
            if (_dame >= 2 && other.gameObject.CompareTag("Stone"))
            {
                DestroyMap(other.gameObject);
            }
            if (_isDestroyed)
            {
                return;
            }
            _isDestroyed = true;
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Instantiate("BulletExplosion", transform.position, Quaternion.identity);
        }

        

        private void DestroyMap(GameObject gameObject)
        {
            MessageBroker.Default.Publish(new Destroy
            {
                go = gameObject
            });
        }
    }
}
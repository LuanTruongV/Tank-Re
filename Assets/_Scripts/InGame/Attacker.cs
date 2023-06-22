using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.InGame
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private Transform _spawnBulletPoint;
        [SerializeField] private float _timeDelay;
        [SerializeField] private string _layerBullet;
        private GameObject _bullet;
        private float _lastTime;
    
        private Time _lastTimeShoot;

        public void Shoot(int dame,Vector2 direction,float speed)
        {
            if (_bullet != null)
            {
                return;
            }

            if (Time.time < _lastTime + _timeDelay)
            {
                return;
            }
            _lastTime = Time.time;
            _bullet=PhotonNetwork.Instantiate("bullet", _spawnBulletPoint.position, quaternion.identity);
            Bullet bullet = _bullet.GetComponent<Bullet>();
            bullet.Init(dame,LayerMask.NameToLayer(_layerBullet),direction,speed);
        
        }
    }
}
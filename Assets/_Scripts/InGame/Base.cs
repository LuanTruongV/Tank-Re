using System;
using _Scripts.InGame.Message;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace _Scripts.InGame
{
    public class Base : MonoBehaviour
    {
        [SerializeField]private GameObject[] _bricks = new GameObject[10];
        [SerializeField]private GameObject _base;
        [SerializeField]private GameObject _baseDestroyed;
        private void OnTriggerEnter2D(Collider2D other)
        {
            MessageBroker.Default.Publish(new EndGame{isVictory = false});
        }

        
    }
}


using System.Collections.Generic;
using _Scripts.InGame.Message;
using _Scripts.InGame.UI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;

namespace _Scripts.InGame
{
    public class GameOver : MonoBehaviourPunCallbacks
    {
        [SerializeField] private EndPanel _endPanel;
        [SerializeField] private GameObject _bgGameOver;
        [SerializeField] private RectTransform _gameOverRectTransform;
        private List<bool> end = new List<bool>();

        private void Awake()
        {
            MessageBroker.Default.Receive<PlayerEnd>().Where(x => Equals(x.player, PhotonNetwork.LocalPlayer))
                .Subscribe(x => CheckGameOver(x.player)).AddTo(this);

            MessageBroker.Default.Receive<EndGame>().Subscribe(x => EndGame(x.isVictory)).AddTo(this);
            InitListPlayerEnd();
        }


        private void EndGame(bool isVictory)
        {
            photonView.RPC(nameof(EndGameRPC), RpcTarget.AllBuffered, isVictory);
        }


        [PunRPC]
        public void EndGameRPC(bool isVictory)
        {
            _endPanel.SetVictory(isVictory);
            Time.timeScale = 0;
            if (!isVictory)
            {
                _bgGameOver.SetActive(true);
                _gameOverRectTransform.DOLocalMoveY(0, 5f).SetUpdate(true).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        _bgGameOver.SetActive(false);
                        _endPanel.gameObject.SetActive(true);
                    });
                });
            }
            else
            {
                _endPanel.gameObject.SetActive(true);
                int level = PlayerPrefs.GetInt("level", 1);
                level++;
                PlayerPrefs.SetInt("level", level);
            }
        }

        private void InitListPlayerEnd()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    end.Add(false);
                }
            }
        }

        private void CheckGameOver(Player player)
        {
            int id = PhotonNetwork.IsMasterClient ? 0 : 1;
            photonView.RPC(nameof(PlayerEndRPC), RpcTarget.MasterClient, id);
        }

        [PunRPC]
        public void PlayerEndRPC(int idPlayer)
        {
            end[idPlayer] = true;
            if (!end.Contains(false))
            {
                EndGame(false);
            }
        }
    }
}
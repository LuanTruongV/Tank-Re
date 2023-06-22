
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.InGame.UI
{
    public class EndPanel: MonoBehaviour
    {
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private ScoreItem _player1Score;
        [SerializeField] private ScoreItem _player2Score;
        private bool _isVictory;
        public void SetVictory(bool isVicotry)
        {
            _isVictory = isVicotry;
        }

        private void Start()
        {
            
            _player1Score.Init(_scoreManager.ScorePlayer1);
            _player1Score.gameObject.SetActive(true);
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                _player2Score.Init(_scoreManager.ScorePlayer2);
                _player2Score.gameObject.SetActive(true);
            }


            DOVirtual.DelayedCall(10f, () =>
            {
                Time.timeScale = 1;
                if (_isVictory)
                {
                    PhotonNetwork.AutomaticallySyncScene = true;
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.LoadLevel("Scenes/Game");
                    }
                }
                else
                {
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene("Scenes/Lobby");
                }
            });
            
            
        }
    }
}
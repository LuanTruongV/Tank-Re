using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.InGame.UI
{
    
    public class ScoreItem : MonoBehaviour
    {
        private const int SCORE_BASIC = 100;
        private const int SCORE_FAST = 200;
        private const int SCORE_POWER = 300;
        private const int SCORE_ARMOR = 400;
        private Dictionary<EnemyType, int> _playerScore;
        [SerializeField] private BoardItem _basic;
        [SerializeField] private BoardItem _fast;
        [SerializeField] private BoardItem _power;
        [SerializeField] private BoardItem _armor;
        [SerializeField] private GameObject _dash;
        [SerializeField] private GameObject _total;
        [SerializeField] private TMP_Text _totalScore;

        public void Init(Dictionary<EnemyType, int> score)
        {
            _playerScore = score;
        }

        private void Start()
        {
            int total = _playerScore[EnemyType.Basic] * SCORE_BASIC + _playerScore[EnemyType.Fast] * SCORE_FAST +
                        _playerScore[EnemyType.Power] * SCORE_POWER + _playerScore[EnemyType.Armor] * SCORE_ARMOR;
            StartDOTWeen(_basic, _playerScore[EnemyType.Basic], SCORE_BASIC, () =>
            {
                StartDOTWeen(_fast, _playerScore[EnemyType.Fast], SCORE_FAST, () =>
                {
                    StartDOTWeen(_power, _playerScore[EnemyType.Power], SCORE_POWER, () =>
                    {
                        StartDOTWeen(_armor, _playerScore[EnemyType.Armor], SCORE_ARMOR, () =>
                        {
                            int i = 0;
                            _dash.SetActive(true);
                            _total.gameObject.SetActive(true);
                            DOTween.To(() => 0, x => i = x, total, 1.5f).SetUpdate(true).OnUpdate(() =>
                            {
                                _totalScore.text = i.ToString();
                            });
                        });
                    });
                });
            });
        }
        private void StartDOTWeen(BoardItem board, int count, int score, TweenCallback callback)
        {
            int i = 0;
            board.gameObject.SetActive(true);
            DOTween.To(() => i, x => i = x, count, 1.5f).SetUpdate(true).OnUpdate(() =>
            {
                board._countText.text = i.ToString();
                board._scoreText.text = (i * score).ToString();
            }).OnComplete(callback);
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace _Scripts.InGame.UI
{
    public class MapCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text _mapCountText;

        private void Start()
        {
            _mapCountText.text = PlayerPrefs.GetInt("level", 1).ToString();
        }
    }
}
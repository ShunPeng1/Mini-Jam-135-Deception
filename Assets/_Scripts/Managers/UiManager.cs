using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scrollText;
        [SerializeField] private TMP_Text _loseScrollText;

        private void Start()
        {
            DataManager.Instance.OnScrollChange += () =>
            {
                _scrollText.text = DataManager.Instance.ScrollCount.ToString();
                _loseScrollText.text = DataManager.Instance.ScrollCount.ToString();
            };
        }

        
    }
}
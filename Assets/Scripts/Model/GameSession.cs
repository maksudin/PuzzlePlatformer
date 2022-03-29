using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _localData;

        public PlayerData Data
        {
            get { return _localData; }
            set { _localData = value; }
        }

        private void Awake()
        {
            LoadHUD();

            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private bool IsSessionExit()
        {
            GameSession[] sessions = FindObjectsOfType<GameSession>();
            foreach (GameSession session in sessions)
            {
                if (session != this)
                {
                    return true;
                }
            }
            return false;
        }
    }
}


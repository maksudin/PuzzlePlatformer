using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _localData;
        [SerializeField] private PlayerData _savedData;

        public PlayerData LocalData
        {
            get { return _localData; }
            set { _localData = value; }
        }

        public PlayerData SavedData
        {
            get { return _localData; }
        }

        public void SavePlayer()
        {
            _savedData = _localData.ShallowCopy();
        }


        private void Awake()
        {
            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
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


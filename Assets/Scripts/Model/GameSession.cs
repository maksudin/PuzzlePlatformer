using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _localData;

        public QuickInventoryModel QuickInventory { get; private set; }

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
                InitModels();
                LoadHP();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private void LoadHP()
        {
            Data.Hp.Value = DefsFacade.I.Player.MaxHealth;
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


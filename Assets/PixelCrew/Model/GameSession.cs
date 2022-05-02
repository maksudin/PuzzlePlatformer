using System.Collections.Generic;
using System.Linq;
using Assets.PixelCrew.Model.Data.Models;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;

        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private readonly List<string> _checkpoints = new List<string>();

        public PlayerData Data
        {
            get { return _data; }
            set { _data = value; }
        }


        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                Destroy(gameObject);
            }
            else
            {
                InitModels();
                LoadHP();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string _defaultCheckPoint)
        {
            SetChecked(_defaultCheckPoint);
            LoadHUD();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                // TODO: Save();
                _checkpoints.Add(id);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private void LoadHP()
        {
            Data.Hp.Value = DefsFacade.I.Player.MaxHealth;
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return gameSession;
                }
            }
            return null;
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }
    }
}


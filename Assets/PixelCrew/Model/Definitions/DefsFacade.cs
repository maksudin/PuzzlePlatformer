﻿using Assets.PixelCrew.Model.Definitions.Controls;
using Assets.PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Items;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;
        [SerializeField] private ControlsDef _controlIcons;
        [SerializeField] private ControlsRepository _defaultControlsRepository;

        public ItemsRepository Items => _items;
        public PotionRepository Potions => _potions;
        public PerkRepository Perks => _perks;
        public ThrowableRepository Throwable => _throwableItems;
        public PlayerDef Player => _player;
        public ControlsDef ControlIcons => _controlIcons;
        public ControlsRepository DefaultControlsRepository => _defaultControlsRepository;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }

    }
}
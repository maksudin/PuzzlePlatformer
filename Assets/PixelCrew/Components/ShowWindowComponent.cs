﻿using PixelCrew.Utils;
using UnityEngine;

namespace Assets.PixelCrew.Components
{
    public class ShowWindowComponent : MonoBehaviour
    {
        [SerializeField] private string _path;
        public void Show() => WindowUtils.CreateWindow(_path);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_BattleTimeline : MonoBehaviour
    {
        [SerializeField] private static readonly int TOTAL_KOKU = 25;
        [SerializeField] private HorizontalLayoutGroup[] _horizontalLayouts;
        
        public HorizontalLayoutGroup[] horizontalLayouts { get => _horizontalLayouts; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_BattleTimelineIcon : MonoBehaviour
    {
        [SerializeField] private UI_BattleTimeline _timeline;
        [SerializeField] private UnitObject _unit;
        [SerializeField] private Transform _transform;
        [SerializeField] private Image _image;

        private void Awake()
        {
            _transform = transform;
        }

        public void Initialize(UI_BattleTimeline timeline, UnitObject unit)
        {
            _timeline = timeline;
            _image.sprite = unit.spriteRenderer.sprite;
            _unit = unit;
            
            unit.param.OnAPChanged += UpdatePositionOnTimeline;
        }
        
        public void UpdatePositionOnTimeline(UnitObject _)
        {
            int index = 20 - _unit.param.AP;
            _transform.SetParent(_timeline.horizontalLayouts[index].gameObject.transform, false);
        }
    }
}
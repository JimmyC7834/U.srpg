using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Unit
{
    public class UnitTimelineIconController : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        [SerializeField] private GameObject _timelineIconPrefab;
        [SerializeField] private ObjectPool<UI_BattleTimelineIcon> _iconPool;

        private void Awake()
        {
            _iconPool = new ObjectPool<UI_BattleTimelineIcon>(
                CreateNewIcon,
                PoolIcon,
                ReleaseIcon
            );
        }

        public void RegisterIconOn(UnitObject _unit)
        {
            UI_BattleTimelineIcon newIcon = _iconPool.Get();
            newIcon.Initialize(_battleService.uiManager.timeline, _unit);
            newIcon.UpdatePositionOnTimeline(_unit);
        }
        
        private UI_BattleTimelineIcon CreateNewIcon()
        {
            return Instantiate(_timelineIconPrefab).GetComponent<UI_BattleTimelineIcon>();
        }

        private void PoolIcon(UI_BattleTimelineIcon icon)
        {
            icon.gameObject.SetActive(true);
        }
        
        private void ReleaseIcon(UI_BattleTimelineIcon icon)
        {
            icon.gameObject.SetActive(false);
        }
    }
}
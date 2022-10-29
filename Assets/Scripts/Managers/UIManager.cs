using System;
using Game.Battle;
using Game.UI;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;

        [SerializeField] private UI_SkillSelectionMenu _skillSelectionMenu;
        [SerializeField] private UI_UnitInfoSEPanel _unitInfoSePanel;
        [SerializeField] private UI_BattleTimeline _timeline;
        [SerializeField] private UI_UnitInfoStatusPanel _unitInfoStatusPanel;
        [SerializeField] private GameObject _actionMenu;

        [SerializeField] private GameObject _damageIndicatorPrefab;
        private GameObjectPool<UI_DamageIndicator> _damageIndicatorPool;
        
        public UI_BattleTimeline timeline { get => _timeline; }
        
        private void Awake()
        {
            _damageIndicatorPool = new GameObjectPool<UI_DamageIndicator>(_damageIndicatorPrefab, transform);
        }

        public void Initialize()
        {
            _unitInfoSePanel.Initialize(_battleService);
            _unitInfoStatusPanel.Initialize(_battleService);
        }

        public void OpenSkillSelectionMenu(UnitObject unit, Action<SkillSO> callback)
        {
            // TODO: handle no skills
            _skillSelectionMenu.OpenMenu(unit.partTree.GetAllSkills(), callback);
            EventSystem.current.SetSelectedGameObject(_skillSelectionMenu.items[0].gameObject);
        }
        
        public void ToggleActionMenu(bool value) => _actionMenu.gameObject.SetActive(value);

        public void CreateDamageIndicator(Vector3 worldPosition, int value)
        {
            UI_DamageIndicator indicator = _damageIndicatorPool.Get(
                (i) => i.Initialize(worldPosition, value));
        }
    }
}
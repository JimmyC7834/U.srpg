using System;
using Game.UI;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
    /**
     * Provide interface ro manage the UI used in a battle
     */
    public class BattleUIManager : UI_ViewController
    {
        [SerializeField] private BattleService _battleService;

        [SerializeField] private UI_SkillSelectionMenu _skillSelectionMenu;
        [SerializeField] private UI_UnitInfoSEPanel _unitInfoSePanel;
        [SerializeField] private UI_BattleTimeline _timeline;
        [SerializeField] private UI_UnitInfoStatusPanel _unitInfoStatusPanel;
        [SerializeField] private UI_PlayerBattleActionMenu _actionMenu;

        [SerializeField] private UI_DamageIndicator _prefab;
        
        // private UI_ViewController _viewController;
        private GameObjectPool<UI_DamageIndicator> _damageIndicatorPool;

        public UI_BattleTimeline timeline { get => _timeline; }
        
        private void Awake()
        {
            // _viewController = new UI_ViewController();
            _damageIndicatorPool = new GameObjectPool<UI_DamageIndicator>(_prefab, transform);
        }

        public void Initialize()
        {
            PushView(_unitInfoSePanel);
            PushView(_unitInfoStatusPanel);
            PushView(_timeline);
        }

        public void OpenSkillSelectionMenu(UnitObject unit, Action<SkillSO> callback)
        {
            // TODO: handle no skills
            _skillSelectionMenu.OpenMenu(unit.partTree.GetAllSkills(), (skill) =>
            {
                callback.Invoke(skill);
                PopView();
            });
            PushView(_skillSelectionMenu);
        }
        
        public void ToggleActionMenu(bool value) => _actionMenu.gameObject.SetActive(value);

        public void CreateDamageIndicator(Vector3 worldPosition, int value)
        {
            UI_DamageIndicator indicator = _damageIndicatorPool.Get(
                (i) => i.Initialize(worldPosition, value));
        }
    }
}
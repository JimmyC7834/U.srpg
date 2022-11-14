using System;
using Game.Battle;
using Game.UI;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game
{
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;

        [SerializeField] private UI_SkillSelectionMenu _skillSelectionMenu;
        [SerializeField] private UI_UnitInfoSEPanel _unitInfoSePanel;
        [SerializeField] private UI_BattleTimeline _timeline;
        [SerializeField] private UI_UnitInfoStatusPanel _unitInfoStatusPanel;
        [SerializeField] private UI_PlayerBattleActionMenu _actionMenu;

        [SerializeField] private UI_DamageIndicator _prefab;
        
        private UI_ViewController _viewController;
        private GameObjectPool<UI_DamageIndicator> _damageIndicatorPool;

        public UI_BattleTimeline timeline { get => _timeline; }
        
        private void Awake()
        {
            _viewController = new UI_ViewController();
            _damageIndicatorPool = new GameObjectPool<UI_DamageIndicator>(_prefab, transform);
        }

        public void Initialize()
        {
            _viewController.PushView(_unitInfoSePanel);
            _viewController.PushView(_unitInfoStatusPanel);
            _viewController.PushView(_timeline);
        }

        public void OpenSkillSelectionMenu(UnitObject unit, Action<SkillSO> callback)
        {
            // TODO: handle no skills
            _skillSelectionMenu.OpenMenu(unit.partTree.GetAllSkills(), (skill) =>
            {
                callback.Invoke(skill);
                _viewController.PopView();
            });
            _viewController.PushView(_skillSelectionMenu);
        }
        
        public void OpenActionMenu() => _viewController.PushExitOnNextPushView(_actionMenu);

        public void CreateDamageIndicator(Vector3 worldPosition, int value)
        {
            UI_DamageIndicator indicator = _damageIndicatorPool.Get(
                (i) => i.Initialize(worldPosition, value));
        }
    }
}
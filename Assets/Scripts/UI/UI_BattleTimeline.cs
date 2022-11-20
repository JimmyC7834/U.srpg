using Game.Battle;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * Controls the Battle Timeline UI behaviour
     */
    public class UI_BattleTimeline : UI_View
    {
        [SerializeField] private BattleService _battleService;
        
        private static readonly int TOTAL_KOKU = 20;
        [SerializeField] private HorizontalLayoutGroup[] _horizontalLayouts;
        [SerializeField] private Slider _kokuIndicator;
        
        [SerializeField] private UI_BattleTimelineIcon _prefab;
        private GameObjectPool<UI_BattleTimelineIcon> _iconPool;
        
        public HorizontalLayoutGroup[] horizontalLayouts { get => _horizontalLayouts; }

        protected override void Enter()
        {
            _iconPool = new GameObjectPool<UI_BattleTimelineIcon>(_prefab, transform);
            _battleService.battleTurnManager.OnKokuChanged += UpdateKokuIndicator;
            _battleService.unitManager.OnUnitSpawned += RegisterIconOn;
            UpdateKokuIndicator(_battleService.battleTurnManager.koku);
        }

        private void UpdateKokuIndicator(int koku)
        {
            _kokuIndicator.value = TOTAL_KOKU - koku;
        }

        private void RegisterIconOn(UnitObject unit)
        {
            _iconPool.Get(
                obj => obj.Initialize(_battleService.BattleUIManager.timeline, unit));
        }
    }
}
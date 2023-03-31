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
            _image.sprite = unit.unitSO.Sprite;
            _unit = unit;
            
            unit.Stats.OnAPChanged += UpdatePositionOnTimeline;
            
            UpdatePositionOnTimeline(unit);
        }
        
        public void UpdatePositionOnTimeline(UnitObject _)
        {
            int index = 20 - _unit.Stats.AP;
            _transform.SetParent(_timeline.horizontalLayouts[index].gameObject.transform, false);
        }
    }
}
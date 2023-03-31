using Game.Battle;
using Game.DataSet;
using Game.Unit.Part;
using UnityEngine;

namespace Game.Unit
{
    [CreateAssetMenu(menuName = "Game/DataEntry/Unit")]
    public class UnitSO : DataEntrySO<UnitId>
    {
        [Header("Metadata")]
        [SerializeField] private Sprite _sprite;
        
        [Header("Properties")]
        [SerializeField] private PartSO[] _partSOs;
        [SerializeField] private CpuUnitAI _ai;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        
        public Sprite Sprite { get => _sprite; }
        public CpuUnitAI AI { get => _ai; }
        public AnimatorOverrideController AnimatorOverrideController { get => _animatorOverrideController; }

        public UnitData Create()
        {
            UnitPart[] parts = new UnitPart[_partSOs.Length];
            for (int i = 0; i < _partSOs.Length; i++)
                parts[i] = _partSOs[i].Create();
            
            return new UnitData(parts);
        }
    }

    public enum UnitId
    {
        None = -1,
        PlayerUnit1 = 1000,
        DebugUnit = 9900,
        Norm = 9910,
        Norm1 = 9920,
        Norm2 = 9930,
        Norm3 = 9940,
        Count = 5
    }
}


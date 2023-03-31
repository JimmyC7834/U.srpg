using System;
using System.Collections.Generic;
using Game.Unit;
using Game.Unit.StatusEffect;
using UnityEngine.Assertions;

namespace Game
{
    /**
     * Manager of Status Effects of an unit
     */
    public class UnitSEHandler
    {
        private Dictionary<StatusEffectId, StatusEffectRegister> _seMap;
        private List<StatusEffectRegister> _statusEffects;
        private bool _initialized = false;
        
        public void Initialize(UnitObject unit)
        {
            Assert.IsTrue(!_initialized);
            _initialized = true;
            
            unit.Data.OnPreAttack += (info) => Map((x) => x.OnPreAttack(info));
            unit.Data.OnPostAttack += (info) => Map((x) => x.OnPostAttack(info));
            unit.Data.OnAttackMissed += (info) => Map((x) => x.OnAttackMissed(info));
            unit.Data.OnAttackDodged += (info) => Map((x) => x.OnAttackDodged(info));
            unit.Data.OnAttackHit += (info) => Map((x) => x.OnAttackHit(info));
            unit.Data.OnPreTakeAttack += (info) => Map((x) => x.OnPreTakeAttack(info));
            unit.Data.OnPostTakeAttack += (info) => Map((x) => x.OnPostTakeAttack(info));
            unit.Data.OnDodgedAttack += (info) => Map((x) => x.OnDodgeAttack(info));
            unit.Data.OnPreTakeDamage += (info) => Map((x) => x.OnPreTakeDamage(info));
            unit.Data.OnPostTakeDamage += (info) => Map((x) => x.OnPostTakeDamage(info));

            _statusEffects = new List<StatusEffectRegister>();
            _seMap = new Dictionary<StatusEffectId, StatusEffectRegister>();
        }
        
        /// <summary>
        /// Register a new SE to the unit. Stack effects if SE already exists.
        /// </summary>
        /// <param name="se"> The SE o register </param>
        public void Register(StatusEffectRegister statusEffectRegister)
        {
            Assert.IsTrue(_initialized);
            if (_seMap.ContainsKey(statusEffectRegister.ID))
            {
                _seMap[statusEffectRegister.ID].StackEffect(1);
                return;
            }
            
            StatusEffectRegister se = statusEffectRegister.Copy();
            _statusEffects.Add(se);
            _seMap.Add(se.ID, se);
            se.OnRegister();
        }

        /// <summary>
        /// Reduce the 1 count of the SE.
        /// </summary>
        /// <param name="se"> The SE to be reduce </param>
        public void Reduce(StatusEffectId id)
        {
            Assert.IsTrue(_initialized);
            if (!_seMap.ContainsKey(id)) return;
            _seMap[id].ReduceEffect(1);
        }
        
        /// <summary>
        /// Remove the SE from unit completely.
        /// </summary>
        /// <param name="id"> The id of the SE to be removed </param>
        public void RemoveAll(StatusEffectId id)
        {
            Assert.IsTrue(_initialized);
            if (!_seMap.ContainsKey(id)) return;
            StatusEffectRegister se = _statusEffects.Find(x => x.ID == id);
            _statusEffects.Remove(se);
            _seMap.Remove(id);
            se.OnRemove();
        }

        private void Map(Action<StatusEffectRegister> map)
        {
            foreach (StatusEffectRegister se in _statusEffects)
                map(se);
        }

        public StatusEffectRegister[] GetStatusEffects() {
            Assert.IsTrue(_initialized);
            return _statusEffects.ToArray();
        }
    }
}
 
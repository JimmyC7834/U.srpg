using System;
using System.Collections.Generic;
using Game.Unit;
using Game.Unit.StatusEffect;

namespace Game
{
    /**
     * Manager of Status Effects of an unit
     */
    public class UnitSEHandler
    {
        private Dictionary<StatusEffectId, StatusEffect> _seMap;
        private List<StatusEffect> _statusEffects;

        public UnitSEHandler(UnitObject unit)
        {
            unit.OnPreAttack += (info) => Map((x) => x.OnPreAttack(info));
            unit.OnPostAttack += (info) => Map((x) => x.OnPostAttack(info));
            unit.OnAttackMissed += (info) => Map((x) => x.OnAttackMissed(info));
            unit.OnAttackDodged += (info) => Map((x) => x.OnAttackDodged(info));
            unit.OnAttackHit += (info) => Map((x) => x.OnAttackHit(info));
            unit.OnPreTakeAttack += (info) => Map((x) => x.OnPreTakeAttack(info));
            unit.OnPostTakeAttack += (info) => Map((x) => x.OnPostTakeAttack(info));
            unit.OnDodgedAttack += (info) => Map((x) => x.OnDodgeAttack(info));
            unit.OnPreTakeDamage += (info) => Map((x) => x.OnPreTakeDamage(info));
            unit.OnPostTakeDamage += (info) => Map((x) => x.OnPostTakeDamage(info));

            _statusEffects = new List<StatusEffect>();
            _seMap = new Dictionary<StatusEffectId, StatusEffect>();
        }
        
        /// <summary>
        /// Register a new SE to the unit. Stack effects if SE already exists.
        /// </summary>
        /// <param name="se"> The SE o register </param>
        public void Register(StatusEffect statusEffect)
        {
            if (_seMap.ContainsKey(statusEffect.ID))
            {
                _seMap[statusEffect.ID].StackEffect(statusEffect);
                return;
            }
            
            StatusEffect se = statusEffect.Copy();
            _statusEffects.Add(se);
            _seMap.Add(se.ID, se);
            se.OnRegister();
        }

        /// <summary>
        /// Reduce the stack count of a SE.
        /// </summary>
        /// <param name="se"> The SE to be reduce </param>
        public void Reduce(StatusEffect se)
        {
            if (!_seMap.ContainsKey(se.ID)) return;
            _seMap[se.ID].ReduceEffect(se);
        }
        
        /// <summary>
        /// Remove the SE from unit completely.
        /// </summary>
        /// <param name="id"> The id of the SE to be removed </param>
        public void RemoveAll(StatusEffectId id)
        {
            if (!_seMap.ContainsKey(id)) return;
            StatusEffect se = _statusEffects.Find(x => x.ID == id);
            _statusEffects.Remove(se);
            _seMap.Remove(id);
            se.OnRemove();
        }

        private void Map(Action<StatusEffect> map)
        {
            foreach (StatusEffect se in _statusEffects)
                map(se);
        }

        public StatusEffect[] GetStatusEffects() => _statusEffects.ToArray();
    }
}
 
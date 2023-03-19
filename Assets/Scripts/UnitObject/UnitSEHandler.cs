using System;
using System.Collections.Generic;
using System.Linq;
using Game.Unit;
using Game.Unit.StatusEffect;

namespace Game
{
    /**
     * Manager of Status Effects of an unit
     */
    public class UnitSEHandler
    {
        private Dictionary<StatusEffectId, StatusEffect> _statusEffects;

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
        }
        
        public void RegisterStatusEffect(StatusEffect se)
        {
            if (_statusEffects.ContainsKey(se.ID))
            {
                _statusEffects[se.ID].StackEffect(se);
                return;
            }
            
            _statusEffects.Add(se.ID, se);
            se.OnRegister();
        }

        public void RemoveStatusEffect(StatusEffectId id)
        {
            if (!_statusEffects.ContainsKey(id)) return;
            _statusEffects[id].OnRemove();
            _statusEffects.Remove(id);
        }

        private void Map(Action<StatusEffect> map)
        {
            foreach (StatusEffect se in _statusEffects.Values)
                map(se);
        }

        public StatusEffect[] GetStatusEffects() => _statusEffects.Values.ToArray();
    }
}
 
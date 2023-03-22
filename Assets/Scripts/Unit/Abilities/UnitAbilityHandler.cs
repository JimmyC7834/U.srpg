using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Unit.Ability
{
    /// <summary>
    /// A manager class that handles all the abilities of an unit.
    /// </summary>
    public class UnitAbilityHandler
    {
        private Dictionary<AbilityID, Ability> _abilities;

        public UnitAbilityHandler(UnitObject unit)
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
        
        /// <summary>
        /// Register the ability to the unit.
        /// If the ability exists, the count of the ability increase by 1.
        ///
        /// Invokes OnRegister of the ability
        /// when the ability is added for the first time.
        /// </summary>
        /// <param name="ab"></param>
        public void Register(Ability ab)
        {
            if (_abilities.ContainsKey(ab.ID))
            {
                _abilities[ab.ID].Stack(1);
                return;
            }
            
            _abilities.Add(ab.ID, ab);
        }

        /// <summary>
        /// Reduce count of ability of the unit by 1.
        /// Remove the ability from the unit completely
        /// if the stack count of the ability turns 0.
        /// </summary>
        /// <param name="id"> The id of the ability to reduce. </param>
        public void Reduce(AbilityID id)
        {
            if (!_abilities.ContainsKey(id)) return;
            
            _abilities[id].Stack(-1);
            if (_abilities[id].Count <= 0)
                Remove(id);
        }
        
        /// <summary>
        /// Remove the ability from the unit completely.
        /// Invoke OnRemove of the ability when it's removed.
        /// </summary>
        /// <param name="id"> The id of the ability to remove. </param>
        public void Remove(AbilityID id)
        {
            if (!_abilities.ContainsKey(id)) return;
            _abilities.Remove(id);
        }

        // Maps the function to all abilities on the unit
        // Used to invoke unit event call functions of each ability
        private void Map(Action<Ability> map)
        {
            foreach (Ability ability in _abilities.Values)
                map(ability);
        }

        public Ability[] GetAbilities() => _abilities.Values.ToArray();
    }
}
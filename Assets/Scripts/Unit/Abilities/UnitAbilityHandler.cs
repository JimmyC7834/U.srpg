using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Game.Unit.Ability
{
    /// <summary>
    /// A manager class that handles all the abilities of an unit.
    /// </summary>
    public class UnitAbilityHandler
    {
        private readonly Dictionary<AbilityID, UnitAbility> _abilities;
        private bool _initialized = false;
        private UnitObject _unit;

        public UnitAbilityHandler()
        {
            _abilities = new Dictionary<AbilityID, UnitAbility>();
        }
        
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

            _unit = unit;
        }
        
        /// <summary>
        /// Register the ability to the unit.
        /// If the ability exists, the count of the ability increase by 1.
        ///
        /// Invokes OnRegister of the ability
        /// when the ability is added for the first time.
        /// </summary>
        /// <param name="ab"></param>
        public void Register(UnitAbility ab)
        {
            Assert.IsTrue(_initialized);
            if (ab == null) return;
            if (_abilities.ContainsKey(ab.ID))
            {
                _abilities[ab.ID].Stack(1);
                return;
            }
            
            ab.RegisterTo(_unit);
            _abilities.Add(ab.ID, ab);
        }

        public void Register(UnitAbility[] abs)
        {
            Assert.IsTrue(_initialized);
            for (int i = 0; i < abs.Length; i++)
                Register(abs[i]);
        }

        /// <summary>
        /// Reduce count of ability of the unit by 1.
        /// Remove the ability from the unit completely
        /// if the stack count of the ability turns 0.
        /// </summary>
        /// <param name="id"> The id of the ability to reduce. </param>
        public void Reduce(AbilityID id)
        {
            Assert.IsTrue(_initialized);
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
            Assert.IsTrue(_initialized);
            if (!_abilities.ContainsKey(id)) return;
            _abilities.Remove(id);
        }

        // Maps the function to all abilities on the unit
        // Used to invoke unit event call functions of each ability
        private void Map(Action<UnitAbility> map)
        {
            foreach (UnitAbility ability in _abilities.Values)
                map(ability);
        }

        public UnitAbility[] GetAbilities()
        {
            Assert.IsTrue(_initialized);
            return _abilities.Values.ToArray();
        }
    }
}
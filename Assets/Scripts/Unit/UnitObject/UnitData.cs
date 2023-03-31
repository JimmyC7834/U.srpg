using System;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using UnityEngine.InputSystem.Utilities;

namespace Game.Unit
{
    
    public class UnitData
    {
        public readonly ReadOnlyArray<Optional<UnitPart>> Parts;
        public readonly ReadOnlyArray<SkillSO> Skills;
        public readonly UnitAbilityHandler AbilityHandler;
        public readonly UnitSEHandler SEHandler;
        public readonly UnitStats Stats;

        #region UnitEvents

        public event Action OnActionStart = delegate { };
        public event Action OnActionEnd = delegate { };

        public event Action<AttackInfo> OnPreAttack = delegate { };
        public event Action<AttackInfo> OnPostAttack = delegate { };
        public event Action<AttackInfo> OnAttackHit = delegate { };
        public event Action<AttackInfo> OnAttackDodged = delegate { };
        public event Action<AttackInfo> OnAttackMissed = delegate { };
        public event Action<AttackInfo> OnPreTakeAttack = delegate { };
        public event Action<AttackInfo> OnPostTakeAttack = delegate { };
        public event Action<AttackInfo> OnDodgedAttack = delegate { };
        public event Action<DamageInfo> OnPreTakeDamage = delegate { };
        public event Action<DamageInfo> OnPostTakeDamage = delegate { };
        
        public event Action<UnitObject> OnSETurnChanged = delegate { };
        public event Action<UnitObject> OnSEKokuChanged = delegate { };
        public event Action<UnitObject> OnSEActionEnd = delegate { };
        
        #endregion
        
        public UnitData(UnitPart[] parts)
        {
            Optional<UnitPart>[] partsRegs = new Optional<UnitPart>[parts.Length];
            SkillSO[] skills = new SkillSO[parts.Length];
            AbilityHandler = new UnitAbilityHandler();
            SEHandler = new UnitSEHandler();
            Stats = new UnitStats();

            for (int i = 0; i < parts.Length; i++)
            {
                partsRegs[i] = new Optional<UnitPart>(parts[i]);
                // skills[i] = parts[i].GetSkill();
            }

            Parts = partsRegs;
            Skills = skills;
        }

        public void Bind(UnitObject unit)
        {
            AbilityHandler.Initialize(unit);
            SEHandler.Initialize(unit);
            Stats.Initialize(unit);

            foreach (Optional<UnitPart> part in Parts)
                foreach (UnitAbility ab in part.Value.Abilities)
                    AbilityHandler.Register(ab);
        }
        
        public void EndAction()
        {
            OnActionEnd.Invoke();
        }
        
        public void Attack(AttackInfo attackInfo)
        {
            OnPreAttack.Invoke(attackInfo);
            
            if (!attackInfo.RollHit())
            {
                OnAttackMissed.Invoke(attackInfo);
                return;
            }

            attackInfo.target.Data.TakeAttack(attackInfo);
            OnAttackHit.Invoke(attackInfo);

            OnPostAttack.Invoke(attackInfo);
        }
        
        public void TakeAttack(AttackInfo attackInfo)
        {
            OnPreTakeAttack.Invoke(attackInfo);
            
            if (attackInfo.RollDodged())
            {
                OnDodgedAttack.Invoke(attackInfo);
                return;
            }

            if (attackInfo.RollCritical())
            {
                // TODO: process critical
            }
            
            TakeDamage(attackInfo.damageInfo);
            OnPostAttack.Invoke(attackInfo);
        }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnPreTakeDamage.Invoke(damageInfo);
            UnitParamModifier damageModifier = damageInfo.damageModifier;
            Stats.AddModifier(damageModifier);
            Stats.Evaluate();
            OnPostTakeDamage.Invoke(damageInfo);
        }
    }
}
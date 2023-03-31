using Game.Unit;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using NUnit.Framework;
using Tests.Builders;
using UnityEngine;

namespace Tests
{
    public class Test_UnitData
    {
        private static void assert(bool val)
        {
            Assert.IsTrue(val);
        }
        
        private static UnitDataBuilder EmptyUnit() => Unit_XPart_YAbility_ZCount(0, 0, 0);

        private static UnitDataBuilder Unit_XPart_YAbility_ZCount(int x, int y, int z)
        {
            UnitPart[] parts = new UnitPart[x];
            for (int i = 0; i < x; i++)
                parts[i] = Part_XAbility_YCount(y, z);
            return An.UnitData.WithParts(parts);
        }

        private static PartBuilder Part_XAbility_YCount(int x, int y)
        {
            UnitAbility[] abilities = new UnitAbility[x];
            for (int i = 0; i < x; i++)
                abilities[i] = TestAbility_StackX(y);
            return A.Part.WithId(PartID.None)
                .WithAbilities(abilities)
                .WithSkill(SkillID.None)
                .WithParams(new UnitParam.ParamBoostEntry[] { });
        }

        private static TestAbilityBuilder TestAbility_StackX(int x)
        {
            return A.TestAbility.WithCount(x);
        }

        [Test]
        public static void Test_UnitDataEmpty()
        {
            UnitData emptyUnitData = EmptyUnit();
            assert(emptyUnitData.Parts.Length == 0);
            assert(emptyUnitData.Skills.Length == 0);
            assert(emptyUnitData.AbilityHandler.GetAbilities().Length == 0);
        }
        
        [Test]
        public static void Test_UnitDataPartCount()
        {
            UnitData unit = Unit_XPart_YAbility_ZCount(1, 1, 1);
            assert(unit.Parts.Length == 1);
            
            unit = Unit_XPart_YAbility_ZCount(1, 2, 1);
            assert(unit.Parts.Length == 1);
            
            unit = Unit_XPart_YAbility_ZCount(2, 2, 1);
            assert(unit.Parts.Length == 2);
            
            unit = Unit_XPart_YAbility_ZCount(2, 2, 2);
            assert(unit.Parts.Length == 2);
            
            unit = Unit_XPart_YAbility_ZCount(3, 2, 2);
            assert(unit.Parts.Length == 3);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Test_UnitSEHandler
    {
        private static UnitSO TEST_UNIT_SO;
    
        [Test]
        public static void Test_UnitSEHandlerInitialization()
        {
            UnitObject unit = NewUnit();
            Assert.IsNotNull(unit.Data.SEHandler);
        }

        public static UnitObject NewUnit()
        {
            UnitObject unit = new GameObject().AddComponent<UnitObject>();
            if (TEST_UNIT_SO == null)
                TEST_UNIT_SO = ScriptableObject.CreateInstance<UnitSO>();
            unit.Initialize(TEST_UNIT_SO, "");
            return unit;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Punch", fileName = "Sk_Punch")]
    public class Sk_Punch : SkillSO
    {
        public override int Range()
        {
            return 1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UI_PlayerBattleActionMenu : UI_View
    {
        public override void Enter() => gameObject.SetActive(true);
        public override void Exit() => gameObject.SetActive(false);
    }
}
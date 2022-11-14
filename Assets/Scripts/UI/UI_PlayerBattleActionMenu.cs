using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UI_PlayerBattleActionMenu : UI_View
    {
        protected override void Enter() => gameObject.SetActive(true);
        protected override void Exit() => gameObject.SetActive(false);
    }
}
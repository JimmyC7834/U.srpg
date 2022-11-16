using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game.Battle;
using UnityEngine;

namespace Game.UI
{
    public class UI_PlayerBattleActionMenu : UI_View
    {
        [SerializeField] private BattleService _battleService;
        [SerializeField] private InputReader _input;

        protected override void Enter()
        {
            CinemachineVirtualCamera cam = _battleService.camera;
            cam.ForceCameraPosition(
                _battleService.CurrentCoord.GameV2ToV3(), cam.transform.rotation);
            gameObject.SetActive(true);
        }

        protected override void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}
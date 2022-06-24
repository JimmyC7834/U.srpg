using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;

        [SerializeField] private UI_SkillSelectionMenu _skillList;

        [SerializeField] private GameObject _damageIndicatorPrefab;
        private ObjectPool<UI_DamageIndicator> _damageIndicatorPool;

        private void Awake()
        {
            _damageIndicatorPool = new ObjectPool<UI_DamageIndicator>(
                () => Instantiate(_damageIndicatorPrefab).GetComponent<UI_DamageIndicator>(),
                (item) => item.gameObject.SetActive(true),
                (item) => item.gameObject.SetActive(false)
                );
        }

        public void OpenSkillSelectionMenu(UnitObject unit, Action<UI_SkillSelectionMenuItem> callback)
        {
            // TODO: handle no skills
            _skillList.OpenMenu(unit.partTree.GetAllSkills(), (item) => callback.Invoke(item));
            _eventSystem.SetSelectedGameObject(_skillList.GetItemAt(0).gameObject);
            callback += (_) => _skillList.CloseMenu();
        }

        public void CreateDamageIndicator(Vector3 worldPosition, int value)
        {
            UI_DamageIndicator indicator = _damageIndicatorPool.Get();
            indicator.Initialize(worldPosition, value);
        }
    }
}
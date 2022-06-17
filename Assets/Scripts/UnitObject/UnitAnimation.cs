using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Unit;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private UnitObject _unit;
    
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    // public IEnumerator Move(Vector2 targetCoord, float time) => StartCoroutine(Move(targetCoord, time));
    
    public IEnumerator Move(Vector2 targetCoord, float time)
    {
        Vector2 startPosition = _unit.location;
        Vector2 currentPosition = startPosition;
        RaycastHit hit;
        float startTime = Time.time;

        while (Vector2.Distance(targetCoord, currentPosition) > 0.01f)
        {
            currentPosition = Vector2.Lerp(startPosition, targetCoord, (Time.time - startTime)/time);
            Physics.Raycast(new Ray(new Vector3(currentPosition.x + .5f, 10, currentPosition.y + .5f), Vector3.down), out hit, 20f);
            _unit.transform.position = currentPosition.GameV2ToV3() + hit.point.ExtractY();
                
            yield return null;
        }
        
        Physics.Raycast(new Ray(new Vector3(currentPosition.x + .5f, 10, currentPosition.y + .5f), Vector3.down), out hit, 20f);
        _unit.transform.position = currentPosition.GameV2ToV3() + hit.point.ExtractY();
        yield return null;
    }
}

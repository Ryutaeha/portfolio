using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    public Dictionary<DestroyedObject, (bool isDone, bool isClick)> childStates = new Dictionary<DestroyedObject, (bool, bool)>();
    private int doneCount = 0;
    [SerializeField] private int allowCount = 2;

    private void OnEnable()
    {
        foreach (var child in GetComponentsInChildren<DestroyedObject>())
        {
            child.OnStateChange += HandleStateChange;
        }
    }

    private void OnDisable()
    {
        foreach (var child in GetComponentsInChildren<DestroyedObject>())
        {
            child.OnStateChange -= HandleStateChange;
        }
    }

    private void HandleStateChange(DestroyedObject child, bool isDone, bool isClick)
    {
        if (childStates.TryGetValue(child, out var state))
        {
            if (isDone && !state.isDone)
            {
                doneCount++;
            }
            else if (!isDone && state.isDone)
            {
                doneCount--;
            }
        }

        childStates[child] = (isDone, isClick);

        List<DestroyedObject> childrenToChange = new List<DestroyedObject>();

        foreach (var childObject in childStates.Keys)
        {
            if (isClick && !childObject.isClick) // 이미 isClick 상태인 자식은 제외
            {
                childrenToChange.Add(childObject);
            }
            else if (!isClick && childObject.isClick) // 이미 !isClick 상태인 자식은 제외
            {
                childrenToChange.Add(childObject);
            }
        }

        foreach (var childObject in childrenToChange)
        {
            childObject.Recording(childStates[childObject].isDone, isClick);
        }
        if (child.Type == ObjectPropertyType.DESTROY && doneCount == childStates.Count )
        {
            //Todo 만약 우클릭시 파괴 없앨꺼면 여기 지우면 됨
            child.Destroying();
        }

        if (child.Type == ObjectPropertyType.RESTORE && doneCount + allowCount == childStates.Count)
        {
            child.Restoring();
        }
    }

}

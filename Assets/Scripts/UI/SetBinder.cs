using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

// UI 네임스페이스
namespace UI
{
    // Binder 클래스는 UI 컴포넌트를 관리하는 클래스입니다.
    // 이 클래스는 UI 컴포넌트를 찾아서 저장하고, 필요할 해당 컴포넌트를 반환하는 역할을 합니다.
    public class Binder
    {
        // _objects 딕셔너리는 UI 컴포넌트를 저장합니다.
        // 이 딕셔너리의 키는 컴포넌트의 타입이고, 값은 또 다른 딕셔너리입니다.
        // 이때 내부 딕셔너리의 키는 컴포넌트의 이름이고, 값은 해당 이름의 컴포넌트 인스턴스입니다.
        private readonly Dictionary<Type, Dictionary<string, Object>> _objects = new();

        // Binding 메소드는 주어진 GameObject의 자식 컴포넌트 중 타입이 T인 것들을 찾아서 _objects 딕셔너리에 저장합니다.
        // T는 컴포넌트의 타입이며, parentObject는 컴포넌트를 찾을 부모 GameObject입니다.
        // 이 메소드는 parentObject의 모든 자식 중에서 타입이 T인 컴포넌트를 찾아서 _objects 딕셔너리에 저장하고, 이를 AssignmentComponent 메소드에 전달합니다.
        public void Binding<T>(GameObject parentObject) where T : Object
        {
            T[] objects = parentObject.GetComponentsInChildren<T>(true);
            //Dictionary<string, Object> objectDict = objects.ToDictionary(comp => comp.name, comp => comp as Object);
            Dictionary<string, Object> objectDict = new Dictionary<string, Object>();

            foreach (var comp in objects)
            {
                string key = comp.name;

                // 중복된 키가 없을 때만 딕셔너리에 추가
                if (!objectDict.ContainsKey(key))
                {
                    objectDict.Add(key, comp as Object);
                }
                else
                    Debug.Log($"중복 key 발견 :{key}");
            }

            _objects[typeof(T)] = objectDict;
            AssignmentComponent<T>(parentObject, objectDict);
        }

        // AssignmentComponent 메소드는 _objects 딕셔너리에 저장된 컴포넌트들을 실제 게임 오브젝트에 할당합니다.
        // 이 메소드는 Binding 메소드에서 찾은 컴포넌트들을 실제로 해당 게임 오브젝트에 할당하는 역할을 합니다.
        private void AssignmentComponent<T>(GameObject parentObject, Dictionary<string, Object> objects) where T : Object
        {
            foreach (var key in objects.Keys.ToList())
            {
                if (objects[key] != null) continue;
                Object component = typeof(T) == typeof(GameObject)
                ? FindComponentDirectChild<GameObject>(parentObject, key)
                : FindComponentDirectChild<T>(parentObject, key);

                if (component != null) objects[key] = component;
                else Debug.Log($"Binding failed for Object : {key}");
            }
        }

        // FindComponentDirectChild 메소드는 주어진 GameObject의 직접적인 자식 중 이름이 일치하는 컴포넌트를 찾아 반환합니다.
        // 이 메소드는 parentObject의 직접적인 자식들 중에서 이름이 name과 일치하는 컴포넌트를 찾아서 반환합니다.
        private T FindComponentDirectChild<T>(GameObject parentObject, string name) where T : Object
        {
            return (from Transform child in parentObject.transform
                    where child.name == name
                    select child.GetComponent<T>()).FirstOrDefault();
        }

        // Getter 메소드는 _objects 딕셔너리에서 주어진 이름의 컴포넌트를 반환합니다.
        // 이 메소드는 _objects 딕셔너리에 저장된 컴포넌트 중에서 이름이 componentName과 일치하는 컴포넌트를 반환합니다.
        // 만약 해당하는 컴포넌트가 없으면 null을 반환합니다.
        public T Getter<T>(string componentName) where T : Object
        {
            if (!_objects.TryGetValue(typeof(T), out Dictionary<string, Object> components)) return null;
            if (components.TryGetValue(componentName, out var component)) return component as T;
            return null;
        }
    }
}


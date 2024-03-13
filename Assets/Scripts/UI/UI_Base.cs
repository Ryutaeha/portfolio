using System;
using System.Collections.Generic;
using UnityEngine;  
using Object = UnityEngine.Object;

namespace UI
{
    // UIBase 클래스는 Binder 클래스를 이용해서 자신의 자식 컴포넌트들을 관리하는 클래스입니다.
    public class UIBase : MonoBehaviour
    {
        private Binder _binder;

        private bool _initialized;
        protected virtual void OnEnable()
        {
            Init();
        }
        // Start 메소드에서 ServiceLocator를 통해 Binder 인스턴스를 가져옵니다.
        protected virtual bool Init()
        {
            if (_initialized) return false;
            _binder = Main.Game.ServiceLocator.GetService<Binder>();
            _initialized = true;
            return true;
        }

        // SetUI 메소드에서는 자신의 자식 컴포넌트 중 타입이 T인 것들을 Binder에 등록합니다.
        protected void SetUI<T>() where T : Object
        {
            _binder.Binding<T>(gameObject);
        }

        // GetUI 메소드에서는 Binder를 통해 주어진 이름의 컴포넌트를 가져옵니다.
        protected T GetUI<T>(string componentName) where T : Object
        {
            return _binder.Getter<T>(componentName);
        }

    }
}
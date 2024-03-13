using System.Collections.Generic;
using System;

public class ServiceLocator
{

    // 서비스를 저장하기 위한 딕셔너리
    private Dictionary<Type, object> services = new Dictionary<Type, object>();

    // 서비스를 추가하는 메소드
    public void RegisterService<T>(T service)
    {
        // 이미 등록된 서비스인지 확인
        if (services.ContainsKey(typeof(T)))
        {
            return;
        }

        // 서비스를 딕셔너리에 추가
        services[typeof(T)] = service;
    }

    // 서비스를 가져오는 메소드
    public T GetService<T>()
    {
        // 요청된 서비스가 딕셔너리에 있는지 확인
        if (!services.TryGetValue(typeof(T), out object service))
        {
            //없으면 추가
            RegisterService(typeof(T));
        }

        // 서비스를 해당 타입으로 변환하여 반환
        return (T)service;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWater : MonoBehaviour
{
    #region fields
    [Header("데미지")]
    [SerializeField] private int _damage;
    [SerializeField] private float _damageRate;
    private List<IDamageable> _damageableList = new List<IDamageable>();


    [Header("색상")]
    private Color _originColor;
    private float _originFogDensity;
    [SerializeField] private Color _waterColor;
    [SerializeField] private float _waterFogDensity;
    #endregion

    #region monobehaviours
    void Start()
    {
        _originColor = RenderSettings.fogColor;
        _originFogDensity = RenderSettings.fogDensity;

        //InvokeRepeating은 TimeScale의 영향을 받음 => 코루틴으로 대체
        //InvokeRepeating("DealDamage", 0, damageRate);
        StartCoroutine(CoDeal());

        //플레이어 사망 event에 등록
        Main.Game.Player.GetComponent<PlayerEventController>().OnDeath += OnPlayerDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fogColor = _waterColor;
            RenderSettings.fogDensity = _waterFogDensity;
        }
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            //데미지를 받을 객체List에 등록
            _damageableList.Add(damageable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fogColor = _originColor;
            RenderSettings.fogDensity = _originFogDensity;
        }
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            _damageableList.Remove(damageable);
        }
    }
    #endregion

    /// <summary>
    /// 플레이어 사망 시 RenderSettings fog의 Color와 Density를 원래 값으로 돌려줍니다.
    /// </summary>
    private void OnPlayerDeath()
    {
        _damageableList.Clear();
        RenderSettings.fogColor = _originColor;
        RenderSettings.fogDensity = _originFogDensity;
    }
    private void DealDamage()
    {
        for (int i = 0; i < _damageableList.Count; i++)
        {
            _damageableList[i].TakeDamage(_damage);
        }
    }
    IEnumerator CoDeal()
    {
        while (true)
        {
            DealDamage();
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}

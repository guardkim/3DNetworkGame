using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class Player : MonoBehaviour
{
    public PlayerStat Stat;
    private readonly Dictionary<Type, PlayerAbility> _abilitiesCache = new Dictionary<Type, PlayerAbility>();

    public T GetAbility<T>() where T : PlayerAbility
    {
        Type type = typeof(T);

        if (_abilitiesCache.TryGetValue(typeof(T), out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라 필요할 때만 하는.. 뒤로 미루는 기법
        if (TryGetComponent(out T newAbility))
        {
            _abilitiesCache[newAbility.GetType()] = newAbility;
            return newAbility;
        }


        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다");
    }
}

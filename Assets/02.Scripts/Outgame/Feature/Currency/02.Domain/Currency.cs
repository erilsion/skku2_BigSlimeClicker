using UnityEngine;
using System;

// 재화를 의미하는 도메인 모델이다.
// 우리 게임에서의 재화 규칙을 만든다.
// 1. 음수가 되면 안 된다. (생성될 때, 연산할 때)
// 2. 정해진 표기법이 있다.
// 3. 재화 간에 +-가 되어야 한다.
// 4. 대소 비교가 되어야 한다.

// 도메인을 만들어야 하는 경우
// 1. 재화가 여러 곳에 사용 된다. (UI, 상점, 업그레이드 등 다양한 콘텐츠에서 쓰인다.)
// 2. 무조건 포멧팅이 통일되어야 한다.
// 3. 재화끼리의 연산이 빈번하다.
// 4. 팀 프로젝트에 있어서 실수를 방지하고 싶다.

// 안 만들어도 되는 경우
// 1. 게임을 빠르게 만들고 싶다.
// 2. 재화가 한 종류뿐이고 사용처도 많지 않다.
// 3. 팀원 없이 혼자 개발해서 도메인에 대한 지식이 곧 내 머리이다.

// struct vs class
// struct은 int, double처럼 값으로 동작하기에 딱 좋다.
// 재화는 '값'이 중요하다.

public readonly struct Currency
{
    public readonly double Value;

    public Currency(double value)
    {
        // 유효성 검사를 한다.
        if(value < 0)
        {
            // 이런 잘못된 데이터가 들어왔다는 것은 여러가지 부작용이 생길 수 있다.
            // 게임 플레이 도중에 그 부작용을 느끼는 것보다 애초에 시작 단계에서 에러를 뱉어버리는 게 유지보수 면에서 편하다.
            throw new Exception("재화가 음수입니다. 이러면 안 돼요!");
        }
        Value = value;
    }

    // 연산자 오버라이딩: 객체간의 연산자(+, -, >, <)할 때 암시적으로 호출되는 메서드이다.
    // 1. 재화끼리 더한다.
    public static Currency operator +(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value + currency2.Value);
    }
    // 2. 재화끼리 뺀다.
    public static Currency operator -(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value - currency2.Value);
    }
    // 3. 비교 연산한다.
    public static bool operator >=(Currency a, Currency b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(Currency a, Currency b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator >(Currency a, Currency b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(Currency a, Currency b)
    {
        return a.Value < b.Value;
    }

    // 4. double → Currency 또는 Currency -> double로 암시적 변환한다(저장/불러오기에서 사용하기 위해 넣었다.).    
    public static implicit operator Currency(double value)
    {
        return new Currency(value);
    }
    public static explicit operator double(Currency currency)
    {
        return currency.Value;
    }

    // ToString은 객체를 문자열로 변환할 때 암시적으로 호출되는 메서드이다.
    // 이걸 개조(메서드 오버라이드)해서 특정 포멧으로 변환되게 한다.
    public override string ToString()
    {
        return Value.FormattedString();
    }
}

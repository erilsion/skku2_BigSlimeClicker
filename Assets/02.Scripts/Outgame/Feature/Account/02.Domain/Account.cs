using System;
using System.Text.RegularExpressions;
using UnityEngine;

// 객체: 어떤 대상 / 개념에 대한 속성(데이터) + 기능(메서드)이다.
// 도메인: 어떤 개념에 집중해서 객체로 표현한 것이다.
public class Account
{
    // 이메일
    // 비밀번호
    public readonly string Email;
    public readonly string Password;

    public Account(string email, string password)
    {
        var emailSpec = new AccountEmailSpecification();
        var passwordSpec = new AccountPasswordSpecification();
        if(!emailSpec.IsSatisfiedBy(email))
        {
            throw new ArgumentException(emailSpec.ErrorMessage);
        }
        if (!passwordSpec.IsSatisfiedBy(password))
        {
            throw new ArgumentException(passwordSpec.ErrorMessage);
        }

        Email = email;
        Password = password;
    }

    // 이메일 규칙: 올바른 이메일이여야 한다.
    // - 비어있으면 안 된다.
    // - 한글이면 안 되고, @사이트주소.com 형태여야 한다.
    // - 동일한 이메일이면 중복이 불가능하다.

    // 비밀번호 규칙
    // - 비어있으면 안 된다.
    // - 여기선 6자리 이상 16자리 이하로 한다. (+ 대문자 1개 이상 포함, 특수문자 1개 이상 포함...)
}

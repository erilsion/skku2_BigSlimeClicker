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

    private int _minPasswordLength = 6;
    private int _maxPasswordLength = 15;

    // ── 정규표현식 (컴파일하여 성능 최적화) ──
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public Account(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException($"이메일은 비어있을 수 없습니다.");
        }
        if (!EmailRegex.IsMatch(email))
        {
            throw new ArgumentException($"올바르지 않은 이메일 형식입니다.");
        }
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException($"비밀번호는 비어있을 수 없습니다.");
        }
        if (password.Length < _minPasswordLength || _maxPasswordLength < password.Length)
        {
            throw new ArgumentException($"비밀번호는 6~16자 사이여야 합니다.");
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

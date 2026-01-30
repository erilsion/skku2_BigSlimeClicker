using System.Text.RegularExpressions;
using UnityEngine;

// Account의 Email에 관한 명세(규칙)이다.
public class AccountEmailSpecification
{
    // ── 정규표현식 (컴파일하여 성능 최적화) ──
    private const string EmailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    private bool _isValidEmail(string email) => Regex.IsMatch(email, EmailPattern);

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;
    public bool IsSatisfiedBy(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _errorMessage = "이메일이 비어있어요!";
            return false;
        }
        if (!_isValidEmail(email))
        {
            _errorMessage = "이메일 형식이 올바르지 않아요!";
            return false;
        }
        return true;
    }
}

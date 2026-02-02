using System.Text.RegularExpressions;
using UnityEngine;

public class AccountPasswordSpecification
{
    // ── 정규표현식 (컴파일하여 성능 최적화) ──
    private const string PasswordPattern = @"^(?=.{6,16}$)(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d]+$";

    private bool _isValidPassword(string password) => Regex.IsMatch(password, PasswordPattern);

    private string _errorMessage;
    public string ErrorMessage => _errorMessage;
    public bool IsSatisfiedBy(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _errorMessage = "패스워드가 비어있어요!";
            return false;
        }
        if (!_isValidPassword(password))
        {
            _errorMessage = "패스워드는 대문자와 숫자를 1개 이상 포함한 6자 이상 16자 이하의 영어로 설정해 주세요!";
            return false;
        }
        return true;
    }
}

using System.Text.RegularExpressions;
using UnityEngine;

public class AccountPasswordSpecification : MonoBehaviour
{
    // ── 정규표현식 (컴파일하여 성능 최적화) ──
    private const string PasswordPattern = @"^(?=.{6,16}$)(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])[A-Za-z\d!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$";

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
            _errorMessage = "패스워드 형식이 올바르지 않아요!";
            return false;
        }
        return true;
    }
}

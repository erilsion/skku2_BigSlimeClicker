using UnityEngine;

// 인증 결과 로그인 / 회원가입에 대한 성공 여부와 에러메시지와 Account 관련 구조체이다.
public struct AuthResult
{
    public bool Success;
    public string ErrorMessage;
    public Account Account;
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class LoginScene : MonoBehaviour
{
    private enum SceneMode
    {
        Login,
        Register
    }

    private SceneMode _mode = SceneMode.Login;

    // 비밀번호 확인 오브젝트
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _registerButton;

    [SerializeField] private TextMeshProUGUI _messageText;

    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;

    private const string EmailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    private const string PasswordPattern = @"^(?=.{7,20}$)(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])[A-Za-z\d!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$";

    private bool _isValidEmail(string email) => Regex.IsMatch(email, EmailPattern);

    private bool _isValidPassword(string pw) => Regex.IsMatch(pw, PasswordPattern);

    private void Start()
    {
        AddButtonEvents();
        LoadLastLoginID();
        Refresh();
        ValidateInputs(); // 초기 로드 시 입력 필드 검증
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(Login);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(Register);
    }

    private void Refresh()
    {
        // 2차 비밀번호 오브젝트는 회원가입 모드일때만 노출된다.
        _passwordConfirmObject.SetActive(_mode == SceneMode.Register);
        _gotoRegisterButton.gameObject.SetActive(_mode == SceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == SceneMode.Login);
        _gotoLoginButton.gameObject.SetActive(_mode == SceneMode.Register);
        _registerButton.gameObject.SetActive(_mode == SceneMode.Register);
    }

    public void OnEmailTextChange(string email)
    {
        ValidateInputs();
    }

    public void OnPasswordTextChange(string password)
    {
        ValidateInputs();
    }

    private void ValidateInputs()
    {
        string email = _emailInputField.text.Trim();
        string password = _passwordInputField.text;

        // 로그인 모드일 때만 검증한다.
        if (_mode != SceneMode.Login)
        {
            return;
        }

        if (string.IsNullOrEmpty(email))
        {
            _messageText.text = "이메일이 비어있어요!";
            _loginButton.interactable = false;
            return;
        }

        if (!_isValidEmail(email))
        {
            _messageText.text = "이메일 형식이 올바르지 않아요!";
            _loginButton.interactable = false;
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            _messageText.text = "비밀번호를 입력해주세요!";
            _loginButton.interactable = false;
            return;
        }

        _messageText.text = "로그인 가능해요!";
        _loginButton.interactable = true;
    }

    private void Login()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;

        string encrypted = PlayerPrefs.GetString(email);
        string decryptedPassword = AESCrypto.Decrypt(encrypted);

        var result = AccountManager.Instance.TryLogin(email, password);
        if (result.Success)
        {
            PlayerPrefs.SetString("LastLoginID", email);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            _messageText.text = $"아이디와 패스워드를 확인해주세요!";
        }
    }

    private void Register()
    {
        string email = _emailInputField.text.Trim();
        string password = _passwordInputField.text;
        string password2 = _passwordConfirmInputField.text;

        if (string.IsNullOrEmpty(password2) || password != password2)
        {
            _messageText.text = "패스워드를 다시 확인해주세요!";
            return;
        }

        var result = AccountManager.Instance.TryRegister(email, password);
        if (result.Success)
        {
            _messageText.text = "회원가입에 성공했어요!";
            GotoLogin();
        }
        else
        {
            _messageText.text = result.ErrorMessage;
        }
    }

    private void GotoLogin()
    {
        _mode = SceneMode.Login;
        Refresh();
        // 로그인 모드로 전환 시 검증한다.
        ValidateInputs();
    }

    private void GotoRegister()
    {
        _mode = SceneMode.Register;
        Refresh();
    }

    private void LoadLastLoginID()
    {
        if (PlayerPrefs.HasKey("LastLoginID"))
        {
            _emailInputField.text = PlayerPrefs.GetString("LastLoginID");
            _passwordInputField.text = string.Empty;
        }
        else
        {
            _emailInputField.text = string.Empty;
        }
    }
}

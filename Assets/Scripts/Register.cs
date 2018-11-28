using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.Networking;

public struct UserData
{
    public string username, password, nickname;
}
public class Register : MonoBehaviour {

    #region LoginFiled
    public InputField Lid;
    public InputField Lpw;
    public GameObject Loginpanel;
    public GameObject Regpanel;
    #endregion

    #region RegisterFiled
    public InputField Sid;
    public InputField Spw;
    public InputField Spwc;
    public InputField Sname;
    #endregion

    private void Start()
    {
        Regpanel.SetActive(false);
    }

    //로그인 버튼 클릭
    public void ClickLoginBtn()
    {
        string id = Lid.text;
        string pw = Lpw.text;
        if(string.IsNullOrEmpty(id)|| string.IsNullOrEmpty(pw))
        {
            return;
        }
        UserData ud = new UserData();
        ud.username = id;
        ud.password = pw;
        StartCoroutine(SignIn(ud));
        
    }

    //로그인 창에서 회원가입 버튼 클릭
    public void ClickOnRegPanel()
    {
        Regpanel.SetActive(true);
        Loginpanel.SetActive(false);
    }

    //회원가입창에서 회원가입완료 버튼 클릭시.
    public void ClickRegOk()
    {
        string id = Sid.text;
        string pw = Spw.text;
        string pwc = Spwc.text;
        string name = Sname.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(pwc) || string.IsNullOrEmpty(name)) 
        {
            return;
        }
        if(pw.Equals(pwc))
        {
            //서버에 전송
            UserData ud = new UserData();
            ud.username = id;
            ud.password = pw;
            ud.nickname = name;
            StartCoroutine(SignUp(ud));
            Regpanel.SetActive(false);
            Loginpanel.SetActive(true);

            Debug.Log("회원가입완료.. 회원의 ID는 " + ud.username + "이며 pw는 "+ ud.password + " 회원의 이름은 "+ ud.nickname + " 입니다.");
        }
    }
    //x 버튼
    public void CloseBtn()
    {
        Regpanel.SetActive(false);
        Loginpanel.SetActive(true);
    }

    IEnumerator SignUp(UserData userdata)
    {
        string postData = JsonUtility.ToJson(userdata);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/add", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    IEnumerator SignIn(UserData userdata)
    {
        string postData = JsonUtility.ToJson(userdata);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/find", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                SceneManager.LoadScene("InGame");
            }
        }
    }


}

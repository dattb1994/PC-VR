using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiFromCms : PersistentSingleton<ApiFromCms>
{


    public LoginState state = LoginState.notYet;
    public string IP = "http://192.168.1.5:8082";//http://192.168.10.101:5000

    public LoginResultRoot adminLogin;
    public LoginResultRoot userLogin;

    public EnvironmentGetAll environmentGetAll;
    public PracticeGetAll practiceGetAll;
    public RootLogObjectData logObjectData;
    public GetInfoByUser info;
    public IEnumerator LoginAdmin(UnityAction onSuccess)
    {
        print("STart login admin");
        IP = GameConfig.IpConfig.IP_CMS;

        var admin = new UserData();
        admin.loginName = "Administrator";
        admin.password = "Admin@123";

        string json = JsonUtility.ToJson(admin);
        string url = $"{IP}/api/user/login";
        //var req = new UnityWebRequest(url, "POST");
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(req.error);
            print(" login admin failded");

        }
        else
        {
            print(" login admin success");
            adminLogin = JsonUtility.FromJson<LoginResultRoot>(req.downloadHandler.text);
        }

        onSuccess?.Invoke();
    }
    public IEnumerator LoginUser(string username, string password)
    {


        IP = GameConfig.IpConfig.IP_CMS;

        //username = "D39A01";//"Administrator";///
        //username = "Administrator";///
        //password = "Admin@123";

        //@TODO: call API login
        // Store Token
        // Add Token to headers

        var user = new UserData();
        user.loginName = username;
        user.password = password;

        string json = JsonUtility.ToJson(user);
        print(json);
        string url = $"{IP}/api/user/login";
        //var req = new UnityWebRequest(url, "POST");
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        File.WriteAllBytes("D:/login.json", jsonToSend);

        //Send the request then wait here until it returns

        state = LoginState.tryingLogin;

        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(req.error);
            state = LoginState.loginFailed;
        }
        else
        {
            state = LoginState.loggedIn;
            Debug.Log("Received: " + req.downloadHandler.text);
            userLogin = JsonUtility.FromJson<LoginResultRoot>(req.downloadHandler.text);
            StartCoroutine(GetEnv());
            StartCoroutine(GetPractice());
            StartCoroutine(GetInfoByUser());

            //StartCoroutine(IEEnvironmentGetAll());
            //StartCoroutine(IEPracticeGetAll());

        }
    }
    public IEnumerator GetEnv()
    {

        string url = $"{IP}/api/environment/getall";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        string jsonBody = JsonUtility.ToJson(userLogin.objectData);
        string replaceObjectData = jsonBody.Replace("{}", "null");
        print(replaceObjectData);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(replaceObjectData);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi yêu cầu và chờ phản hồi
        yield return request.SendWebRequest();

        // Kiểm tra nếu yêu cầu thành công
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Yêu cầu thành công! Phản hồi từ server: " + request.downloadHandler.text);
            environmentGetAll = JsonUtility.FromJson<EnvironmentGetAll>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Lỗi khi gửi yêu cầu: " + request.error);
        }
    }
    public IEnumerator GetPractice()
    {
        string url = $"{IP}/api/practice/getall";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        string jsonBody = JsonUtility.ToJson(userLogin.objectData);
        string replaceObjectData = jsonBody.Replace("{}", "null");
        print(replaceObjectData);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(replaceObjectData);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi yêu cầu và chờ phản hồi
        yield return request.SendWebRequest();

        // Kiểm tra nếu yêu cầu thành công
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Yêu cầu thành công! Phản hồi từ server: " + request.downloadHandler.text);
            practiceGetAll = JsonUtility.FromJson<PracticeGetAll>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Lỗi khi gửi yêu cầu: " + request.error);
        }
    }
    public IEnumerator IELogPracticeStudent(LogPracticeStudentBody jsonBody)
    {
        //@TODO: call API login
        // Store Token
        // Add Token to headers
        string json = JsonUtility.ToJson(jsonBody);
        string replaceObjectData = json.Replace("{}", "null");
        print("chua sủa:" + json);
        print("da sủa:" + replaceObjectData);


        var req = new UnityWebRequest($"{IP}/api/logPracticeStudent/pointinput", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(replaceObjectData);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();
        logObjectData = JsonUtility.FromJson<RootLogObjectData>(req.downloadHandler.text);
        print(req.result);
    }
    public IEnumerator GetInfoByUser()
    {
        string url = $"{IP}/api/logPracticeStudent/GetInfoByUser";
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        GetInfoByUserBody body = new GetInfoByUserBody();
        body.language = adminLogin.objectData.currentLanguage;
        body.clientId = adminLogin.objectData.clientId;
        body.currentUserId = adminLogin.objectData.currentUserId;
        body.isAdmin = true;
        body.tokenKey = adminLogin.objectData.tokenKey;
        body.currentLanguage = adminLogin.objectData.currentLanguage;
        body.formObject = adminLogin.objectData.formObject;

        string jsonBody = JsonUtility.ToJson(body);
        string replaceObjectData = jsonBody.Replace("{}", "null");
        print(replaceObjectData);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(replaceObjectData);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);



        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi yêu cầu và chờ phản hồi
        yield return request.SendWebRequest();

        // Kiểm tra nếu yêu cầu thành công
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Yêu cầu thành công! Phản hồi từ server: " + request.downloadHandler.text);
            info = JsonUtility.FromJson<GetInfoByUser>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Lỗi khi gửi yêu cầu: " + request.error);
        }
    }
}
public enum LoginState
{
    notYet,
    tryingLogin,
    loggedIn,
    loginFailed,
}

#region Login API
[System.Serializable]
public class LoginResultRoot
{
    public LoginResult objectData;
    public string message;
}
[System.Serializable]
public class LoginResult
{
    public string clientId;
    public int currentUserId;
    public bool isAdmin;
    public string tokenKey;
    public string currentLanguage;
    public FormObject formObject;
}
public class UserData
{
    public string loginName;
    public string password;
}
#endregion

#region Practice
[System.Serializable]
public class PracticeData
{
    public int practiceId;
    public string code;
    public string name;
    public string note;
    public int primaryKey;
    public bool isOwner;
    public bool canEdit;
    public bool canView;
    public bool canDelete;
    public bool active;
    public int status;
    public string fullTextSearchData;
    public int createdBy;
    public DateTime createdOn;
    public int changedBy;
    public DateTime changedOn;
    public int deletedBy;
    public object deletedOn;
    public int rowState;
}
[System.Serializable]
public class PracticeGetAll
{
    public List<PracticeData> objectData;
    public string message;
    public List<object> messages;
}
#endregion

#region Environment
[System.Serializable]
public class EnvironmentData
{
    public int environmentId;
    public string code;
    public string name;
    public string address;
    public string note;
    public int primaryKey;
    public bool isOwner;
    public bool canEdit;
    public bool canView;
    public bool canDelete;
    public bool active;
    public int status;
    public string fullTextSearchData;
    public int createdBy;
    public DateTime createdOn;
    public int changedBy;
    public object changedOn;
    public int deletedBy;
    public object deletedOn;
    public int rowState;
}

[System.Serializable]
public class EnvironmentGetAll
{
    public List<EnvironmentData> objectData;
    public string message;
    public List<object> messages;
}
#endregion

#region LogPracticeStudent
[System.Serializable]
public class LogPracticeStudentBody
{
    public string userId;
    public string practiceCode;
    public string environmentCode;
    public string logContent;
    public string startOn;
    public string finishOn;
    public int point;
    public string mapCode;
    public int difficultLevel;
    public string note;
    public string clientId;
    public int currentUserId;
    public bool isAdmin;
    public string tokenKey;
    public string currentLanguage;
    public FormObject formObject;
    public LogPracticeStudentBody(string userId, string practiceCode, string environmentCode, string logContent, string startOn, string finishOn, int point, string mapCode,
        int difficultLevel, string note, string clientId, int currentUserId, bool isAdmin, string tokenKey,
        string currentLanguage, FormObject formObject)
    {
        this.userId = userId;
        this.practiceCode = practiceCode;
        this.environmentCode = environmentCode;
        this.logContent = logContent;
        this.startOn = startOn;
        this.finishOn = finishOn;
        this.point = point;
        this.mapCode = mapCode;
        this.difficultLevel = difficultLevel;
        this.currentUserId = currentUserId;
        this.note = note;
        this.clientId = clientId;
        this.currentUserId = currentUserId;
        this.isAdmin = isAdmin;
        this.tokenKey = tokenKey;
        this.currentLanguage = currentLanguage;
        this.formObject = null;
    }
}
[System.Serializable]
public class FormObject
{ }

[System.Serializable]
public class LogObjectData
{
    public int logPracticeStudentId;
    public int userId;
    public string fullName;
    public int practiceId;
    public string practiceCode;
    public string practiceName;
    public int environmentId;
    public string environmentCode;
    public string environmentName;
    public string logContent;
    public DateTime startOn;
    public DateTime finishOn;
    public double point;
    public string mapCode;
    public int difficultLevel;
    public string sectionId;
    public int primaryKey;
    public bool isOwner;
    public bool canEdit;
    public bool canView;
    public bool canDelete;
    public string note;
    public bool active;
    public int status;
    public string fullTextSearchData;
    public int createdBy;
    public DateTime createdOn;
    public int changedBy;
    public object changedOn;
    public int deletedBy;
    public object deletedOn;
    public int rowState;
}
[System.Serializable]
public class RootLogObjectData
{
    public LogObjectData objectData;
    public string message;
    public List<string> messages;
}

#endregion
#region GetInfoByUser
[System.Serializable]
public class GetInfoByUserBody
{
    public string loginName;
    public string language;
    public string clientId;
    public int currentUserId;
    public bool isAdmin;
    public string tokenKey;
    public string currentLanguage;
    public object formObject;
}
[System.Serializable]
public class AvgScoreByPractice
{
    public string practiceName;
    public double point;
}

[System.Serializable]
public class LogPracticeStudentModel
{
    public string employeeCode;
    public string practiceStatus;
    public int logPracticeStudentId;
    public int userId;
    public string fullName;
    public int practiceId;
    public string practiceCode;
    public string practiceName;
    public int environmentId;
    public string environmentCode;
    public string environmentName;
    public string logContent;
    public DateTime startOn;
    public DateTime finishOn;
    public double point;
    public string mapCode;
    public int difficultLevel;
    public string sectionId;
    public int primaryKey;
    public bool isOwner;
    public bool canEdit;
    public bool canView;
    public bool canDelete;
    public string note;
    public bool active;
    public int status;
    public string fullTextSearchData;
    public int createdBy;
    public DateTime createdOn;
    public int changedBy;
    public object changedOn;
    public int deletedBy;
    public object deletedOn;
    public int rowState;
}

[System.Serializable]
public class ObjectData
{
    public string code;
    public string fullName;
    public DateTime latestDate;
    public string totalTime;
    public int totalPracticed;
    public int totalCompleted;
    public int totalUnCompleted;
    public List<LogPracticeStudentModel> logPracticeStudentModels;
    public List<TrainingTimeByWeek> trainingTimeByWeek;
    public List<WeekDescription> weekDescription;
    public List<AvgScoreByPractice> avgScoreByPractices;
}

[System.Serializable]
public class GetInfoByUser
{
    public ObjectData objectData;
    public string message;
    public List<object> messages;
}

[System.Serializable]
public class TrainingTimeByWeek
{
    public int entryIndex;
    public int weekOfYear;
    public bool fullWeek;
    public string week;
    public DateTime startDate;
    public DateTime endDate;
    public DateTime rangeStart;
    public DateTime rangeEnd;
    public double totalTime;
}

[System.Serializable]
public class WeekDescription
{
    public string value;
    public string name;
}
#endregion

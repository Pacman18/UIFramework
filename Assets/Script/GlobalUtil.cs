using UnityEngine;
using System.Collections;
using System.Text;
using System;

using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public static class GlobalUtil
{
    public static string GetTextString
    {
        get { return Text.ToString(); }
    }

    public static StringBuilder Text = new StringBuilder(256);
    public static StringBuilder DialogText = new StringBuilder(256);

    public static string ColorEnd = "</color>";

    private static int _seed = 354792;

    public static string[] Scolor =
    {
        "<color=#ff0000ff>", // 레드 0
        "<color=#0000ffff>", // 블루 1
        "<color=#008000ff>", // 그린 2
        "<color=#ffa500ff>", // 오렌지 3
        "<color=#ffff00ff>", // 옐로우 4
        "<color=#800080ff>", // 퍼플 5
        "<color=#a52a2aff>", // 브라운 6
        "<color=#00ffffff>", // 아쿠아 7
        "<color=#ff00ffff>", // 핑크 8   
        "<color=#808000ff>", // 올리브 9
        "<color=#add8e6ff>", // 라이트블루 10
        "<color=#000000ff>", // 블랙 11        
    };

    

    public static bool IsNotNull(this System.Object obj)
    {
        return System.Object.ReferenceEquals(obj, null) == false;
    }

    public static bool IsNull(this System.Object obj)
    {
        return System.Object.ReferenceEquals(obj, null);
    }

    public static float V3SqrDistance(this Vector3 vec, Vector3 other)
    {
        float temp1 = vec.x - other.x;
        temp1 *= temp1;
        float temp2 = vec.y * other.y;
        temp2 *= temp2;
        return temp1 + temp2;
    }

    static public string CreateGUID
    {

        get 
        {
            Guid myGuid = Guid.NewGuid();  //GUID 인스턴스 초기화
            string guid = myGuid.ToString("N"); //지정자 N은 하이픈이 없는 연결된 GUID값
            return guid;
        }
    }

    public static int CharacterMaxLevel = 30;
    public static int ItemMaxLevel = 20;
    public static int ItemCombinePrice = 20;
    public static float DashCoolTime = 7.0f;
    public static float CardCoolTime = 4.0f;


    public const string Narration = "Narration";
    public const string Suddendeath = "Sudden Death";
    public const string WIN = "YOU WIN!";
    public const string LOSE = "YOU LOSE";
    public const string True = "true";
    public const string False = "";
    public const string Ready = "READY";
    public const string Waiting = "Waiting...";
    public const string SLASH = "/";

    public const string ImgBack = "Img_Back";
    public const string ImgPrice = "Img_Price";
    public const string ImgBg = "Img_Bg";
    public const string ImgIcon = "Img_IconBack";
    public const string ImgInGameIcon = "Img_Spell_Icon";
    public const string ImgInGameIconObj = "Img_IconBack";
    public const string ImgInfo = "Img_Info";
    public const string ImgChar = "Img_Char";
    public const string ImgSelect = "Img_Select";
    public const string ImgLock = "Img_Lock";
    public const string ImgHas = "Img_Has";
    public const string ImgType = "Img_Type";
    public const string ImgForeground = "Img_Foreground";

    public const string ImgLeft = "Img_Left";
    public const string ImgRight = "Img_Right";
    public const string ImgUp = "Img_Up";
    public const string ImgDown = "Img_Down";

    public const string MyCard = "myHandCard";
    public const string Deck = "Deck";
    public const string ImgCard = "Img_Card";
    public const string ImgCamp = "Img_Camp";


    public const string NPC = "NPC";
    public const string Level = "Lv.";

    public const string TextMoney = "Text_Money";
    public const string TextName = "Text_Name";
    public const string TextInfo = "Text_Info";
    public const string TextDescription = "Text_Desc";
    public const string TextPrice = "Text_Price";
    public const string TextTicket = "Text_Ticket";
    public const string TextTime = "Text_Time";
    public const string TextTitle = "Text_Title";
    public const string TextLevel= "Text_Level";
    public const string TextCamp = "Text_Camp";
    public const string TextExp = "Text_Exp";
    public const string TextWinReport = "Text_WinLose";
    public const string TextPower = "Text_Power";
    public const string TextPain = "Text_Pain";
    public const string TextNoti = "Text_Noti";
    public const string TextRank = "Text_Rank";
    public const string TextResult = "Text_Result";
    public const string TextState = "Text_State";
    public const string TextCash = "Text_Cash";
    public const string TextFlower = "Text_Flower";
    public const string TextCount = "Text_Count";
    public const string Text1000 = "{0:#,##0}";
    public const string TextNeed = "Text_Need";
    public const string TextReward = "Text_Reward";
    public const string TextScore = "Text_Score";
    public const string TextSpecial = "Text_Special";
    public const string TextPrev = "Text_Prev";
    public const string TextNext = "Text_Next";
    public const string TextVs = "Text_Versus";
    public const string TextOwner = "Text_Owner";
    public const string TextMana = "Text_Count";
    



    public const string BtnOut = "Btn_Out";
    public const string BtnSubmit = "Btn_Submit";
    public const string BtnSell = "Btn_Sell";
    public const string BtnBuy = "Btn_Buy";
    public const string DefaultText = "Text";
    public const string Character3DPath = "Prefab/3DObject/";
    public const string Character3DSportPath = "Prefab/3DObject/SportModel";
    public const string BtnPrev = "Btn_Prev";
    public const string BtnNext = "Btn_Next";
    public const string BtnBall = "Btn_Ball";
    public const string BtnFind = "Btn_Find";
    public const string Send = "Btn_Send";
    public const string Versus = "Btn_Vs";
    public const string BtnExit = "Btn_Exit"; 
    public const string Chat = "Btn_Chat";
    public const string Change = "Btn_Change";
    public const string Request = "Btn_Request";
    public const string BtnCancel = "Btn_Cancel";
    public const string BtnClose = "Btn_Close";
    public const string BtnConfirm = "Btn_Confirm";
    public const string BtnStart = "Btn_Start";
    public const string BtnSelect = "Btn_Select";
    public const string BtnRank = "Btn_Rank";
    public const string BtnChar = "Btn_Character";
    public const string BtnCash = "Btn_Cash";
    public const string BtnComp = "Btn_Comp";
    public const string BtnDodge = "Btn_Dodge";
    public const string BtnCGP = "Btn_CGP";
    public const string BtnStory = "Btn_Story";
    public const string BtnPvP = "Btn_PvP";

    public const string BtnDeck = "Btn_Deck";
    public const string BtnDetail = "Btn_Detail";
    public const string BtnShop = "Btn_Shop";
    public const string BtnOption = "Btn_Option";
    public const string BtnCoin = "Btn_Coin";
    public const string BtnTicket = "Btn_Ticket";
    public const string BtnFlower = "Btn_Flower";
    public const string BtnUp = "Btn_Up";
    public const string BtnDown = "Btn_Down";
    public const string BtnLeft = "Btn_Left";
    public const string BtnLevelUp = "Btn_LevelUp";
    public const string BtnRight = "Btn_Right";
    public const string BtnRequest = "Btn_Request";
    public const string BtnReview = "Btn_Review";
    public const string BtnSearch = "Btn_Search";
    public const string BtnSkip = "Btn_Skip";
    public const string BtnLobby = "Btn_Lobby";
    public const string BtnMail = "Btn_Mail";
    public const string BtnMini = "Btn_Mini";
    public const string BtnModify = "Btn_Modify";
    public const string BtnMix = "Btn_Mix";
    public const string BtnInfo = "Btn_Info";
    public const string BtnDash = "Btn_Dash";
    public const string BtnUse = "Btn_Use";
    public const string Btn_No0 = "Btn_0";
    public const string Btn_No1 = "Btn_1";
    public const string Btn_No2 = "Btn_2";
    public const string Btn_No3 = "Btn_3";



    public const string DefaultBallTransPosition = "Bip001 R Finger0Nub/Ball";
    public const string BallTransPosition = "Character1_RightHandMiddle2/Ball";

    // PlayerPrefab
    public const string SelectedDeck = "SelectDodgeDeck";

    // Character Stats 
    public const string WINPOSE = "Win";
    public const string POSE = "Pose";
    public const string IDLE = "Idle";
    public const string WALK = "Run";
    public const string DASH = "Dash";
    public const string SHOOT = "Shoot";
    public const string HITED = "Hited";
    public const string STUN = "Stun";
    public const string DIE = "Die";
    public const string CATCH = "Catch";
    public const string UNLOAD = "UnLoad";
    public const string BLOCK = "Block";
    public const string STANDUP = "StandUp";
    public const string DOWN = "Down";
    public const string COMBI_SHOOT = "Combi_S";


    // Layer     
    public const int MyTeam = 13; // 팀플레이어
    public const int MyAttackBall = 14; // 적군 맞추는 볼
    public const int GetBall = 12; // 볼을 습득할 수 있는 상태
    public const int NoContect = 0; // 공이 중력상태로 무엇과도 충돌하지 않는다
    public const int Obstacle = 9;
    public const int PlayerLayer = 10;
    public const int EnemyLayer = 11;
    public const int MapLayer = 8;
    public const int IgnoreLayer = 2;// 절대 충돌하지 않는   

    static public Vector3 VectorZero = new Vector3(0,0,0);


    private const string AssetBundleDirectory = "Assets/AssetBundles";
    private const string ExtensionName = ".unity3d";


    static public Vector3 LEFT_V = new Vector3(-1,0,0);
    static public Vector3 RIGHT_V = new Vector3(1, 0, 0);
    static public Vector3 UP_V = new Vector3(0, 0, 1);
    static public Vector3 DOWN_V = new Vector3(0, 0, -1);
    static public Vector3 LEFT_UP_V = new Vector3(-1, 0, 1);
    static public Vector3 LEFT_DOWN_V = new Vector3(-1, 0, -1);
    static public Vector3 RIGHT_UP_V = new Vector3(1, 0, 1);
    static public Vector3 RIGHT_DOWN_V = new Vector3(1, 0, -1);

    static public Vector3 UP_Qnew = new Vector3(0, 0, 0); // 0
    static public Vector3 RIGHT_UP_Q = new Vector3(0, 45, 0); // 1
    static public Vector3 RIGHT_Q = new Vector3(0, 90, 0); // 2 
    static public Vector3 RIGHT_DOWN_Q = new Vector3(0, 135, 0); // 3 
    static public Vector3 DOWN_Q = new Vector3(0, 180, 0); // 4 
    static public Vector3 LEFT_DOWN_Q = new Vector3(0, 225, 0); // 5 
    static public Vector3 LEFT_Q = new Vector3(0, 275, 0); // 6
    static public Vector3 LEFT_UP_Q = new Vector3(0, 320, 0);  // 7


    /// <summary>
    /// 텍스트 초기화 및 누적데이타 반환
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string AddString(params string[] param)
    {
        Text.Length = 0;

        for (int i = 0; i < param.Length; i++)
        {
            Text.Append(param[i]);
        }

        return Text.ToString();
    }

    private const int AddHours = 9; // 한국시간..  9시간을 더한다.
    // UTC 시간가져오기 
    public static System.DateTime GetUtcNow()
    {
        return System.DateTime.UtcNow.AddHours(AddHours);
    }
    public static T EnumUtilParse<T>( string name)
    {
        object result;

        System.Enum.TryParse(typeof(T), name, true, out result);

        return result != null ? (T)result : default(T);
    }


    public static string RichText(string color, string str)
    {
        return AddString("<color=",color,">",str,"</color>");
    }

    /// <summary>
    /// 텍스트 누적용
    /// </summary>
    /// <param name="param"></param>
    public static void AccString(params string[] param)
    {
        for (int i = 0; i < param.Length; i++)
        {
            Text.Append(param[i]);
        }        
    }

    public static void ShuffleList<T>(System.Collections.Generic.List<T> list)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 0; index < list.Count; ++index)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            tmp = list[random1];
            list[random1] = list[random2];
            list[random2] = tmp;
        }
    }

    static public Image GetImage(Transform transform, string childName)
    {
        Image childresult;

        Transform trans = transform.Find(childName);

        if (trans)
        {
            return trans.GetComponent<Image>();
        }
        else
        {
            Image[] images = transform.GetComponentsInChildren<Image>(true);

            if (images.Length > 0)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i].name == childName)
                    {
                        childresult = images[i];
                        return childresult;
                    }
                }

                //Debug.Log("Component is Not found : " + childName);

                return null;
            }
            else
            {
                Debug.Log("Component is Not found");
                return null;
            }
        }
    }

    static public Text GetText(Transform specTrans, string childName)
    {
        Text childresult;

        Transform trans = specTrans.Find(childName);

        if (trans)
        {
            return trans.GetComponent<Text>();
        }
        else
        {
            Text[] texts = specTrans.GetComponentsInChildren<Text>(true);

            if (texts.Length > 0)
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    if (texts[i].name == childName)
                    {
                        childresult = texts[i];
                        return childresult;
                    }
                }

                //Debug.Log("Component is Not found " + childName);

                return null;
            }
            else
            {

                //Debug.Log("Component is Not found" + childName);
                return null;
            }
        }
    }
  
    static public void SetRandomSeed(int seed)
    {
        _seed = seed;
        UnityEngine.Random.InitState(_seed);
    }

    static public Vector3 RandomPosition(int radius)
    {
        int x = UnityEngine.Random.Range(0, radius);
        int y = UnityEngine.Random.Range(0, radius);
        int z = UnityEngine.Random.Range(0, radius);

        return new Vector3(x, y, z);    
    }
}

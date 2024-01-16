using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using uTools;
using System.Text;

namespace UIContent
{
    public class DialogBox : UIPopupBase
    {
        private Text _textInfo; //  대화내용
        private Text _Owner; // 대화 주체
        private Button _nextBtn; // 다음   
        private Button _skipBtn; // 스킵
        private Image _icon; // 화자

        private StringBuilder _infomation;

        private uTweenPosition _tween;
        private UnityAction _nextAction;
        private UnityAction _onClosedAction;


        #region 텍스트 Show 관련 변수 
        private bool _show = false;
        private float _term = 0.1f;
        private float _accTime = 0;
        private int _textIndex = 0;
        private float _showSkipTime = 1f;
        #endregion



        void Awake()
        {
            _textInfo = GetText(GlobalUtil.TextInfo);
            _Owner = GetText(GlobalUtil.TextOwner);
            _icon = GetImage(GlobalUtil.ImgIcon);

            _tween = GetComponent<uTweenPosition>();
            _infomation = GlobalUtil.DialogText;

            _nextBtn = GetButton(GlobalUtil.BtnNext);
            _skipBtn = GetButton(GlobalUtil.BtnSkip);

            _nextBtn.onClick.AddListener(ShowAllText);
            _skipBtn.onClick.AddListener(Close);

            _skipBtn.gameObject.SetActive(false);

        }

        public void SetUIData(int ownerIndex, string converText, UnityAction endCallback = null)
        {   
            _infomation.Length = 0;
            _infomation.Append(converText);
            _textInfo.text = string.Empty;

            if (ownerIndex > 0)
            {
                //_icon.sprite = UIHelper.i.GetCharacterIcon(ownerIndex);
                //_Owner.text = UIHelper.i.GetCharacterData(ownerIndex).Name;
            }

            _onClosedAction = endCallback;       
        }

        // 텍스트를 출력한다
        public void ShowText(float term = 0.03f)
        {
            //DownloadTheAudio("하하하하하하");
            _show = true;
            _term = term;
            _accTime = 0;
            _textIndex = 0;
            _textInfo.text = string.Empty;

            _skipBtn.gameObject.SetActive(false);
            _nextBtn.gameObject.SetActive(false);
        }

        /*private void RequestTTS()
        {
            //RestClient.Request
        }

        void DownloadTheAudio(string text)
        {
            string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q= " + text + "&tl=ko-KR";

            RequestHelper helper = new RequestHelper();
            helper.Uri = url;
            helper.Method = "POST";
            helper.Timeout = 5;
            

            Debug.Log("Request DownloadTheAudio");
            RestClient.Request(helper).Then(res =>
            {
                Debug.Log(res.Error);
            }).Catch( ex =>
            {
                Debug.Log(ex.Message);
            });
            /*WWWForm www = new WWWForm(url);
            yield return www;

            audioSource.clip = www.GetAudioClip(false, true, AudioType.MPEG);
            audioSource.Play();
        }*/

        // 테이블의 대화 인덱스 시작과끝을 연속해서 플레이한다. 
        public void ShowByTableIndex(int start, int end, UnityAction endCallback = null)
        {
            var uidata = GetData() as DialogBoxData;
            uidata.AddConversation(start, end);
            Show(true);

            _onClosedAction = endCallback;
        }

        public void Show(bool isRelay = false)
        {
            string converText = "testtestsetest";

            bool isEnd = string.IsNullOrEmpty(converText);

            if(isEnd)
            {
                Close();
                return;
            }

            _infomation.Length = 0;
            _infomation.Append(converText);
            ShowText();

            if (isRelay)
            {
                _nextAction = () => Show(true);
            }
        }

        public void ShowTweenText(Vector3 from, Vector3 to, float term = 0.03f)
        {
            _tween.from = from;
            _tween.to = to;
            _tween.ResetToBeginning();
            _tween.onFinished = new UnityEvent();
            _tween.onFinished.AddListener(() => ShowText(term));
            _tween.PlayForward();
        }

        void Update()
        {
            if (_show)
            {
                _accTime += Time.deltaTime;

                if (_accTime >= _term)
                {
                    _textInfo.text = _infomation.ToString(0, _textIndex);
                    _textIndex++;
                    _accTime = 0;

                    if (_textIndex > _infomation.Length)
                    {
                        _show = false;
                        _nextBtn.gameObject.SetActive(true);
                    }
                }

                if (_accTime > _showSkipTime && _skipBtn.gameObject.activeSelf == false)
                    _skipBtn.gameObject.SetActive(true);
            }
        }

        private void ShowAllText()
        {
            _textInfo.text = _infomation.ToString(0, _infomation.Length);
            _show = false;

            if(_nextAction != null)
                _nextAction.Invoke();
            else
            {
                Close();
            }

        }

        public override void Close()
        {
            _onClosedAction?.Invoke();
            base.Close();
        }
    }

    public class DialogBoxData : UIData
    {
        private Queue<int> _conversationQueue = new  Queue<int>();

        public int Step; // 현재 스탭 
        public override void OpenInitData()
        {

        }

        // 테이블 인덱스만 저장됨
        public void AddConversation(int start, int end)
        {
            for (int i = start; i <= end; i++)
                _conversationQueue.Enqueue(i);
        }

        // 테이블 인덱스만 저장됨
        public void AddConversation(int start)
        {
            _conversationQueue.Enqueue(start);
        }

        // 테이블 인덱스로만 구동됨
        public string GetNextConversation()
        {
            if(_conversationQueue.Count < 1)
                return string.Empty;


            return string.Empty;

            /*int tableIndex = _conversationQueue.Dequeue();

            var tableData = Dualgate.Tables.Communication.GetByIndex(tableIndex);

            Debug.Log("GetNextConversation : " + tableIndex + ", tableData:" + tableData);

            if (tableData == null)
                return string.Empty;

            return tableData.Conversation;*/
        }

    }
}


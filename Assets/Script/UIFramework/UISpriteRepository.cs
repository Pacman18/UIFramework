using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class UISpriteRepository : SingleTone<UISpriteRepository>
{
    public enum AtlasType { None, Lobby, InGAME}

    public class SpriteContainer
    {
        public string AtlasName;
        private Dictionary<string, Sprite> _container = new Dictionary<string, Sprite>();        

        public int Count
        { get { return _container.Count; } }

        public void AddSprite(string name , Sprite image)
        {
            if(_container.ContainsKey(name) == false)
                _container.Add(name , image);
        }

        public Sprite GetSprite(string spriteName)
        {
            Sprite findSprite;

            if (_container.TryGetValue(spriteName, out findSprite))
                return findSprite;
            else
            {   
                return null;
            }                
        }

        public void Clear()
        {
            _container.Clear();
            _container = null;
            AtlasName = null;
        }
    }
        
    
    private Dictionary<string, SpriteContainer> _spriteDictionary = new Dictionary<string, SpriteContainer>();
    private const string extensionText = ".png";


    private const string PopAtlasPath = "Atlas/Popup/";
    private const string AtlasPath = "Atlas/";

    // 공통 숫자 , 아이콘
    private const string Common = "GCP_Common";

    // 로비 전용
    private const string Lobby= "Lobby";

    // 인게임 전용
    private const string Game= "Game";

    /// <summary>
    ///  가위바위보 관련 카드 리소스 부분
    /// </summary>
    private const string CardReaourcesPath = "UITexture/Card/";

    private const string C_CARD_L = "icon_big_scissor";
    private const string G_CARD_L = "icon_big_rock";
    private const string P_CARD_L = "icon_big_paper";
    private const string C_CARD_S = "icon_top_scissor";
    private const string G_CARD_S = "icon_top_rock";
    private const string P_CARD_S = "icon_top_paper";
    private const string C_CARD_BG = "card_scissor";
    private const string G_CARD_BG = "card_rock";
    private const string P_CARD_BG = "card_paper";
    private const string EmptyOther = "emptyslot_other";
    private const string Empty = "emptyslot_mine";

    public const string Icon_Coin = "coin_ic";
    public const string Icon_Dia = "icon_Dia";
    public const string Icon_Flower = "Item_Flower";
    public const string Icon_Star = "icon_Star01";

    private bool _init = false;    
    

    // 팝업 단위로 로드할때 쓰인다
    public void DynamicLoadAtlasTexture(string textureName, AtlasType atlasType = AtlasType.None)
    {
        // 지정하지 않았을때         
        LoadAtlasTexture( atlasType, textureName);        
    }

    // 폴더단위로 로드할려고 해놓은것 같은데 인게임쪽 로드함
    public void FoldSpriteLoad()
    {
        if (_init)
            return;

        LoadAtlasTexture(AtlasType.InGAME);

        _init = true;
    }    
  
    public void UnLoadAtlasTexture(AtlasType atlasType)
    {
        switch (atlasType)
        {
            case AtlasType.Lobby:
                ReleaseAtlasSprite(Lobby);                
                break;
            case AtlasType.InGAME:
                ReleaseAtlasSprite(Game);
                break;
            default:
                Debug.Log("Try wrong Sprite Path Release ");
                break;
        }        
    }


    public void LoadAtlasTexture(AtlasType atlasType, string fileName = null)
    {

        if (atlasType == AtlasType.None && _spriteDictionary.ContainsKey(fileName)
            || _spriteDictionary.ContainsKey(atlasType.ToString()))
            return;

        GlobalUtil.AddString(AtlasPath, fileName);

        switch (atlasType)
        {   
            case AtlasType.Lobby:
                AddAtlasSprite(Lobby, "Icon_Atlas");
                break;         
            case AtlasType.InGAME:
                AddAtlasSprite(Game, "DualCardAtlas");
                break;            
            default: // 팝업 단위로 아틀라스 생성시
                AddAtlasSprite(PopAtlasPath,fileName);
                break;
        }
    }   

    // 아틀라스 컨테이너에 추가한다 ex : Lobby + Common + Icon  한가지 키로 통합
    private void AddAtlasSprite(string Key, string atlasName)
    {
        SpriteContainer container;

        Sprite[] sprites;

        sprites = Resources.LoadAll<Sprite>(GlobalUtil.AddString(AtlasPath, atlasName));

        if (sprites == null)
            return;

        if (_spriteDictionary.TryGetValue(Key, out container))
        {
            for (int i = 0; i < sprites.Length; ++i)
                container.AddSprite(sprites[i].name, sprites[i]);
        }
        else
        {
            container = new SpriteContainer();
            container.AtlasName = Key;

            for (int i = 0; i < sprites.Length; ++i)
                container.AddSprite(sprites[i].name, sprites[i]);

            _spriteDictionary.Add(Key, container);
        }

        Debug.Log("Load Atlas : " + container.AtlasName + " , AtlasCount : " + _spriteDictionary.Count);
    }





    public void ReleaseAtlasSprite(string name)
    {
        SpriteContainer releaseSp;

        if(_spriteDictionary.TryGetValue(name , out releaseSp ))
        {
            _spriteDictionary.Remove(name);
            releaseSp.Clear();
            releaseSp = null;

            Debug.Log("Release Atlas : " + name + ", spriteContainerList Count: " + _spriteDictionary.Count);

            Resources.UnloadUnusedAssets();
        }
    }


    /// <summary>
    ///  현재 로드된 팝업 이름으로 스프라이트를 가지고 오는 함수
    /// </summary>
    /// <param name="popupName">로드된 팝업 이름</param>
    /// <param name="spriteName">가져올 이미지 이름</param>
    /// <returns></returns>
    public Sprite GetSprite(string popupName, string spriteName)
    {
        SpriteContainer sprcontainer;

        if (_spriteDictionary.TryGetValue(popupName , out sprcontainer))
            return sprcontainer.GetSprite(spriteName);
        else
            return null;
    }    

    // 로비에서 쓰이는 공통 스프라이트 모음집에서 로드함 
    public Sprite GetLobbySprite(string spriteName)
    {
        SpriteContainer sprcontainer;

        if (_spriteDictionary.TryGetValue(Lobby, out sprcontainer))
            return sprcontainer.GetSprite(spriteName);
        else
            return null;
    }

    // InGame에서 쓰이는 공통 스프라이트 모음집에서 로드함 
    public Sprite GetGameSprite(string spriteName)
    {
        SpriteContainer sprcontainer;

        if (_spriteDictionary.TryGetValue(Game, out sprcontainer))
            return sprcontainer.GetSprite(spriteName);
        else
            return null;
    }

    // 공통 스프라이트
    public Sprite GetCommonSprite(string spriteName)
    {
        SpriteContainer sprcontainer;

        if (_spriteDictionary.TryGetValue(Common, out sprcontainer))
            return sprcontainer.GetSprite(spriteName);
        else
            return null;
    }

}

public static class UISpriteExtexsionMethod
{
    static public Sprite GetAtlasSprite(this UIPopupBase uiBase, string spriteName)
    {
        Sprite temp = null;

        UISpriteRepository.Instance.GetSprite(uiBase.GetType().ToString(), spriteName);

        return temp;
    }
}
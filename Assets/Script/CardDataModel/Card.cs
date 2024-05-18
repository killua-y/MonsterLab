using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card
{
    // 卡牌编号
    public int id;
    // 卡牌在游戏内的专属编号
    public int uniqueID;
    // 卡牌名称
    public string cardName;
    // 卡牌职业颜色
    public CardColor color;
    // 卡牌稀有度
    public CardRarity cardRarity;
    // 卡牌释放代价，怪兽卡为rank，魔法装备卡为费用
    public int cost;
    // 卡牌释放类型
    public CastType castType;
    // 卡牌数值
    public int effectData;
    // 卡片效果文本
    public string effectText;
    // script位置 (怪兽卡为怪兽script，魔法装备卡为卡牌释放script)
    public string scriptLocation;
    // 图片位置
    public string imageLocation;

    public Card(int _id, int _uniqueID, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _scriptLocation, string _imageLocation)
    {
        this.id = _id;
        this.uniqueID = _uniqueID;
        this.cardName = _cardName;
        this.color = _color;
        this.cardRarity = _cardRarity;
        this.cost = _cost;
        this.castType = _castType;
        this.effectData = _effectData;
        this.effectText = _effectText;
        this.scriptLocation = _scriptLocation;
        this.imageLocation = _imageLocation;
    }

    // 怪兽卡类
    public class MonsterCard : Card
    {
        // 种族
        public MonsterType type;
        // 攻击力
        public int attackPower;
        // 生命值
        public int healthPoint;
        // 攻击距离
        public float attackRange;
        // 蓝量
        public int Mana;
        // 怪兽模型位置
        public string modelLocation;


        public MonsterCard(int _id, int _uniqueID, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation, string _imageLocation,
        MonsterType _type, int _attackPower, int _healthPoint, float _attackRange, int _Mana, string _modelLocation) :
            base(_id, _uniqueID, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation)
        {
            this.type = _type;
            this.attackPower = _attackPower;
            this.healthPoint = _healthPoint;
            this.attackRange = _attackRange;
            this.Mana = _Mana;
            this.modelLocation = _modelLocation;
        }
    }

    // 法术卡类 继承自卡牌类
    public class SpellCard : Card
    {
        public SpellCard(int _id, int _uniqueID, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation, string _imageLocation) :
            base(_id, _uniqueID, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation)
        {

        }
    }

    // 装备卡类，继承自卡牌类
    public class ItemCard : Card
    {
        public ItemCard(int _id, int _uniqueID, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation, string _imageLocation) :
            base(_id, _uniqueID, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation)
        {

        }
    }


}





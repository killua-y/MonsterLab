using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card
{
    // 卡牌编号
    public int id;
    // 卡牌名称
    public string cardName;
    // 卡牌稀有度
    public CardRarity cardRarity;
    // 卡牌在游戏内的专属编号
    public int uniqueID;

    // 卡片效果文本
    public string effectText;
    // 卡片位置
    public string cardLocation;
    // 图片位置
    public string imageLocation;

    public Card(int _id, string _cardName, CardRarity _cardRarity, int _uniqueID, string _effectText, string _cardLocation, string _imageLocation)
    {
        this.id = _id;
        this.cardName = _cardName;
        this.cardRarity = _cardRarity;
        this.uniqueID = _uniqueID;
        this.effectText = _effectText;
        this.cardLocation = _cardLocation;
        this.imageLocation = _imageLocation;
    }

    // 怪兽卡类
    public class MonsterCard : Card
    {
        // 种族
        public MonsterType type;
        // 星级
        public int rank;
        // 召唤条件
        public int sacrifice;
        // 攻击力
        public int attack;
        // 生命值
        public int healthPoint;
        // 攻击距离
        public int attackRange;
        // 效果数值
        public int effectData;
        // 模型位置
        public string modelLocation;


        public MonsterCard(int _id, string _cardName, CardRarity _cardRarity, int _uniqueID, MonsterType _type,
            int _rank, int _sacrifice, int _attack, int _healthPoint, int _attackRange,
            int _effectData, string _effectText, string _cardLocation, string _imageLocation,
            string _modelLocation) : base(_id, _cardName, _cardRarity, _uniqueID, _effectText, _cardLocation, _imageLocation)
        {
            this.type = _type;
            this.rank = _rank;
            this.sacrifice = _sacrifice;
            this.attack = _attack;
            this.healthPoint = _healthPoint;
            this.attackRange = _attackRange;
            this.effectData = _effectData;
            this.effectText = _effectText;
            this.cardLocation = _cardLocation;
            this.modelLocation = _modelLocation;
        }

    }

    // 法术卡类 继承自卡牌类
    public class SpellCard : Card
    {
        // 卡片费用
        public int cost;
        // 卡片数值
        public int effectData;

        public SpellCard(int _id, string _cardName, CardRarity _cardRarity, int _uniqueID, int _cost,
            int _effectData, string _effectText, string _cardLocation, string _imageLocation) : base(_id, _cardName, _cardRarity, _uniqueID, _effectText, _cardLocation, _imageLocation)
        {
            this.cost = _cost;
            this.effectData = _effectData;
        }
    }

    // 装备卡类，继承自卡牌类
    public class ItemCard : Card
    {
        // 卡片费用
        public int cost;
        // 卡片数值
        public int effectData;

        public ItemCard(int _id, string _cardName, CardRarity _cardRarity, int _uniqueID, int _cost,
            int _effectData, string _effectText, string _cardLocation, string _imageLocation) : base(_id, _cardName, _cardRarity, _uniqueID, _effectText, _cardLocation, _imageLocation)
        {
            this.cost = _cost;
            this.effectData = _effectData;
        }
    }


}





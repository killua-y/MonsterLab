
using System.Collections.Generic;

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
        public int rank;
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
        // 怪兽技能脚本位置
        public string skillScriptLocation;
        // 被装备的卡片list
        public List<Card> equipedCard;


        public MonsterCard(int _id, int _uniqueID, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation, string _imageLocation,
        int _rank, MonsterType _type, int _attackPower, int _healthPoint, float _attackRange,
        int _Mana, string _modelLocation, string _skillScriptLocation, List<Card> _equipedCard = null) :
            base(_id, _uniqueID, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation)
        {
            this.rank = _rank;
            this.type = _type;
            this.attackPower = _attackPower;
            this.healthPoint = _healthPoint;
            this.attackRange = _attackRange;
            this.Mana = _Mana;
            this.modelLocation = _modelLocation;
            this.skillScriptLocation = _skillScriptLocation;
            if (_equipedCard == null)
            {
                this.equipedCard = new List<Card>();
            }
            else
            {
                this.equipedCard = _equipedCard;
            }
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

    // Static method to clone a card
    public static Card CloneCard(Card originalCard)
    {
        if (originalCard is MonsterCard monsterCard)
        {
            return new MonsterCard(
                monsterCard.id,
                monsterCard.uniqueID,
                monsterCard.cardName,
                monsterCard.color,
                monsterCard.cardRarity,
                monsterCard.cost,
                monsterCard.castType,
                monsterCard.effectData,
                monsterCard.effectText,
                monsterCard.scriptLocation,
                monsterCard.imageLocation,
                monsterCard.rank,
                monsterCard.type,
                monsterCard.attackPower,
                monsterCard.healthPoint,
                monsterCard.attackRange,
                monsterCard.Mana,
                monsterCard.modelLocation,
                monsterCard.skillScriptLocation,
                monsterCard.equipedCard
            );
        }
        else if (originalCard is SpellCard spellCard)
        {
            return new SpellCard(
                spellCard.id,
                spellCard.uniqueID,
                spellCard.cardName,
                spellCard.color,
                spellCard.cardRarity,
                spellCard.cost,
                spellCard.castType,
                spellCard.effectData,
                spellCard.effectText,
                spellCard.scriptLocation,
                spellCard.imageLocation
            );
        }
        else if (originalCard is ItemCard itemCard)
        {
            return new ItemCard(
                itemCard.id,
                itemCard.uniqueID,
                itemCard.cardName,
                itemCard.color,
                itemCard.cardRarity,
                itemCard.cost,
                itemCard.castType,
                itemCard.effectData,
                itemCard.effectText,
                itemCard.scriptLocation,
                itemCard.imageLocation
            );
        }
        else
        {
            // Handle other card types or throw an exception
            throw new System.InvalidOperationException("Unknown card type");
        }
    }
}





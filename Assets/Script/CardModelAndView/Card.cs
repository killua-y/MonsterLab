
using System.Collections.Generic;

[System.Serializable]
public class Card
{
    // 卡牌编号
    public int id;
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
    // 关键词列表
    public List<string> keyWords;

    public Card(int _id, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _scriptLocation,
        string _imageLocation, List<string> _keyWords = null)
    {
        this.id = _id;
        this.cardName = _cardName;
        this.color = _color;
        this.cardRarity = _cardRarity;
        this.cost = _cost;
        this.castType = _castType;
        this.effectData = _effectData;
        this.effectText = _effectText;
        this.scriptLocation = _scriptLocation;
        this.imageLocation = _imageLocation;
        this.keyWords = _keyWords ?? new List<string>();
    }

    public Card(Card original)
    {
        this.id = original.id;
        this.cardName = original.cardName;
        this.color = original.color;
        this.cardRarity = original.cardRarity;
        this.cost = original.cost;
        this.castType = original.castType;
        this.effectData = original.effectData;
        this.effectText = original.effectText;
        this.scriptLocation = original.scriptLocation;
        this.imageLocation = original.imageLocation;
        this.keyWords = new List<string>(original.keyWords);
    }

    // 怪兽卡类
    [System.Serializable]
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
        // 怪兽的小头像位置
        public string smallIconLocation;
        // 被装备的卡片list
        public List<Card> equippedCard;


        public MonsterCard(int _id, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation, string _imageLocation,
        int _rank, MonsterType _type, int _attackPower, int _healthPoint, float _attackRange,
        int _Mana, string _modelLocation, string _skillScriptLocation, string _smallIconLocation,
        List<Card> _equippedCard = null, List<string> _keyWords = null) :
            base(_id, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation, _keyWords)
        {
            this.rank = _rank;
            this.type = _type;
            this.attackPower = _attackPower;
            this.healthPoint = _healthPoint;
            this.attackRange = _attackRange;
            this.Mana = _Mana;
            this.modelLocation = _modelLocation;
            this.skillScriptLocation = _skillScriptLocation;
            this.smallIconLocation = _smallIconLocation;
            this.equippedCard = _equippedCard ?? new List<Card>();
        }

        public MonsterCard(MonsterCard original) : base(original)
        {
            this.rank = original.rank;
            this.type = original.type;
            this.attackPower = original.attackPower;
            this.healthPoint = original.healthPoint;
            this.attackRange = original.attackRange;
            this.Mana = original.Mana;
            this.modelLocation = original.modelLocation;
            this.skillScriptLocation = original.skillScriptLocation;
            this.smallIconLocation = original.smallIconLocation;
            this.equippedCard = new List<Card>(original.equippedCard);
        }
    }

    // 法术卡类 继承自卡牌类
    [System.Serializable]
    public class SpellCard : Card
    {
        public SpellCard(int _id, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation,
        string _imageLocation, List<string> _keyWords = null) :
            base(_id, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation, _keyWords)
        {
        }

        public SpellCard(SpellCard original) : base(original)
        {
        }
    }

    // 装备卡类，继承自卡牌类
    [System.Serializable]
    public class ItemCard : Card
    {
        public bool isSpecial;

        public ItemCard(int _id, string _cardName, CardColor _color, CardRarity _cardRarity,
        int _cost, CastType _castType, int _effectData, string _effectText, string _cardLocation,
        string _imageLocation, List<string> _keyWords = null) :
            base(_id, _cardName, _color, _cardRarity, _cost, _castType, _effectData, _effectText, _cardLocation, _imageLocation, _keyWords)
        {
        }

        public ItemCard(ItemCard original) : base(original)
        {
        }
    }

    public static Card CloneCard(Card originalCard)
    {
        switch (originalCard)
        {
            case MonsterCard monsterCard:
                return new MonsterCard(monsterCard);
            case SpellCard spellCard:
                return new SpellCard(spellCard);
            case ItemCard itemCard:
                return new ItemCard(itemCard);
            default:
                throw new System.InvalidOperationException("Unknown card type");
        }
    }
}





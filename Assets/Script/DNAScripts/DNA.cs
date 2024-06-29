

using System.Collections.Generic;

[System.Serializable]
public class DNA
{
    // 编号
    public int id;
    // DNA名称
    public string DNAName;
    // DNA职业颜色
    public CardColor NDAColor;
    // 稀有度
    public CardRarity DNARarity;
    // 卡牌数值
    public int effectData;
    // 卡片效果文本
    public string effectText;
    // script位置
    public string scriptLocation;
    // 图片位置
    public string imageLocation;
    // 关键词列表
    public List<string> keyWords;

    public DNA(int _id, string _DNAName, CardColor _NDAColor, CardRarity _DNARarity, int _effectData,
        string _effectText, string _scriptLocation, string _imageLocation, List<string> _keyWords = null)
    {
        this.id = _id;
        this.DNAName = _DNAName;
        this.NDAColor = _NDAColor;
        this.DNARarity = _DNARarity;
        this.effectData = _effectData;
        this.effectText = _effectText;
        this.scriptLocation = _scriptLocation;
        this.imageLocation = _imageLocation;
        this.keyWords = _keyWords ?? new List<string>();
    }
}

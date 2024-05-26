

public class DNA
{
    // 编号
    public int id;
    // DNA名称
    public string DNAName;
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

    public DNA(int _id, string _DNAName, CardRarity _DNARarity, int _effectData,
        string _effectText, string _scriptLocation, string _imageLocation)
    {
        this.id = _id;
        this.DNAName = _DNAName;
        this.DNARarity = _DNARarity;
        this.effectData = _effectData;
        this.effectText = _effectText;
        this.scriptLocation = _scriptLocation;
        this.imageLocation = _imageLocation;
    }
}

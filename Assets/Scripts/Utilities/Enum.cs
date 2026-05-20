public enum CardType
{
    PetCard,//宠物卡
    AttackCard,//攻击卡
    AbilityCard,//效果卡
    PAffectCard,//宠物效果卡
    SpecialCard,
    None
}

public enum TurnState
{
    PlayerTurnStart,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurnStart,
    EnemyTurn,
    EnemyTurnEnd,
    None
}

public enum RoomType
{
    Start,
    Normal,
    Exit,
    Chest,
    Event,
    Task,
    Boss,
}

public enum EquipmentType
{
    /// <summary>
    /// 头盔
    /// </summary>
    Helmet,

    /// <summary>
    /// 胸甲
    /// </summary>
    Cuirass,

    /// <summary>
    /// 武器
    /// </summary>
    Weapon,

    /// <summary>
    /// 鞋子
    /// </summary>
    Shoe
}


public enum Quality
{
    Ordinary,
    Excellent,
    Rare,
    Epic,
    Legendary,
    Mythical
}

public enum CameraMode
{
    Main,
    Map,
}

/// <summary>
/// 道具类型
/// </summary>

public enum ItemType
{
    Normal,
    Equipment,
    Consumable,
    Material,
}
/// <summary>
/// 道具品质
/// </summary>

public enum ItemRate
{
    white,
    green,
    blue,
    purple,
    gold,
    red,
}
/// <summary>
/// 道具品质
/// </summary>
public enum CardRate
{
    white,
    green,
    blue,
    purple,
    gold,
    red,
}

public enum ItemBoxType
{
    bag,
    warehouse,
    chest,
}

public enum CameraType
{
    None,
    Battle,
    Map,
}
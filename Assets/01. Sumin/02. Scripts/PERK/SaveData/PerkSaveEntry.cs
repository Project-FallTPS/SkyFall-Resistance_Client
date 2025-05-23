[System.Serializable]
public class PerkSaveEntry
{
    public EPerkType PerkType;
    public int Count;

    public PerkSaveEntry(EPerkType perkType, int count)
    {
        PerkType = perkType;
        Count = count;
    }
}
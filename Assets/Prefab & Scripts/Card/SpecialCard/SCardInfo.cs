using UnityEngine;

[System.Serializable]
public class SCardInfo : MonoBehaviour
{
    public string cardName;
    [TextArea(1,25)]public string description;

    public SCardInfo Clone()
    {
        SCardInfo clone = new SCardInfo();
        clone.cardName = this.cardName;
        clone.description = this.description;
        return clone;
    }
}

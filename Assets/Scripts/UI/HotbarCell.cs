using UnityEngine;
using UnityEngine.UI;

public class HotbarCell : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image cellBackground;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite normalSprite;

    public void SetItem(ItemDataSO item)
    {
        if (item == null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            return;
        }

        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
    }

    public void SetSelected(bool selected)
    {
        cellBackground.sprite = selected ? selectedSprite : normalSprite;
    }
}
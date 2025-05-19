using Lop.Survivor.inventroy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolSlot : MonoBehaviour
{
    public Lop.Survivor.inventroy.Item.ItemData itemData;

    [SerializeField] private Inventory inventory;
    [SerializeField] private ToolWheel toolWheel;

    public GameObject textBackGround;
    public GameObject countBackgroundImage;

    public TextMeshProUGUI countText;

    public Sprite emptySlotImg;
    public Sprite defaultSlotImg;

    public Image currentSlotImg;
    public Image currentItemImg;

    public Transform imagePos;

    public int slotItemCount;
    public int MaxCount = 20;  // 예시로 최대 스택 개수


    public void InitToolSlot(Lop.Survivor.inventroy.Item.ItemData _itemData) // 이미지 띄우기, 이미지가 이미 있다면 지우고 띄우기
    {
        itemData = _itemData;
        toolWheel.currentSlot.slotItemCount = inventory.currentSlot.itemCount;

        if (itemData.canMerge == true)
        {
            countText.gameObject.SetActive(true);
            countBackgroundImage.gameObject.SetActive(true);
            countText.text = slotItemCount.ToString();
        }
        else { countText.gameObject.SetActive(false); }


        if (currentSlotImg != null)
        {
            currentSlotImg.sprite = emptySlotImg;
            currentItemImg.enabled = true;
            currentItemImg.sprite = inventory.currentSlot.slotItemData.itemIventoryPrefab.GetComponent<Image>().sprite;
            toolWheel.isFavorite = false;
        }
    }

    public void ClearSlot()
    {
        itemData = new Lop.Survivor.inventroy.Item.ItemData();
        currentSlotImg.sprite = defaultSlotImg;
        countBackgroundImage.gameObject.SetActive(false);
        countText.gameObject.SetActive(false);
        currentItemImg.enabled = false;
        currentItemImg.sprite = null;
    }
}
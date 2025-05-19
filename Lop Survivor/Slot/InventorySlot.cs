using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lop.Survivor.inventroy.Item;

namespace Lop.Survivor.inventroy.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public ItemData slotItemData; // 슬롯에 담겨있는 아이템의 데이터

        public GameObject itemPrefab; // 슬롯에 표기되고 있는 아이템의 이미지 프리팹
        public GameObject countBackgroundImage;

        public TextMeshProUGUI countText;

        public Slider durabilitySlider;

        public int itemCount;
        public int maxCount = 20;  // 예시로 최대 스택 개수

        /// <summary>
        /// 슬롯을 초기화 해주는 함수
        /// </summary>
        /// <param name="slotImage">슬롯에 넣을 아이템 데이터</param>s
        public void InitSlotItem(Item.Item item, int count)
        {
            if (item == null)
            {
                slotItemData = null;
                itemCount = 0;
                Destroy(itemPrefab);
            }
            else
            {
                slotItemData = item.data;
                if (itemPrefab != null)
                {
                    Destroy(itemPrefab);
                }
                itemPrefab = Instantiate(item.data.itemIventoryPrefab, transform);
                Initialize(slotItemData, count);
                itemPrefab.transform.SetSiblingIndex(0);

                itemCount = count;
                countText.text = count.ToString();
            }
        }

        public void Initialize(Item.ItemData data, int count) // id 새로 받기
        {
            slotItemData = data;

            if (data.canMerge)
            {
                itemCount = count;
            }

            if (data.id == 0)
            {
                data.id = (int)IdManager.Instance.GetNewId();
            }
        }

        public void InitSlotItemData(Item.ItemData data, int count)
        {
            durabilitySlider.gameObject.SetActive(true);
            if (data == null)
            {
                slotItemData = null;
                itemCount = 0;
                Destroy(itemPrefab);
                durabilitySlider.gameObject.SetActive(false);
            }
            else
            {
                slotItemData = data;
                if (itemPrefab != null)
                {
                    Destroy(itemPrefab);
                }
                if (!slotItemData.isDurability)
                {
                    durabilitySlider.gameObject.SetActive(false);
                }
                else
                {
                    durabilitySlider.value = slotItemData.currentDurability / slotItemData.maxDurability;
                }
                itemPrefab = Instantiate(data.itemIventoryPrefab, transform.position, Quaternion.identity, transform);
                itemPrefab.transform.SetSiblingIndex(0);
                itemCount = count;
                countText.text = count.ToString();
            }
        }

        public bool CanMerge(Item.ItemData newData)
        {
            return slotItemData != null &&
                   slotItemData.itemName == newData.itemName &&
                   slotItemData.canMerge;
        }
    }
}


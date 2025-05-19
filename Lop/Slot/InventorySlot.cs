using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lop.Survivor.inventroy.Item;

namespace Lop.Survivor.inventroy.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public ItemData slotItemData; // ���Կ� ����ִ� �������� ������

        public GameObject itemPrefab; // ���Կ� ǥ��ǰ� �ִ� �������� �̹��� ������
        public GameObject countBackgroundImage;

        public TextMeshProUGUI countText;

        public Slider durabilitySlider;

        public int itemCount;
        public int maxCount = 20;  // ���÷� �ִ� ���� ����

        /// <summary>
        /// ������ �ʱ�ȭ ���ִ� �Լ�
        /// </summary>
        /// <param name="slotImage">���Կ� ���� ������ ������</param>s
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

        public void Initialize(Item.ItemData data, int count) // id ���� �ޱ�
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


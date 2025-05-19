using System.Collections.Generic;
using Lop.Survivor.inventroy.Item;
using UnityEngine;

namespace Lop.Survivor.inventroy
{
    public class ToolWheel : MonoBehaviour
    {
        [SerializeField] private RectTransform selectArrowParent;

        [SerializeField] private Inventory inventory;
        [SerializeField] public InventoryHandler inventoryHandler;
        [SerializeField] private GameObject craftingPanel;
        [SerializeField] private GameObject cookingPanel;

        public GameObject toolWheelPanel;
        public ToolSlot currentSlot;

        public List<ToolSlot> toolSlots = new List<ToolSlot>();

        public PenguinBody penguinBody;
        public PenguinFunction penguinFunction;
        public NetworkPlayer networkPlayer;

        public bool isFavorite = false;
        public bool isFavoriteToolWheel = false;

        private int currentSlotIndex = 0;
        private float[] arrowAnchorTransforms = { 0, 288, 216, 144, 72 };

        private void Awake()
        {
            penguinBody = GetComponentInParent<PenguinBody>();
            penguinFunction = GetComponentInParent<PenguinFunction>();
            networkPlayer = GetComponentInParent<NetworkPlayer>();
        }

        private void Start()
        {
            currentSlot = toolSlots[currentSlotIndex];
            toolWheelPanel.SetActive(false);
        }

        private void Update()
        {
            if (!penguinBody.isLocalPlayer) { return; }

            MoveSelctSlot();
        }
        /// <summary>
        /// ������ ������ �����̴� �Լ�
        /// </summary>
        private void MoveSelctSlot()
        {
            if (toolWheelPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentSlotIndex++;
                    if (currentSlotIndex >= toolSlots.Count)
                    {
                        currentSlotIndex = 0;
                    }
                    currentSlot = toolSlots[currentSlotIndex];
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentSlotIndex--;
                    if (currentSlotIndex < 0)
                    {
                        currentSlotIndex = toolSlots.Count - 1;
                    }
                    currentSlot = toolSlots[currentSlotIndex];
                }
                MoveSelectImage();
            }
        }

        /// <summary>
        /// ���� ������ ������ ǥ�����ִ� �̹��� �̵�
        /// </summary>
        /// <param name="slot"></param>
        private void MoveSelectImage()
        {
            selectArrowParent.rotation = Quaternion.Euler(0, 0, arrowAnchorTransforms[currentSlotIndex]);
        }
        /// <summary>
        /// �� �ٿ� �������� ���
        /// </summary>
        public void FavoriteItem()
        {
            if (isFavorite)
            {
                foreach (var slot in toolSlots)
                {
                    //if (slot.itemData == null)
                    //{
                    //    continue;
                    //}
                    long itemId = slot.itemData.id;
                    if (inventory.currentSlot.slotItemData.id == itemId)
                    {
                        slot.ClearSlot();
                    }

                    currentSlot.InitToolSlot(inventory.currentSlot.slotItemData);
                    isFavorite = false;
                }

                inventoryHandler.currentState = InventoryHandlerState.FavoriteState;
                penguinBody.UnInput(true);
            }
        }
        /// <summary>
        /// �� �ٿ��� �κ��丮â�� �������ִ� �Լ�
        /// </summary>
        public void ClearSlot()
        {
            if (currentSlot.itemData.id != 0) //������� �ʴٸ� ������ ���
            {
                currentSlot.ClearSlot();
            }
        }
        /// <summary>
        /// �κ��丮���� ������ ����
        /// </summary>
        public void FavoriteInInventoryItem()
        {
            toolWheelPanel.SetActive(true);
            inventory.inventoryPanel.SetActive(false);

            foreach (var slot in toolSlots)
            {
                if (slot.itemData == null)
                {
                    continue;
                }
                long itemId = slot.itemData.id;
                if (inventory.currentSlot.slotItemData.id == itemId)
                {
                    slot.ClearSlot();
                }

                currentSlot.InitToolSlot(inventory.currentSlot.slotItemData);
                inventoryHandler.currentState = InventoryHandlerState.FavoriteState;
            }
        }

        /// <summary>
        /// ���ã�� â ���� �ݱ�
        /// </summary>
        public void FavoriteOn()
        {
            if (!inventory.inventoryPanel.activeSelf && !toolWheelPanel.activeSelf && !craftingPanel.activeSelf && !cookingPanel.activeSelf) //�� �� �������� ��
            {
                toolWheelPanel.SetActive(true);
                SoundManager.Instance.PlaySFX("UIClick");
                inventoryHandler.currentState = InventoryHandlerState.FavoriteState;
                penguinBody.UnInput(false);
                penguinFunction.MoveControllerOffFunction();
            }
            //else if (!InventoryManager.Instance.inventoryPanel.activeSelf && toolWheelPanel.activeSelf)
            //{
            //    toolWheelPanel.SetActive(false);
            //}
        }

        public void FavoriteOff()
        {
            toolWheelPanel.SetActive(false);
            SoundManager.Instance.PlaySFX("UIClick");
            inventoryHandler.currentState = InventoryHandlerState.Default;
            penguinBody.UnInput(true);
        }

        public void SelectItemInToolWheel()
        {
            penguinFunction.HidCharacterFavoritesCheack();

            if(currentSlot.itemData.itemType == ItemType.Food)
            {
                inventory.MinusCount(currentSlot.itemData.id);

                penguinBody.status.status_hp += currentSlot.itemData.healingValue;
                if (penguinBody.status.status_hp >= penguinBody.status.status_MaxHp)
                {
                    penguinBody.status.status_hp = penguinBody.status.status_MaxHp;
                }

                if (penguinBody.status.status_temperature_gauge < 50)
                {
                    penguinBody.status.status_temperature_gauge += inventory.currentSlot.slotItemData.temperatureRecovery;

                    if (penguinBody.status.status_temperature_gauge > 50)
                    {
                        penguinBody.status.status_temperature_gauge = 50;
                    }

                }
                return;
            } 
            else if(currentSlot.itemData.itemType == ItemType.Block)
            {
                networkPlayer.SelectBlock();
            }

            FavoriteOff();
        }

        public ItemData returnItemData()
        {
            Debug.Log(currentSlot.itemData);
            return currentSlot.itemData;
        }

        public bool ReturnActive()
        {
            if (toolWheelPanel.activeSelf)  //�ǳ��� �����ִٸ� True�� ��ȯ
            {
                return true;
            }
            else return false;
        }
    }
}

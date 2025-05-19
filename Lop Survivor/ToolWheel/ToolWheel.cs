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
        /// 선택한 슬롯을 움직이는 함수
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
        /// 현재 선택한 슬롯을 표시해주는 이미지 이동
        /// </summary>
        /// <param name="slot"></param>
        private void MoveSelectImage()
        {
            selectArrowParent.rotation = Quaternion.Euler(0, 0, arrowAnchorTransforms[currentSlotIndex]);
        }
        /// <summary>
        /// 툴 휠에 아이템을 등록
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
        /// 툴 휠에서 인벤토리창을 열게해주는 함수
        /// </summary>
        public void ClearSlot()
        {
            if (currentSlot.itemData.id != 0) //비어있지 않다면 슬롯을 비움
            {
                currentSlot.ClearSlot();
            }
        }
        /// <summary>
        /// 인벤토리에서 아이템 선택
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
        /// 즐겨찾기 창 열고 닫기
        /// </summary>
        public void FavoriteOn()
        {
            if (!inventory.inventoryPanel.activeSelf && !toolWheelPanel.activeSelf && !craftingPanel.activeSelf && !cookingPanel.activeSelf) //둘 다 꺼져있을 때
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
            if (toolWheelPanel.activeSelf)  //판넬이 켜져있다면 True를 반환
            {
                return true;
            }
            else return false;
        }
    }
}

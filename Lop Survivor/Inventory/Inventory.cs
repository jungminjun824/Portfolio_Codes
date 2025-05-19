using System;
using UnityEngine;
using UnityEngine.UI;

using Lop.Survivor.inventroy.Item;
using Lop.Survivor.inventroy.Item.Ground;
using Lop.Survivor.inventroy.UI;

using Lop.Survivor.Event.Mission;
using System.Collections.Generic;
using Mirror;
using DG.Tweening;

namespace Lop.Survivor.inventroy
{
    public class Inventory : NetworkBehaviour
    {
        public List<Item.ItemData> dataList = new List<Item.ItemData>();
        private ToolWheel toolwheel;
        private InventoryHandler inventoryHandler;

        [Header("CurrentSlot")]
        public InventorySlot currentSlot;
        public Image selectImage;
        private int currentSlotIndex = 0;

        [Header("Slot")]
        public GameObject slotPrefab;
        public Transform slotParent8;
        public Transform slotParent16;
        public GameObject slotBG8;
        public GameObject slotBG16;
        public List<InventorySlot> slotDatas = new List<InventorySlot>();
        public int slotValue8 = 8;
        public int slotValue16 = 16;
        private int columnCount = 4;    //한 줄의 슬롯 개수

        [Header("ContextMenu")]
        public UI.ContextMenu contextMenu;
        public bool isOnContextMenu;
        public GameObject contextMenuObj;

        [Header("ItemSwap")]
        private float holdTime = 0f;
        private float holdThreshold = 0.6f;
        private bool isKeyHold = false;
        private bool isItemSwap = false;
        public bool isChange;
        public InventorySlot changeSlot;
        private GameObject moveItem;

        public int testItemOder;
        JsonItemDataList jsonItemDataList = new JsonItemDataList();
        JsonItemDataSave jsonItemDataSave = new JsonItemDataSave();

        [SerializeField] private Transform dropItemPos;

        float throwForce;

        public GameObject inventoryPanel;
        public GameObject craftingPanel;
        public GameObject cookingPanel;

        public List<Item.Ground.ItemGround> itemGround = null;

        private string itemPrefabPath = "Asset/LastOfPenguinSurvivor_Archived/Resources/inventory/Prefab/";

        [SerializeField] private ScriptableItemData[] itemDataLists;

        [SerializeField] private PenguinFunction penguinFunction;
        [SerializeField] private PenguinBody penguinBody;

        private void Awake()
        {
            toolwheel = GetComponent<ToolWheel>();
            inventoryHandler = GetComponent<InventoryHandler>();
            penguinFunction = GetComponentInParent<PenguinFunction>();
            penguinBody = GetComponentInParent<PenguinBody>();
        }

        private void Start()
        {

            CreateInventory();
            jsonItemDataSave = GetItemFromJson.LoadItemDataFromJson();
            //jsonItemDataList = GetItemFromJson.LoadItemDataFromJsom();
            Debug.Log(jsonItemDataSave.data.Count);

            currentSlot = slotDatas[currentSlotIndex];
            ItemToInventory(ItemCategory.Wood_Sword);
            ItemToInventory(ItemCategory.Wood_Hammer);
            ItemToInventory(ItemCategory.Wood_Spoon);
            ItemToInventory(ItemCategory.Wood_Fork);
            ItemToInventory(ItemCategory.Wood_Axe);

            contextMenuObj.SetActive(false);
            inventoryPanel.SetActive(false);
            isOnContextMenu = false;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            NetworkObjectManager.Instance.itemDataLists = itemDataLists;

            NetworkObjectManager.Instance.RequestAuthority(gameObject);
        }

        private void Update()
        {
            if (!isLocalPlayer) { return; }

            if (!isOnContextMenu)
            {
                MoveSelectSlot();
            }
            else
            {
                MoveSelectContextMenu();
            }

            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    JsonItemDataSave save = new JsonItemDataSave();
            //    save.data = new();
            //    for (int i = 0; i < jsonItemDataList.itemList.Count; i++)
            //    {
            //        ItemData item = jsonItemDataList.itemList[i];
            //        JsonItemData data = new JsonItemData();
            //        if (item == null)
            //        {
            //            save.data.Add(new JsonItemData());
            //            continue;
            //        }

            //        data.itemName = item.itemName;
            //        data.itemType = item.itemType;
            //        data.itemTier = item.itemTier;
            //        data.order = item.order;
            //        data.isDurability = item.isDurability;
            //        data.maxDurability = item.maxDurability;
            //        data.healingValue = item.healingValue;
            //        data.temperatureRecovery = item.temperatureRecovery;
            //        data.canMerge = item.canMerge;
            //        data.maxMerge = item.maxMerge;
            //        data.istemperatureDecrease = item.istemperatureDecrease;

            //        ScriptableItemData itemData = Array.Find(itemDataLists, x => x.itemName == item.itemName);

            //        if (itemData == null) continue;

            //        string fileName = default;
            //        switch (data.itemType)
            //        {
            //            case ItemType.Food:
            //                fileName = "Foods";
            //                break;
            //            case ItemType.Facility:
            //                fileName = "Building";
            //                break;
            //            case ItemType.Block:
            //                fileName = "Materials";
            //                break;
            //            case ItemType.Defult:
            //                fileName = "Materials";
            //                break;
            //            case ItemType.Seed:
            //                fileName = "Seed";
            //                break;
            //            case ItemType.Axe:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.BoxingGlove:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.FishingRod:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Fork:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Hammer:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Scissors:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.SlingShot:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Spoon:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Sword:
            //                fileName = "Tools";
            //                break;
            //            case ItemType.Torch:
            //                fileName = "Tools";
            //                break;
            //        }

            //        data.groundPrefabName = "Inventory/Prefab/" + fileName + "/Ground/" + itemData.itemObject.name;
            //        data.uiPrefabName = "Inventory/Prefab/" + fileName + "/UI/" + itemData.uiObject.name;

            //        save.data.Add(data);
            //    }
            //    GetItemFromJson.SaveItemDataToJson(save);
            //}

            if (Input.GetKeyDown(KeyCode.P))
            {
                ItemToInventory(ItemCategory.Ice_Bomb);
            }

            // 모든 슬롯을 확인하면서 아이템이 있다면 알파값을 0으로 설정, 아이템이 없다면 알파값을 1로 설정
            foreach (var slot in slotDatas)
            {
                Image slotImage = slot.GetComponent<Image>();
                if (slotImage != null)
                {
                    // changeSlot이 null이 아니고, 현재 슬롯이 changeSlot인 경우
                    if (changeSlot != null && slot == changeSlot)
                    {
                        Color newColor = slotImage.color;
                        newColor.a = 1f;  // 알파값을 1로 설정
                        slotImage.color = newColor;
                    }
                    else
                    {
                        if (slot.slotItemData != null && slotImage.color.a != 0f)  // 아이템이 있고, 알파값이 0이 아닐 때만 변경
                        {
                            Color newColor = slotImage.color;
                            newColor.a = 0f;  // 알파값을 0으로 설정
                            slotImage.color = newColor;
                        }
                        else if (slot.slotItemData == null && slotImage.color.a != 1f)  // 아이템이 없고, 알파값이 1이 아닐 때만 변경
                        {
                            Color newColor = slotImage.color;
                            newColor.a = 1f;  // 알파값을 1로 설정
                            slotImage.color = newColor;
                        }
                    }
                }
            }
        }

        [Command]
        public void CmdSpawnItem(string itemName)
        {
            float throwForce = UnityEngine.Random.Range(2.85f, 3.5f);

            GameObject prefab = Instantiate(FindItemGameObject(itemName), penguinBody.dropItemPotion.position, Quaternion.identity);
            prefab.GetComponent<ItemGround>().SetId();

            // Drop 스크립트를 가져오고 위치 설정
            Drop dropScript = prefab.GetComponent<Drop>();
            if (dropScript == null)
            {
                dropScript = prefab.AddComponent<Drop>();
            }

            // dropItemPos를 설정
            dropScript.SetDropPosition(penguinBody.dropItemPotion); // 여기서 dropItemPos를 전달
            dropScript.ApplyForce(throwForce);

            NetworkObjectManager.Instance.AddItemList(prefab.transform);

            RpcSpawnItem(itemName, throwForce);
        }

        [ClientRpc]
        private void RpcSpawnItem(string itemName, float throwForce)
        {
            if (isServer || LopNetworkManager.isLoading) { return; }

            GameObject prefab = Instantiate(FindItemGameObject(itemName), penguinBody.dropItemPotion.position, Quaternion.identity);
            prefab.GetComponent<ItemGround>().SetId();

            // Drop 스크립트를 가져오고 위치 설정
            Drop dropScript = prefab.GetComponent<Drop>();
            if (dropScript == null)
            {
                dropScript = prefab.AddComponent<Drop>();
            }

            // dropItemPos를 설정
            dropScript.SetDropPosition(penguinBody.dropItemPotion); // 여기서 dropItemPos를 전달
            dropScript.ApplyForce(throwForce);
        }


        public GameObject FindItemGameObject(string itemName)
        {
            foreach (var item in itemDataLists)
            {
                if (item.itemName == itemName)
                {
                    return item.itemObject;
                }
            }

            return null;
        }

        public void DropItemGameObject(string itemName)
        {
            CmdDropItemGameObject(itemName);
        }

        [Command]
        public void CmdDropItemGameObject(string itemName)
        {
            GameObject prefab = Instantiate(FindItemGameObject(itemName), penguinBody.dropItemPotion.position, Quaternion.identity);
            NetworkObjectManager.Instance.AddItemList(prefab.transform);

            RpcDropItemGameObject(itemName);
        }

        [ClientRpc]
        private void RpcDropItemGameObject(string itemName)
        {
            if (isServer || LopNetworkManager.isLoading) { return; }

            GameObject prefab = Instantiate(FindItemGameObject(itemName), penguinBody.dropItemPotion.position, Quaternion.identity);
        }

        public void ItemToInventory(string name)
        {
            JsonItemData itemData = null;
            foreach (var item in jsonItemDataSave.data)
            {
                if (item.itemName == name)
                {
                    itemData = item;
                }
            }
            if (itemData == null)
            {
                Debug.Log("오류");
            }
            else
            {
                Debug.Log(itemData.groundPrefabName);
                Debug.Log(Resources.Load<GameObject>(itemData.groundPrefabName));
                GameObject prefab = Instantiate(Resources.Load<GameObject>(itemData.groundPrefabName));
                ItemGround groundItem = prefab.GetComponent<ItemGround>();
                itemData.CopyItemData(groundItem.data);
                groundItem.data.itemIventoryPrefab = Resources.Load<GameObject>(itemData.uiPrefabName);
                groundItem.data.itemPrefab = Resources.Load<GameObject>(itemData.groundPrefabName);
                AddToInventory(groundItem, 1);
                //destroyItemObject = prefab;
                Destroy(prefab);
            }
        }

        public void DropItemGameObject(string itemName, Vector3 transform)
        {
            CmdDropItemGameObject(itemName, transform);
        }

        [Command]
        public void CmdDropItemGameObject(string itemName, Vector3 position)
        {
            JsonItemData itemData = null;
            foreach (var item in jsonItemDataSave.data)
            {
                if (item.itemName == name)
                {
                    itemData = item;
                }
            }
            if (itemData == null)
            {
                Debug.Log("오류");
            }
            Debug.Log(itemData.itemName);
            GameObject prefab = Instantiate(Resources.Load<GameObject>(itemData.groundPrefabName));
            ItemGround itemGround = prefab.GetComponent<ItemGround>();
            itemData.CopyItemData(itemGround.data);
            itemGround.data.itemIventoryPrefab = Resources.Load<GameObject>(itemData.uiPrefabName);
            itemGround.data.itemPrefab = Resources.Load<GameObject>(itemData.groundPrefabName);
            NetworkObjectManager.Instance.AddItemList(prefab.transform);

            RpcDropItemGameObject(itemName, position);
        }

        [ClientRpc]
        private void RpcDropItemGameObject(string itemName, Vector3 position)
        {
            if (isServer || LopNetworkManager.isLoading) { return; }

            GameObject prefab = Instantiate(FindItemGameObject(itemName), position, Quaternion.identity);
        }

        public void FavoriteItem()
        {
            if (!toolwheel.ReturnActive())
            {
                inventoryPanel.SetActive(false);
                toolwheel.toolWheelPanel.SetActive(true);
                toolwheel.isFavorite = true;
                contextMenuObj.SetActive(false);
            }
        }

        /// <summary>
        /// 인벤토리에서 방향키를 사용해 칸 이동
        /// </summary>
        private void MoveSelectSlot()
        {
            if (inventoryPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) // 왼쪽 방향키
                {
                    currentSlotIndex--;
                    if (currentSlotIndex < 0)
                    {
                        currentSlotIndex = slotDatas.Count - 1;
                    }
                    currentSlot = slotDatas[currentSlotIndex];
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) // 오른쪽 방향키
                {
                    currentSlotIndex++;
                    if (currentSlotIndex >= slotDatas.Count)
                    {
                        currentSlotIndex = 0;
                    }
                    currentSlot = slotDatas[currentSlotIndex];
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))  // 위쪽 방향키
                {
                    currentSlotIndex -= columnCount;
                    if (currentSlotIndex < 0)
                    {
                        currentSlotIndex += slotDatas.Count;
                    }
                    currentSlot = slotDatas[currentSlotIndex];
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))  // 아래쪽 방향키
                {
                    currentSlotIndex += columnCount;
                    if (currentSlotIndex >= slotDatas.Count)
                    {
                        currentSlotIndex -= slotDatas.Count;
                    }
                    currentSlot = slotDatas[currentSlotIndex];
                }
                MoveSelectImage(currentSlot);
                if (isItemSwap)
                {
                    MoveSelectItem(currentSlot);
                }
            }
        }

        /// <summary>
        /// 인벤토리에서 A를 눌렀을 때 뜨는 팝업 조작
        /// </summary>
        public void MoveSelectContextMenu()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                contextMenu.currentSelect--;
                if (contextMenu.currentSelect < 0)
                {
                    contextMenu.currentSelect = contextMenu.selectList.Count - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                contextMenu.currentSelect++;
                if (contextMenu.currentSelect >= contextMenu.selectList.Count)
                {
                    contextMenu.currentSelect = 0;
                }
            }
        }

        /// <summary>
        /// ContextMenu에서 선택한 슬롯에 따라 코드 실행
        /// </summary>
        public void SelectPopUPOption()
        {
            contextMenu.SelectOption();
            isOnContextMenu = contextMenuObj.activeSelf;
        }
        /// <summary>
        /// 현재 선택한 슬롯을 보여주는 이미지
        /// </summary>
        /// <param name="slot"></param>
        private void MoveSelectImage(InventorySlot slot)
        {
            selectImage.transform.SetParent(slot.transform);
            selectImage.transform.SetSiblingIndex(0);
            selectImage.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 아이템 이동상태일 떄 왼쪽 이미지 왼쪽 위로
        /// </summary>
        /// <param name="slot"></param>
        private void MoveSelectItem(InventorySlot slot)
        {
            moveItem.transform.SetParent(slot.transform);
            moveItem.transform.SetAsLastSibling();
            moveItem.transform.localPosition = new Vector3(-20, 20, 0);
            moveItem.transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        /// <summary>
        /// ContextMenu 키는 함수
        /// </summary>
        private void ToggleContextMenu()
        {
            isOnContextMenu = !isOnContextMenu;
            contextMenuObj.SetActive(isOnContextMenu);
            inventoryHandler.currentState = InventoryHandlerState.InventorySelectPopUp;
        }
        /// <summary>
        /// 인벤토리 열고 닫기
        /// </summary>
        public void InventoryOn()
        {
            if (!inventoryPanel.activeSelf && !toolwheel.toolWheelPanel.activeSelf && !craftingPanel.activeSelf && !cookingPanel.activeSelf) //둘 다 꺼져있을 때
            {
                SoundManager.Instance.PlaySFX("UIClick");
                inventoryPanel.SetActive(true);
                inventoryHandler.currentState = InventoryHandlerState.InventoryState;
                penguinFunction.MoveControllerOffFunction();
                penguinBody.UnInput(false);
            }
        }

        public void InventoryOff()
        {
            ContextMenuOff();
            SoundManager.Instance.PlaySFX("UIClick");
            inventoryPanel.SetActive(false);
            inventoryHandler.currentState = InventoryHandlerState.Default;
            penguinBody.UnInput(true);
        }

        public void ContextMenuOff()
        {
            if (contextMenuObj.activeSelf)
            {
                contextMenuObj.SetActive(false);
                isOnContextMenu = false;
                inventoryHandler.currentState = InventoryHandlerState.InventoryState;
            }
        }

        /// <summary>
        /// 누르는 시간에 따라 아이템 이동상태 or ContextMenu띄우기
        /// </summary>
        public void ItemSwap()
        {
            if (Input.GetKeyDown(KeyCode.A) && !isOnContextMenu)
            {
                if (!isItemSwap && currentSlot.slotItemData != null)
                {
                    isKeyHold = true;
                    holdTime = 0;
                }
                else if (isItemSwap)
                {
                    HoldChangeItem();
                }
            }
            if (Input.GetKey(KeyCode.A) && isKeyHold)
            {
                holdTime += Time.deltaTime;

                if (holdTime > holdThreshold && !isItemSwap)
                {
                    isItemSwap = true;
                    isKeyHold = false;
                    HoldChangeItem();
                }
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                if (isKeyHold)
                {
                    if (holdTime < holdThreshold && currentSlot.slotItemData != null)
                    {
                        ToggleContextMenu();
                    }
                    isKeyHold = false;
                }
                holdTime = 0;
            }
        }

        public void HoldChangeItem()
        {
            currentSlot.durabilitySlider.gameObject.SetActive(false);

            if (changeSlot == null)  // 교환할 슬롯을 선택하는 중
            {
                isChange = true;
                changeSlot = currentSlot;  // 현재 슬롯을 교환 대상으로 설정
                changeSlot.slotItemData = currentSlot.slotItemData;
                moveItem = Instantiate(changeSlot.itemPrefab, transform);

                currentSlot.countText.gameObject.SetActive(false);
                currentSlot.countBackgroundImage.SetActive(false);
                selectImage.gameObject.SetActive(false);
                Destroy(currentSlot.itemPrefab);

                Debug.Log("교환할 슬롯 선택됨: " + changeSlot.name);
            }
            else  // 이미 교환할 슬롯이 선택된 상태에서, 현재 슬롯과 교환
            {
                if (currentSlot != changeSlot)  // 현재 선택된 슬롯이 교환 슬롯과 다를 경우
                {
                    // 슬롯에 있는 아이템 교환
                    Item.ItemData tempItemData = currentSlot.slotItemData;
                    int tempitemCount = currentSlot.itemCount;
                    // 현재 슬롯에 교환할 슬롯의 데이터를 넣음
                    if (tempItemData != null && currentSlot.slotItemData.canMerge)
                    {
                        changeSlot.countText.gameObject.SetActive(true);
                        changeSlot.countBackgroundImage.SetActive(true);
                    }
                    currentSlot.InitSlotItemData(changeSlot.slotItemData, changeSlot.itemCount);
                    if (!changeSlot.slotItemData.canMerge)
                    {
                        currentSlot.countText.gameObject.SetActive(false);
                        currentSlot.countBackgroundImage.SetActive(false);
                    }
                    if (currentSlot.slotItemData.canMerge)
                    {
                        currentSlot.countText.gameObject.SetActive(true);
                        currentSlot.countBackgroundImage.SetActive(true);
                    }
                    // 교환할 슬롯에 임시 데이터를 넣음 (교환 완료)

                    changeSlot.InitSlotItemData(tempItemData, tempitemCount);



                    selectImage.gameObject.SetActive(true);
                    Debug.Log("아이템이 교환되었습니다: " + currentSlot.name + " <-> " + changeSlot.name);

                    // 플래그 초기화
                    isChange = false;
                    isItemSwap = false;
                    changeSlot = null;
                    Destroy(moveItem);
                    moveItem = null;
                }
                else if (currentSlot == changeSlot)  // 같은 슬롯을 선택한 경우, 교환 취소
                {
                    currentSlot.InitSlotItemData(changeSlot.slotItemData, currentSlot.itemCount);
                    Destroy(moveItem);
                    selectImage.gameObject.SetActive(true);
                    if (!changeSlot.slotItemData.canMerge)
                    {
                        currentSlot.countText.gameObject.SetActive(false);
                        currentSlot.countBackgroundImage.SetActive(false);
                    }
                    if (currentSlot.slotItemData.canMerge)
                    {
                        currentSlot.countText.gameObject.SetActive(true);
                        currentSlot.countBackgroundImage.SetActive(true);
                    }
                    Debug.Log("같은 슬롯이 선택되어 교환이 취소되었습니다.");
                    isChange = false;
                    changeSlot = null;
                    moveItem = null;
                    isItemSwap = false;
                }

            }
        }


        public void DropItem()
        {
            string itemName = currentSlot.slotItemData.itemName;
            ItemData itemData = currentSlot.slotItemData;
            int itemCount = currentSlot.itemCount;

            foreach (var toolSlot in toolwheel.toolSlots)
            {
                if (toolSlot.itemData.id == currentSlot.slotItemData.id)
                {
                    toolSlot.ClearSlot();
                }
            }

            for (int i = 0; i < currentSlot.itemCount; i++)
            {
                if (currentSlot.slotItemData.itemType == ItemType.IceBomb)
                {
                    throwForce = 50f;
                }
                else throwForce = UnityEngine.Random.Range(2.85f, 3.5f);

                CmdDropItem(itemName, ChunkSync.SerializeToJson(itemData));
            }

            if (penguinFunction.currentItemData != null)
            {
                if (currentSlot.slotItemData.id == penguinFunction.currentItemData.id)
                {
                    penguinFunction.HidController(default);
                    penguinFunction.currentItemData = default;
                }
            }

            MissionManager.Instance.RemoveCollectEvent(itemName, itemCount);

            currentSlot.InitSlotItem(null, 0);
            currentSlot.countText.gameObject.SetActive(false);
            currentSlot.countBackgroundImage.SetActive(false);
            currentSlot.durabilitySlider.gameObject.SetActive(false);
            contextMenuObj.SetActive(false);
        }

        [Command]
        public void CmdDropItem(string itemName, byte[] itemDataToByte)
        {
            GameObject prefab = FindItemGameObject(itemName);

            prefab.GetComponent<ItemGround>().data.InitData(ChunkSync.DeserializeFromJson<ItemData>(itemDataToByte));

            GameObject itemInstance = Instantiate(prefab, dropItemPos.position, Quaternion.Euler(0, 0, dropItemPos.position.z));

            Rigidbody rb = itemInstance.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = itemInstance.AddComponent<Rigidbody>();
            }

            Vector3 throwDirection = dropItemPos.forward;
            throwDirection.y = 1f; // 위로 던지는 힘 유지
            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

            NetworkObjectManager.Instance.AddItemList(itemInstance.transform);

            RpcDropItem(itemName, itemDataToByte, throwForce);
        }

        [ClientRpc]
        private void RpcDropItem(string itemName, byte[] itemDataToByte, float throwForce)
        {
            if (isServer || LopNetworkManager.isLoading) { return; }

            GameObject prefab = FindItemGameObject(itemName);

            prefab.GetComponent<ItemGround>().data.InitData(ChunkSync.DeserializeFromJson<ItemData>(itemDataToByte));

            GameObject itemInstance = Instantiate(prefab, dropItemPos.position, Quaternion.Euler(0, 0, dropItemPos.position.z));

            Rigidbody rb = itemInstance.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = itemInstance.AddComponent<Rigidbody>();
            }

            Vector3 throwDirection = dropItemPos.forward;
            throwDirection.y = 1f; // 위로 던지는 힘 유지
            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

        }

        /// <summary>
        /// 인벤토리 생성
        /// </summary>
        private void CreateInventory()
        {
            for (int i = 0; i < slotValue8; i++)
            {
                GameObject _gameobject = Instantiate(slotPrefab, slotParent8);
                InventorySlot slot = _gameobject.GetComponent<InventorySlot>();
                slot.InitSlotItem(null, 0);
                slotDatas.Add(slot);
            }
        }

        /// <summary>
        /// 아이템을 인벤토리에 넣는 함수
        /// </summary>
        /// <param name="item">넣을 아이템</param>
        /// <param name="count">개수</param>
        public void AddToInventory(Item.Item item, int count)
        {
            InventorySlot slot = null;
            bool maxInventory = true;

            // 1. 먼저 같은 아이템을 찾고, 합칠 수 있으면 합친다
            foreach (var slotData in slotDatas)
            {
                if (slotData.CanMerge(item.data))
                {
                    int newStackCount = slotData.itemCount + count;

                    // 2. maxCount보다 큰지 확인 후 처리
                    if (newStackCount > slotData.maxCount)
                    {
                        int overflowCount = newStackCount - slotData.maxCount;
                        slotData.itemCount = slotData.maxCount;
                        slotData.countText.text = slotData.maxCount.ToString();
                        count = overflowCount;  // 남은 수량을 처리하도록 넘김
                    }
                    else
                    {
                        slotData.itemCount = newStackCount;

                        foreach (var toolSlot in toolwheel.toolSlots)
                        {
                            if (slotData.slotItemData.id == toolSlot.itemData.id)
                            {
                                toolSlot.slotItemCount += count;
                                toolSlot.countText.text = toolSlot.slotItemCount.ToString();
                            }
                        }
                        slotData.countText.text = newStackCount.ToString();

                        MissionManager.Instance.EventCollect(MissionType.Collect, item.data.itemName);

                        return;  // 아이템이 완전히 합쳐졌으면 함수 종료
                    }
                }
            }

            // 3. 남은 수량을 새로운 빈 슬롯에 넣는다
            foreach (var slotData in slotDatas)
            {
                if (slotData.slotItemData == null)
                {
                    slot = slotData;
                    slot.InitSlotItem(item, count);
                    //destroyItemGround.Add(item.data.id, destroyItemObject);


                    // 아이템 타입에 따라 내구도 슬라이더 처리
                    if (item.data.isDurability)
                    {
                        slot.durabilitySlider.gameObject.SetActive(true);  // 내구도 표시
                    }
                    else
                    {
                        slot.durabilitySlider.gameObject.SetActive(false);  // 내구도 숨기기
                    }

                    if (slot.slotItemData.canMerge)
                    {
                        slot.countText.gameObject.SetActive(true);
                        slot.countBackgroundImage.SetActive(true);
                    }
                    maxInventory = false;
                    break;
                }
            }

            if (maxInventory)
            {
                Debug.LogError("인벤토리가 가득찼습니다");
            }
            MissionManager.Instance.EventCollect(MissionType.Collect, item.data.itemName);
        }

        public bool IsFullInventory(Item.Item item)
        {
            foreach (var slotData in slotDatas)
            {
                if (slotData.slotItemData == null)
                {
                    return false;
                }

                if (slotData.CanMerge(item.data))
                {
                    if (slotData.itemCount < slotData.maxCount)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void MinusCount(long id)
        {
            foreach (var slot in slotDatas)
            {
                if (slot.slotItemData == null)
                {
                    continue;
                }
                if (slot.slotItemData.id == id)
                {
                    //if(penguin.currentItemData.itemType != ItemType.Food || penguin.currentItemData.itemType != ItemType.Facility || penguin.currentItemData.itemType != ItemType.Seed)
                    //{
                    //    penguin.currentItemCount = slot.itemCount;
                    //}
                    slot.itemCount--;
                    if (slot.itemCount == 0)
                    {
                        currentSlot.InitSlotItem(null, 0);
                        slot.countText.gameObject.SetActive(false);
                        slot.countBackgroundImage.SetActive(false);
                        currentSlot.countText.gameObject.SetActive(false);
                        currentSlot.countBackgroundImage.SetActive(false);
                        currentSlot.durabilitySlider.gameObject.SetActive(false);
                    }
                    slot.countText.text = slot.itemCount.ToString();

                    foreach (var toolSlot in toolwheel.toolSlots)
                    {
                        if (toolSlot.itemData.id == id)
                        {
                            toolSlot.slotItemCount = slot.itemCount;
                            toolSlot.countText.text = toolSlot.slotItemCount.ToString();

                            if (toolSlot.slotItemCount == 0)
                            {
                                toolSlot.ClearSlot();
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void MinusDurability()
        {
            penguinFunction.currentItemData.currentDurability -= 1;

            foreach (var inventorySlot in slotDatas)
            {
                if (inventorySlot.slotItemData == null) continue;

                if (inventorySlot.slotItemData.id == penguinFunction.currentItemData.id)
                {
                    inventorySlot.slotItemData.currentDurability = penguinFunction.currentItemData.currentDurability;
                    inventorySlot.durabilitySlider.value = inventorySlot.slotItemData.currentDurability / inventorySlot.slotItemData.maxDurability;
                }
                else if (penguinFunction.currentItemData.currentDurability == 0)
                {
                    penguinFunction.HidController(default);
                    penguinFunction.currentItemData = default;

                    currentSlot.InitSlotItem(null, 0);
                    inventorySlot.countText.gameObject.SetActive(false);
                    inventorySlot.countBackgroundImage.SetActive(false);
                    currentSlot.countText.gameObject.SetActive(false);
                    currentSlot.countBackgroundImage.SetActive(false);
                    currentSlot.durabilitySlider.gameObject.SetActive(false);
                }
            }

            foreach (var toolSlot in toolwheel.toolSlots)
            {
                if (toolSlot.itemData == null) continue;

                if (toolSlot.itemData.id == penguinFunction.currentItemData.id)
                {
                    toolSlot.itemData.currentDurability = penguinFunction.currentItemData.currentDurability;
                }
            }
        }
    }
}

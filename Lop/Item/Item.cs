using UnityEngine;

namespace Lop.Survivor.inventroy.Item
{
    public abstract class Item : MonoBehaviour
    {
        public ItemData data;

        protected int itemCount = 0;

        public abstract void Initialize(ItemData data, int count); // 초기화 하는 함수 재정의 하게 만들기
    }
}

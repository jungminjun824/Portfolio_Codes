using UnityEngine;

namespace Lop.Survivor.inventroy.Item
{
    public abstract class Item : MonoBehaviour
    {
        public ItemData data;

        protected int itemCount = 0;

        public abstract void Initialize(ItemData data, int count); // �ʱ�ȭ �ϴ� �Լ� ������ �ϰ� �����
    }
}

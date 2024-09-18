using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardRoom : RoomController
{
    void Start()
    {
        StartCoroutine(WaitCreateReward(.5f));
        EventManager.Inst.AddEvent(EventName.OnOpenDropItem, OnOpenDropItem);
    }
    protected virtual IEnumerator WaitCreateReward(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 dropItemPosition = GameObject.Find("DropItemSpawnPosition").transform.position;

        ResourcesManager.Inst.GetAsset<GameObject>("Assets/BundleAssets/Prefabs/InterObj_DropItem_Normal.prefab",
            delegate(GameObject itemPrefab)
            {
                //TODO：之后生成位置改成玩家杀死的最后一个敌人单位
                GameObject go = Instantiate(itemPrefab, dropItemPosition, Quaternion.identity);
                //go.GetComponent<InteractObj_Chest_DropItem>().SetItem(roomRewardType);
                //TODO: 设置奖品
                go.transform.SetParent(transform);
            });
    }
    private void OnOpenDropItem(string arg1, object arg2)
    {
        StartCoroutine(OpenDoor(.5f));
    }
    protected override void OnDestroy()
    {
        EventManager.Inst.RemoveEvent(EventName.OnOpenDropItem, OnOpenDropItem);
        base.OnDestroy();
    }


}

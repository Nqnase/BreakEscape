using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するため

public class Eventtrigger : MonoBehaviour
{
    [SerializeField] private CameraController controller;
    [SerializeField] private GameObject BreakWalls;
    [SerializeField] private GameObject Effect;
    [SerializeField] private Vector3 effectSpawnPosition; // エフェクトの発生位置

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerEvent();
        }
    }

    public async void TriggerEvent()
    {
        // イベント開始
        controller.isEventActive = true;
        controller.eventTimer = 0f;
        //controller.currentTarget = controller.eventTarget; // カメラのターゲットをイベントターゲットに変更

        // 1秒待機
        await UniTask.Delay(500); // 1000ミリ秒 (1秒) の待機

        SoundManager.Instance.Play("壁崩壊");
        // 壁を破壊
        Destroy(BreakWalls);

        // エフェクトを発生位置にインスタンス化
        Instantiate(Effect, effectSpawnPosition, Quaternion.identity);

        Destroy(this.gameObject);
    }
}

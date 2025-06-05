using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask���g�p���邽��

public class Eventtrigger : MonoBehaviour
{
    [SerializeField] private CameraController controller;
    [SerializeField] private GameObject BreakWalls;
    [SerializeField] private GameObject Effect;
    [SerializeField] private Vector3 effectSpawnPosition; // �G�t�F�N�g�̔����ʒu

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerEvent();
        }
    }

    public async void TriggerEvent()
    {
        // �C�x���g�J�n
        controller.isEventActive = true;
        controller.eventTimer = 0f;
        //controller.currentTarget = controller.eventTarget; // �J�����̃^�[�Q�b�g���C�x���g�^�[�Q�b�g�ɕύX

        // 1�b�ҋ@
        await UniTask.Delay(500); // 1000�~���b (1�b) �̑ҋ@

        SoundManager.Instance.Play("�Ǖ���");
        // �ǂ�j��
        Destroy(BreakWalls);

        // �G�t�F�N�g�𔭐��ʒu�ɃC���X�^���X��
        Instantiate(Effect, effectSpawnPosition, Quaternion.identity);

        Destroy(this.gameObject);
    }
}

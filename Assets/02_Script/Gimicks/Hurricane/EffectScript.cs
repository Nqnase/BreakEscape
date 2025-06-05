using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("����������G�t�F�N�g(�p�[�e�B�N��)")]
    private ParticleSystem particle;

    // Update is called once per frame
    void Start()
    {
        //�C���X�^���X�̐���
        ParticleSystem newParticle = Instantiate(particle);

        //�����ꏊ���A�^�b�`���Ă���I�u�W�F�N�g�̏ꏊ�ɂ���
        newParticle.transform.position = this.transform.position;

        //����
        newParticle.Play();


    }

    private void Update()
    {
        
    }
}

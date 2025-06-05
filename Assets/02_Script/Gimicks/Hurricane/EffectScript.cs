using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
    private ParticleSystem particle;

    // Update is called once per frame
    void Start()
    {
        //インスタンスの生成
        ParticleSystem newParticle = Instantiate(particle);

        //生成場所をアタッチしているオブジェクトの場所にする
        newParticle.transform.position = this.transform.position;

        //生成
        newParticle.Play();


    }

    private void Update()
    {
        
    }
}

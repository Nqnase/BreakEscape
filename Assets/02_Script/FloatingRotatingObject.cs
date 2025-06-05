using UnityEngine;

public class FloatingRotatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.5f; // 上下移動の振幅
    public float floatSpeed = 2f;       // 上下移動の速さ

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;  // 回転速度

    [Header("Effect")]
    [SerializeField] private ParticleSystem keyEffect;

    private Vector3 startPosition;
    private Vector3 effectPosition;

    private ParticleSystem newParticle;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;

        // エフェクト位置を記録
        effectPosition = this.transform.position;

        // パーティクルシステムのインスタンスを生成する。
        newParticle = Instantiate(keyEffect);
        // パーティクルの発生場所をこのスクリプトをアタッチしているGameObjectの場所にする。
        newParticle.transform.position = new Vector3
            (effectPosition.x, effectPosition.y - 1, effectPosition.z);
        // パーティクルを発生させる。
        newParticle.Play();
    }

    void Update()
    {
        // 上下移動
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 回転
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(newParticle);
        }
    }
}

using UnityEngine;

public class FloatingRotatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.5f; // �㉺�ړ��̐U��
    public float floatSpeed = 2f;       // �㉺�ړ��̑���

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;  // ��]���x

    [Header("Effect")]
    [SerializeField] private ParticleSystem keyEffect;

    private Vector3 startPosition;
    private Vector3 effectPosition;

    private ParticleSystem newParticle;

    void Start()
    {
        // �����ʒu���L�^
        startPosition = transform.position;

        // �G�t�F�N�g�ʒu���L�^
        effectPosition = this.transform.position;

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        newParticle = Instantiate(keyEffect);
        // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���B
        newParticle.transform.position = new Vector3
            (effectPosition.x, effectPosition.y - 1, effectPosition.z);
        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();
    }

    void Update()
    {
        // �㉺�ړ�
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // ��]
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

using Cinemachine;
using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    //TODO:����Entity���Ӿ���Ч
    [Header("��Ч����")]
    [SerializeField] private Material hitM;
    [SerializeField] private float flashDuration;
    private Material normalM;

    [Header("��Ļ��")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] float shakeMultiplier;
    public Vector3 shakePower_Parry;
    public Vector3 shakePower_CatchSword;
    public Vector3 shakePower_BeCriticalHitted;

    [Header("��Ӱ��Ч")]
    [SerializeField] private float afterImageCoolDowm;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLoseRate;
    private float afterImageCoolDowmTimer;

    [Header("Ԫ���쳣��ɫ")]
    private Color chilledColor;
    private Color[] igniteColor = new Color[2];
    private Color[] shockedColor = new Color[2];

    [Header("Ԫ��������Ч")]
    [SerializeField] private ParticleSystem igniteParticleFX;
    [SerializeField] private ParticleSystem chillParticleFX;

    [Header("������Ч")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;

    [Header("������Ч")]
    [SerializeField] private GameObject popUpText;

    private void Awake()
    {
        SetUpColor();
    }


    private void SetUpColor()
    {
        igniteColor[0] = new Color32(255, 44, 44, 255);
        igniteColor[1] = new Color32(243, 192, 192, 255);
        chilledColor = new Color32(145, 160, 255, 255);
        shockedColor[0] = new Color32(255, 227, 98, 255);
        shockedColor[1] = new Color32(240, 223, 140, 255);
    }

    private void Start()
    {
        flashDuration = 0.25f;
        sr = GetComponentInChildren<SpriteRenderer>();
        screenShake = GetComponent<CinemachineImpulseSource>();

        normalM = sr.material;
    }

    private void Update()
    {
        afterImageCoolDowmTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 shakePower)
    {
        //TODO:������Ļ����
        Player player = PlayerManager.instance.player;
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * player.facingDirection, shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();

    }
    private IEnumerator FlashFX()
    {   //TODO��ʹ��һ��Э�����ӳ���˸ʱ��������˸

        sr.material = hitM;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = normalM;
    }
    private void RedWhiteBlink()
    {   //TODO:����һ����׽���

        sr.color = (sr.color != Color.white) ? Color.white : Color.red;
    }
    public void CancelRedWhiteBlink()
    {
        CancelInvoke("RedWhiteBlink");
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteParticleFX.Stop();
        chillParticleFX.Stop();
    }
    public void IgniteFXForSeconds(float _seconds)
    {   //����ȼ����Ч

        igniteParticleFX.Play();
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);

    }

    public void ChillFXForSeconds(float _seconds)
    {
        //����������Ч

        chillParticleFX.Play();
        InvokeRepeating("ChillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockedFXForSeconds(float _seconds)
    {
        InvokeRepeating("ShockedColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFX()
    {
        sr.color = sr.color != igniteColor[0] ? igniteColor[0] : igniteColor[1];
    }
    private void ChillColorFX()
    {
        sr.color = chilledColor;
    }
    private void ShockedColorFX()
    {
        sr.color = sr.color != shockedColor[0] ? shockedColor[0] : shockedColor[1];
    }

    public void CreateHitFX(Vector2 _target)
    {
        //TODO:���ɻ�����Ч
        float randomX = Random.Range(-0.5f, 0.5f);
        float randomY = Random.Range(-0.5f, 0.5f);
        float randomZ = Random.Range(-90, 90);

        GameObject newHitFX = Instantiate(hitFX, _target + new Vector2(randomX, randomY), Quaternion.identity);
        newHitFX.transform.Rotate(new Vector3(0, 0, randomZ));
    }
    public void CreateCriticalHitFX(Vector2 _fromPostion, Vector2 _target)
    {
        //TODO:���ɹؼ�������Ч
        float randomX = Random.Range(-0.5f, 0.5f);
        float randomY = Random.Range(-0.5f, 0.5f);
        float randomZ = Random.Range(-40, 40);

        GameObject newCriticalHitFX = Instantiate(criticalHitFX, _target + new Vector2(randomX, randomY), Quaternion.identity);
        if (_fromPostion.x > _target.x)
        {
            newCriticalHitFX.transform.Rotate(0, 0, 180);
        }
        newCriticalHitFX.transform.Rotate(new Vector3(0, 0, randomZ));
    }

    public void CreateAfterImage()
    {
        if (afterImageCoolDowmTimer <= 0)
        {
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
            newAfterImage.GetComponent<AfterImageFX>().SetUpAfterImage(colorLoseRate);
            newAfterImage.transform.rotation = GetComponent<Entity>().transform.rotation;
            afterImageCoolDowmTimer = afterImageCoolDowm;
        }
    }
    public void CreatePopUpText(string _text)
    {
        //TODO:���ɵ����ı�
        float xOffset = Random.Range(-1f, 1f);
        float yOffset = Random.Range(.2f, 1f);

        Vector3 tempPostion = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
        GameObject newPopUpText = Instantiate(popUpText, tempPostion, Quaternion.identity);
        newPopUpText.GetComponent<PopUpTextFX>().SetUpPopUpText(_text);
    }
}

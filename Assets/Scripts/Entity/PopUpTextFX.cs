using TMPro;
using UnityEngine;

public class PopUpTextFX : MonoBehaviour
{
    [SerializeField] private TextMeshPro popUpText;
    [SerializeField] private float flySpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float disappearSpeed;
    private float lifeTimer;

    private void Start()
    {
        lifeTimer = lifeTime;
    }
    private void Update()
    {
        //TODO:超过生命周期淡化
        lifeTimer -= Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y + flySpeed * Time.deltaTime, transform.position.y);
        if (lifeTimer <= 0)
        {

            float alpha = popUpText.color.a - disappearSpeed * Time.deltaTime;
            popUpText.color = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, alpha);

            if (popUpText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetUpPopUpText(string _text)
    {
        popUpText.text = _text;
    }
}

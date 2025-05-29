using TMPro;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class AccessoryBox : MonoBehaviour
{
    public List<EAccessoryType> Mapper;
    [SerializeField] private TextMeshPro _nameText;

    private EAccessoryType _type;
    public float rotationSpeed = 45f;

    private void OnEnable()
    {
        _type = Mapper[Random.Range(0, Mapper.Count)];
        _nameText.text = _type.ToString();

        transform.localScale = Vector3.zero;

        transform.DOScale(120f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform.DOScale(100f, 0.1f).SetEase(Ease.InSine);
            }); //스케일은 모델자체가 100으로 되어있어서 모델을 수정해야할덧?
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        _nameText.transform.rotation = Quaternion.LookRotation(_nameText.transform.position - Camera.main.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Player)) && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            EAccessoryType finalItem = EAccessoryType.Count;

            foreach (var item in Mapper)
            {
                if (item == _type)
                {
                    finalItem = item;
                    if(AccessoryPoolManager.Instance.GetObject(finalItem).TryGetComponent<IAccessory>(out var acc))
                    {
                        receiver.ReceiveAccessory(_type, acc);
                    }
                    break;
                }
            }

            // TODO : 풀 반환
            gameObject.SetActive(false);
            //BoxPoolManager.Instance.ReturnObject(gameObject, EBoxType.AccessoryBox);
        }
    }
}

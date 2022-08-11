using UnityEngine;

public class LightSwither : MonoBehaviour
{
    [Space(10)]
    [SerializeField] bool IsEnable;

    [Space(10)]
    [SerializeField] Light myLight;
    [SerializeField] Transform handler;
    [SerializeField] AudioSource source;
    [SerializeField] GameObject offLamp;

    private void Start()
    {
        myLight.enabled = IsEnable;
        handler.localRotation = IsEnable ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(15, 0, 0);
    }

    public void Interact()
    {
        source.Play();

        IsEnable = !IsEnable;
        myLight.enabled = IsEnable;
        offLamp.SetActive(!IsEnable);

        handler.localRotation = IsEnable ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(15, 0, 0);
    }
}

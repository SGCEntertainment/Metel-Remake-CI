using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    bool InProcess;
    float currentRotAngle;

    AnimationCurve targetCurve;

    [Space(10)]
    public bool IsOpened;

    [Space(10)]
    public bool IsLock;

    [Space(10)]
    [SerializeField] AnimationCurve openCurve;
    [SerializeField] AnimationCurve closeCurve;

    [Space(10)]
    [SerializeField] AudioSource effect;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0, IsOpened ? 90 : 0, 0);
    }

    public void Interact()
    {
        if(InProcess || IsLock)
        {
            return;
        }

        targetCurve = IsOpened ? closeCurve : openCurve;
        IsOpened = !IsOpened;

        StartCoroutine(nameof(Door_Process));
    }

    IEnumerator Door_Process()
    {
        InProcess = true;

        float et = 0.0f;
        float totalOpenTime = 2.7f;

        float multiplayer = 3;

        float initVolume = 0;
        float maxVolume = 1;
        effect.volume = initVolume;

        effect.Play();
        effect.pitch = multiplayer;

        while(et < totalOpenTime)
        {
            effect.volume = Mathf.Lerp(initVolume, maxVolume, et / totalOpenTime);

            currentRotAngle = targetCurve.Evaluate(et / totalOpenTime);
            transform.localRotation = Quaternion.Euler(0, currentRotAngle, 0);

            et += Time.deltaTime * multiplayer;
            yield return null;
        }

        effect.volume = maxVolume;
        effect.Stop();

        InProcess = false;
    }
}

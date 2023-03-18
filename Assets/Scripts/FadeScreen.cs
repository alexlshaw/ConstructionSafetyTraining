using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public float fadeDuration = 2f;
    public Color fadeColour;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onStartGame += FadeIn;
        GameEvents.current.onCompleteDigging += FadeIn;
        GameEvents.current.onFoundationFilled += FadeIn;
        GameEvents.current.onResetLevel += FadeIn;
        GameEvents.current.onStartSecondLevel += FadeIn;

        rend = GetComponent<Renderer>();
        FadeIn();

    }
    public void FadeIn()
    {
        Fade(1, 0);
    }
    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {

        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }
    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        Color newColor = fadeColour;
        newColor.a = alphaIn;
        rend.material.SetColor("_Color", newColor);
        yield return new WaitForSeconds(2);
        float timer = 0;
        while (timer <= fadeDuration)
        {
            newColor = fadeColour;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            rend.material.SetColor("_Color", newColor);
            timer += Time.deltaTime;
            yield return null; //wait for one frame
        }
        Color newColor2 = fadeColour;
        newColor2.a = alphaOut;
        rend.material.SetColor("_Color", newColor2);
        gameObject.SetActive(false);
    }
}

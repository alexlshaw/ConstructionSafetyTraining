using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public float fadeDuration = 0.1f;
    public Color fadeColour;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onTakeDamage += FadeIn;
        GameEvents.current.onTakeDamage += decreaseHP;

        rend = GetComponent<Renderer>();
        FadeIn();
    }

    public void decreaseHP()
    {
        PointsLostHandler.HP -= 5;
        if(PointsLostHandler.HP <= 0)
        {
            GameEvents.current.GameOver();
        }

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





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject UICanvas;
    public GameObject screenTextObj;

    private Text screenText;
    private Outline screenTextOutline;

	// Use this for initialization
	void Start () {
        UICanvas.SetActive(true);
        screenText = screenTextObj.GetComponent<Text>();
        screenTextOutline = screenTextObj.GetComponent<Outline>();

        screenTextObj.SetActive(false);
	}
	

    public void ShowTurnText()
    {
        if(GameManager.State == GameState.MoveTurn)
        {
            screenText.text = "MOVE";
            ShowText(screenTextObj, screenTextOutline);
        }
        else if(GameManager.State == GameState.BulletTurn)
        {
            screenText.text = "Attack";
            ShowText(screenTextObj, screenTextOutline);
        }
    }

    public void ShowText(GameObject textObj, Outline outline)
    {
        StartCoroutine(DoTextFocusEffect(textObj, outline));
    }

    /* Executes an effect on the input text. */
    IEnumerator DoTextFocusEffect(GameObject textObj, Outline outline)
    {
        textObj.SetActive(true);
        outline.effectDistance = new Vector2(6, -1);
        while (outline.effectDistance.x > 1)
        {
            outline.effectDistance = new Vector2(outline.effectDistance.x - .5f, -1);
            yield return new WaitForSeconds(0.05f);
        }
        outline.effectDistance = new Vector2(1, -1);
        yield return new WaitForSeconds(0.5f);
        textObj.SetActive(false);
    }
}

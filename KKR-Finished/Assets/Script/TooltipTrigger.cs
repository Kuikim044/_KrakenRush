using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    private void Start()
    {
        TooltipBox.Hide();

    }
    private void Update()
    {
        content = GamePlayManager.multiplierScore.ToString() + "/" + "10";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipBox.Show(content,header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipBox.Hide();

    }
}

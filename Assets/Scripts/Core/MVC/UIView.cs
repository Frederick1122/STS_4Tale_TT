using UnityEngine;

public class UIView : MonoBehaviour
{
    virtual public void Show()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    virtual public void Hide()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    virtual public void Init(UIModel uiModel)
    {

    }

    virtual public void UpdateView(UIModel uiModel)
    {

    }

    virtual public void Terminate()
    {

    }
}

using UnityEngine;

public class TestMainPanel : PanelBase
{
    public void OnOpen(int number)
    {
        Debug.LogError(number+"   "+gameObject.name);
    }

    public void OnOpen(string str)
    {
        Debug.LogError(str+"   "+gameObject.name);
    }

    public void OnHide()
    {
        Debug.LogError("Hide    "+gameObject.name);
    }

    public void OnReShow()
    {
        Debug.LogError("OnReShow    "+gameObject.name);
    }

    public void OnClose()
    {
        Debug.LogError("Close   "+gameObject.name);
    }

    public override void Show()
    {
        base.Show();
    }
    
    // public override void Hide()
    // {
    //     base.Hide();
    // }
    
}

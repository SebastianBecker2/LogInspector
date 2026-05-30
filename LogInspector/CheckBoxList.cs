namespace LogInspector;

public class CheckBoxList : CheckedListBox
{
    private int lastChecked = -1;

    public CheckBoxList()
    {
        Dock = DockStyle.Fill;
        IntegralHeight = false;
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        if (SelectedIndex == lastChecked)
        {
            return;
        }
        lastChecked = SelectedIndex;
        base.OnSelectedIndexChanged(e);
    }
}

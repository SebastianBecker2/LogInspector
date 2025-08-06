namespace LogInspector
{
    public class CheckBoxList : CheckedListBox
    {
        private int lastChecked = -1;

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
}

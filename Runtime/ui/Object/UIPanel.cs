namespace CofyUI
{
    public class UIPanel<TPanel>: UIInstance<TPanel>, IUIPanel
    {
        public virtual void ShowPanel(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}
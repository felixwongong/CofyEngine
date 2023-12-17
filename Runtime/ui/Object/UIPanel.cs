namespace CofyUI
{
    public abstract class UIPanel<TPanel>: UIInstance<TPanel>, IUIPanel
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
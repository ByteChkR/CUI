namespace CUI.Common
{
    public interface IRenderer
    {
        RenderBuffer Buffer { get; }
        void Render();
    }
}
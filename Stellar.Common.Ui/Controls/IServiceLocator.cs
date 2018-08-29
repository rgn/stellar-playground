namespace Stellar.Common.Ui.Controls
{
    public interface IServiceLocator
    {
        T GetInstance<T>() where T : class;
    }
}

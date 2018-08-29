using System;
using System.Windows;

namespace Stellar.Common.Ui.Controls
{
    public interface IViewLocator
    {
        UIElement GetOrCreateViewType(Type viewType);
    }
}

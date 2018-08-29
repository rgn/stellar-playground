using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Stellar.Common.Ui.Controls
{
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager
    {
        private readonly ResourceDictionary themeResources;

        public ThemeManager()
        {
            this.themeResources = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Stellar.Common.Ui;component/Resources/Theme.xaml")
            };
        }

        public ResourceDictionary GetThemeResources()
        {
            return this.themeResources;
        }
    }
}

using System.Collections.Generic;
using System.Reflection;
using Stellar.Common.Ui;

namespace Stellar.Monitor
{
    public class AppBootstrapper : MefBootstrapperBase
    {
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IShell))
            };
        }
    }
}

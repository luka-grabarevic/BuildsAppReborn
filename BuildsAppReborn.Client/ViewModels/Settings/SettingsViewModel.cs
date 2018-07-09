using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BuildsAppReborn.Client.Interfaces;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SettingsViewModel : ViewModelBase, ICloseable
    {
        [ImportingConstructor]
        internal SettingsViewModel([ImportMany] IEnumerable<ISubSettingsControl> subSettings)
        {
            SubSettings = subSettings.OrderBy(a => a.Order).ToList();
        }

        public IEnumerable<ISubSettingsControl> SubSettings { get; }

        public void OnClose()
        {
            foreach (var closableVm in SubSettings.Select(subSettingsControl => subSettingsControl.DataContext as ICloseable))
            {
                closableVm?.OnClose();
            }
        }
    }
}
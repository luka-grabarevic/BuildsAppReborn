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
        #region Constructors

        [ImportingConstructor]
        internal SettingsViewModel(GlobalSettingsContainer globalSettingsContainer, [ImportMany] IEnumerable<ISubSettingsControl> subSettings)
        {
            SubSettings = subSettings.OrderBy(a => a.Order).ToList();
        }

        #endregion

        #region Implementation of ICloseable

        public void OnClose()
        {
            foreach (var closableVm in SubSettings.Select(subSettingsControl => subSettingsControl.DataContext as ICloseable))
            {
                closableVm?.OnClose();
            }
        }

        #endregion

        #region Public Properties

        public IEnumerable<ISubSettingsControl> SubSettings { get; }

        #endregion
    }
}
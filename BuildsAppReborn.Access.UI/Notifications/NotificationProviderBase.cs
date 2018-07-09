using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access.UI.Notifications
{
    internal abstract class NotificationProviderBase
    {
        #region Constructors

        protected NotificationProviderBase(GeneralSettings generalSettings)
        {
            this.generalSettings = generalSettings;
        }

        #endregion

        #region Public Methods

        public virtual String GetTitle(IBuild build)
        {
            return build.GenerateTitle(this.generalSettings.ViewStyle);
        }

        #endregion

        #region Private Fields

        private GeneralSettings generalSettings;

        #endregion
    }
}
using Prism.Commands;

namespace BuildsAppReborn.Client.Interfaces
{
    public interface ISaveable
    {
        #region Public Properties

        DelegateCommand SaveCommand { get; }

        #endregion

        #region Public Methods

        void OnSave();

        #endregion
    }
}
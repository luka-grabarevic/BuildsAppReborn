using Prism.Commands;

namespace BuildsAppReborn.Client.Interfaces
{
    public interface ISaveable
    {
        DelegateCommand SaveCommand { get; }

        void OnSave();
    }
}
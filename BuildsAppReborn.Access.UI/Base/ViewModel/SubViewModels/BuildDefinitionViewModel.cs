using System;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Access.UI.ViewModel.SubViewModels
{
    internal class BuildDefinitionViewModel : ViewModelBase
    {
        public BuildDefinitionViewModel(IBuildDefinition buildDefinition)
        {
            if (buildDefinition == null)
            {
                throw new ArgumentNullException(nameof(buildDefinition));
            }

            BuildDefinition = buildDefinition;
        }

        public IBuildDefinition BuildDefinition { get; }

        public Boolean IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                OnPropertyChanged();
            }
        }

        public String Name => BuildDefinition.Name;

        private Boolean isSelected;
    }
}
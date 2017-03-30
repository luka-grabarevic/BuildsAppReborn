using System;

using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Access.UI.ViewModel.SubViewModels
{
    internal class BuildDefinitionViewModel : ViewModelBase
    {
        #region Constructors

        public BuildDefinitionViewModel(IBuildDefinition buildDefinition)
        {
            if (buildDefinition == null) throw new ArgumentNullException(nameof(buildDefinition));
            BuildDefinition = buildDefinition;
        }

        #endregion

        #region Public Properties

        public IBuildDefinition BuildDefinition { get; }

        public Boolean IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                OnPropertyChanged();
            }
        }

        public String Name => BuildDefinition.Name;

        #endregion

        #region Private Fields

        private Boolean isSelected;

        #endregion
    }
}
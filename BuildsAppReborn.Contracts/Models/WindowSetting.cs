using System;
using System.Windows;

namespace BuildsAppReborn.Contracts.Models
{
    public class WindowSetting
    {
        #region Public Properties

        public Double Height { get; set; }

        public String Id { get; set; }

        public Double Left { get; set; }
        public Double Top { get; set; }

        public Double Width { get; set; }

        public WindowState WindowState { get; set; }

        #endregion
    }
}
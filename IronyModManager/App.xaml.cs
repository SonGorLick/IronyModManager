﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 01-10-2020
//
// Last Modified By : Mario
// Last Modified On : 01-20-2020
// ***********************************************************************
// <copyright file="App.xaml.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using IronyModManager.Common;
using IronyModManager.DI;
using IronyModManager.Services.Common;
using IronyModManager.ViewModels;
using IronyModManager.Views;

namespace IronyModManager
{
    /// <summary>
    /// Class App.
    /// Implements the <see cref="Avalonia.Application" />
    /// </summary>
    /// <seealso cref="Avalonia.Application" />
    public class App : Application
    {
        #region Methods

        /// <summary>
        /// Initializes the application by loading XAML etc.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Called when [framework initialization completed].
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                InitCulture();
                InitApp(desktop);
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Initializes the culture.
        /// </summary>
        protected virtual void InitCulture()
        {
            var langService = DIResolver.Get<ILanguagesService>();
            langService.ToggleSelected();
        }

        /// <summary>
        /// Reinitializes the application.
        /// </summary>
        /// <param name="desktop">The desktop.</param>
        private void InitApp(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var resolver = DIResolver.Get<IViewResolver>();
            var mainWindow = DIResolver.Get<MainWindow>();
            var vm = (MainWindowViewModel)resolver.ResolveViewModel<MainWindow>();
            vm.MainWindow = mainWindow;
            mainWindow.DataContext = vm;
            desktop.MainWindow = null;
            desktop.MainWindow = mainWindow;
        }
    }

    #endregion Methods
}

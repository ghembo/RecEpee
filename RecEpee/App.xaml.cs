using RecEpee.DataAccess;
using RecEpee.Framework;
using RecEpee.Models;
using RecEpee.ViewModels;
using RecEpee.Views;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

[assembly: InternalsVisibleTo("RicettarioTest")]
[assembly: CLSCompliant(true)]

namespace RecEpee
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Ioc.RegisterInstance<IDataRepository<Recipe>>(new XmlRecipeRepository());
            Ioc.RegisterInstance<IExporter<Recipe>>(new HtmlRecipeExporter());
            Ioc.RegisterInstance<IDialogService>(new DialogService());

            VvmBinder.RegisterBinding<AboutViewModel, AboutView>();
            VvmBinder.RegisterBinding<AllRecipesViewModel, AllRecipesView>();
            
            MainWindow = VvmBinder.GetView<AllRecipesViewModel>();
            MainWindow.Show();
        }
    }
}

using CoreLibrary;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualStudio.Composition;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VSMEFTestMainApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        //[ImportMany("AnExportWithMetadataTypeValue")]
        //IEnumerable<Lazy<IModule>> ModuleList { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            // Build up a catalog of MEF parts
            MefHosting m = new MefHosting();
            ComposableCatalog catalog = await m.CreateProductCatalogAsync();
            
            // Assemble the parts into a valid graph.
            var config = CompositionConfiguration.Create(catalog);

            // Prepare an ExportProvider factory based on this graph.
            var epf = config.CreateExportProviderFactory();

            // Create an export provider, which represents a unique container of values.
            // You can create as many of these as you want, but typically an app needs just one.
            var exportProvider = epf.CreateExportProvider();
            IEnumerable<Lazy<IModule>> r = exportProvider.GetExports<IModule>();
            //IEnumerable<IModule> r1 = exportProvider.GetExportedValues<IModule>();
            IEnumerable<IModule> r1 = exportProvider.GetExportedValues<IModule>();
            foreach(var rm in r1)
            {
                try
                {
                    if (rm.MainType != null)
                    {
                        var t = Activator.CreateInstance(rm.MainType);

                        //MainFrame.Navigate(rm.MainType);
                        MainFrame.Content = t;
                        break;
                    }
                }
                catch (Exception ex)
                {

                }
                
            }
            // Obtain our first exported value
            //var program = exportProvider.GetExportedValue<Program>();
        }
    }
}

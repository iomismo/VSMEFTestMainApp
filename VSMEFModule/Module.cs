using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Composition;
using CoreLibrary;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VSMEFModule
{
    //[Export("AnExportWithMetadataTypeValue")]
    [Export(typeof(IModule))]
    [ExportMetadata("SomeType", typeof(YetAnotherExport))]
    [ExportMetadata("SomeTypes", typeof(YetAnotherExport))]
    [ExportMetadata("SomeTypes", typeof(string))]
    public class Module : IModule
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Type MainType { get; set; } = typeof(Views.MainPage);
    }
}

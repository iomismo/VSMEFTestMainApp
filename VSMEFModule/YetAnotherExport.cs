using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMEFModule
{
    
    [Export, Export("YetAnotherExport", typeof(object))]
    [ExportMetadata("SomeMetadata", typeof(SomeOtherType))]
    public class YetAnotherExport
    {
    }

    public class SomeOtherType { }
}

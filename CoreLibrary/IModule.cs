using System;

namespace CoreLibrary
{
    public interface IModule
    {
        public string Name { get; set; }
        public Type MainType { get; set; }
    }
}

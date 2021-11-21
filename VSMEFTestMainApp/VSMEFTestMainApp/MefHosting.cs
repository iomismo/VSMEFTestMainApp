using Microsoft.VisualStudio.Composition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VSMEFTestMainApp
{
    internal class MefHosting
    {

        /// <summary>
        /// The MEF discovery module to use (which finds both MEFv1 and MEFv2 parts).
        /// </summary>
        private readonly PartDiscovery discoverer = PartDiscovery.Combine(
            new AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true),
            new AttributedPartDiscoveryV1(Resolver.DefaultInstance));

        /// <summary>
        /// Gets the names of assemblies that belong to the application .exe folder.
        /// </summary>
        /// <returns>A list of assembly names.</returns>
        private static IEnumerable<string> GetAssemblyNames()
        {
            string directoryToSearch = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            foreach (string file in Directory.EnumerateFiles(directoryToSearch, "*.dll"))
            {
                string assemblyFullName = null;
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file);
                    if (assemblyName != null)
                    {
                        assemblyFullName = assemblyName.FullName;
                    }
                }
                catch (Exception)
                {
                }

                if (assemblyFullName != null)
                {
                    yield return assemblyFullName;
                }
            }
        }

        /// <summary>
        /// Creates a catalog with all the assemblies from the application .exe's directory.
        /// </summary>
        /// <returns>A task whose result is the <see cref="ComposableCatalog"/>.</returns>
        public async Task<ComposableCatalog> CreateProductCatalogAsync()
        {
            //var assemblyNames = GetAssemblyNames();
            //var assemblies = assemblyNames.Select(Assembly.Load);
            //var discoveredParts = await this.discoverer.CreatePartsAsync(assemblies);

            List<Assembly> assemblies1 = new List<Assembly>();
            string directoryToSearch = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            foreach (string file in Directory.EnumerateFiles(directoryToSearch, "*.dll"))
            {
                try
                {
                    //assemblies1.Add(Assembly.LoadFrom(file));
                    

                    // Try to load the assembly.
                    Assembly assem = Assembly.LoadFrom(file);
                    assemblies1.Add(assem);
                    Console.WriteLine("File: {0}", file);

                    // Enumerate the resource files.
                    string[] resNames = assem.GetManifestResourceNames();
                    if (resNames.Length == 0)
                        Console.WriteLine("   No resources found.");

                    foreach (var resName in resNames)
                        Console.WriteLine("   Resource: {0}", resName.Replace(".resources", ""));
                }
                catch (Exception)
                {
                }

            }

            var discoveredParts = await this.discoverer.CreatePartsAsync(assemblies1);

            var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
                .AddParts(discoveredParts);
            return catalog;
        }
    }
}

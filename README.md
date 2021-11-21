# VSMEFTestMainApp
WinUI 3 failed to get resources from loaded assemblies when using MEF or Assembly.Load().  
https://github.com/microsoft/microsoft-ui-xaml/issues/5536

**Describe the bug**  
Bunch of problems getting loaded assemblies resources when navigating to xaml pages declared on those assemblies.  

**Steps to reproduce the bug**  
Steps to reproduce the behavior:
1. Create a main "host" winui app
2. Create class library project and create a blank page and some resources for that page
3. Copy compiled class library, pri and xbf files to host app folder
4. Load class library in main app through MEF

**Expected behavior**  
No problem while navigating or creating pages from those loaded assemblies.

**Version Info**  
<!-- Please enter your WinUI NuGet package version, Windows app type (when using WinUI 3+), OS version(s), and form factor(s) -->

NuGet package version: 
[WinUI 3 - Windows App SDK 0.8: 0.8.0]  

**Updated** to Windows App SDK 1.0. Still not getting resources.  
Uploaded minimal app https://github.com/iomismo/VSMEFTestMainApp

<!-- If you are using WinUI 3, please specify for which Windows app type you have encountered the issue. Leave blank if you didn't try that app type. -->
Windows app type:
| UWP              | Win32            |
| :--------------- | :--------------- |
| No | Yes |

<!-- Which Windows versions did you see the issue on? Leave blank if you didn't try that version. -->
| Windows 10 version                  | Saw the problem? |
| :--------------------------------- | :-------------------- |
| Insider Build (xxxxx)              | <!-- Yes/No? -->   |
| October 2020 Update (19042)        | Yes   |
| May 2020 Update (19041)            | Yes   |
| November 2019 Update (18363)       | <!-- Yes/No? -->   |
| May 2019 Update (18362)            | <!-- Yes/No? -->   |
| October 2018 Update (17763)        | <!-- Yes/No? -->   |
| April 2018 Update (17134)          | <!-- Yes/No? -->   |
| Fall Creators Update (16299)       | <!-- Yes/No? -->   |
| Creators Update (15063)            | <!-- Yes/No? -->   |

<!-- Which device form factors did you see the issue on? Leave blank if you didn't try that device. -->
| Device form factor | Saw the problem? |
| :----------------- | :--------------- |
| Desktop            | Yes |
| Xbox               | <!-- Yes/No? --> |
| Surface Hub        | <!-- Yes/No? --> |
| IoT                | <!-- Yes/No? --> |


**Additional context**  
Similar to https://github.com/microsoft/microsoft-ui-xaml/issues/3888 and https://github.com/microsoft/microsoft-ui-xaml/issues/3657  

It is a WinUI module container app which loads different modules through MEF.  
MEF composes well and loads all the dlls stored in a "modules" folder inside the main app folder.  

Pri files of modules are stored along with dlls files in modules folder.  
e.g:  
mainapp/modules/module1.dll  
mainapp/modules/module1.pri  

xbf of modules are stored inside main app folder under its respective Module folder.  
e.g.:  
mainapp/module1/views/page.xbf  

The only way that almost works is in managed only mode and creating instances of that page types through Activator.CreateInstance and assign them to frame.content, but it seems that it (managed) ignores resourcemap not found errors and let you create the page, but without displaying x:uid string resources in the controls, for example. All those string resources are stored in the pri files.  

Navigating to a page type through a frame with managed only, throws 'System.AccessViolationException' Attempted to read or write protected memory. This is often an indication that other memory is corrupt.  

Different behaviors depending on debug mode (sorry for non english words):  

**Managed and native with Activator.CreateInstance()**

```
object cp = Activator.CreateInstance(pageType);
NavigationService.Frame.Content = cp;
```

Callstack
```
 	[External code]	
 	combase.dll!SendReport(HRESULT error, unsigned int cchMax, const wchar_t * message, unsigned short pSid, void * pExceptionObject, IUnknown *) Línea 438	C++
 	combase.dll!RoOriginateLanguageException(HRESULT error, HSTRING__ * message, IUnknown * languageException) Línea 1497	C++
 	Microsoft.ApplicationModel.Resources.dll!winrt::hresult_error::originate(const winrt::hresult code, void * message) Línea 4682	C++
 	Microsoft.ApplicationModel.Resources.dll!winrt::hresult_error::hresult_error(const winrt::hresult code, winrt::take_ownership_from_abi_t __formal) Línea 4604	C++
 	Microsoft.ApplicationModel.Resources.dll!winrt::throw_hresult(const winrt::hresult result) Línea 4871	C++
 	[Marco flotante] Microsoft.ApplicationModel.Resources.dll!winrt::check_hresult(const winrt::hresult) Línea 4916	C++
 	[Marco flotante] Microsoft.ApplicationModel.Resources.dll!winrt::Microsoft::ApplicationModel::Resources::implementation::ResourceMap::GetSubtree(const winrt::hstring &) Línea 37	C++
 	Microsoft.ApplicationModel.Resources.dll!winrt::impl::produce<winrt::Microsoft::ApplicationModel::Resources::implementation::ResourceMap,winrt::Microsoft::ApplicationModel::Resources::IResourceMap>::GetSubtree(void * reference, void * * result) Línea 491	C++
 	[External code]	
>	COMPANY.CORE.MODULES.WINUI.MODULEMANAGER.DLL!Company.Core.Modules.WinUI.ModuleManager.Views.ModulePage.ModulePage() Línea 33	C#
 	[External code]	
 	MainApp.dll!MainApp.ViewModels.ShellViewModel.OnItemInvoked(Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args) Línea 431	C#
 	[External code]	
 	[Marco flotante] hostpolicy.dll!coreclr_t::execute_assembly(int) Línea 89	C++
 	hostpolicy.dll!run_app_for_context(const hostpolicy_context_t & context, int argc, const wchar_t * * argv) Línea 246	C++
 	hostpolicy.dll!run_app(const int argc, const wchar_t * * argv) Línea 275	C++
 	hostpolicy.dll!corehost_main(const int argc, const wchar_t * * argv) Línea 408	C++
 	hostfxr.dll!execute_app(const std::wstring & impl_dll_dir, corehost_init_t * init, const int argc, const wchar_t * * argv) Línea 147	C++
 	hostfxr.dll!`anonymous namespace'::read_config_and_execute(const std::wstring & host_command, const host_startup_info_t & host_info, const std::wstring & app_candidate, const std::unordered_map<enum known_options,std::vector<std::wstring,std::allocator<std::wstring>>,known_options_hash,std::equal_to<enum known_options>,std::allocator<std::pair<enum known_options const ,std::vector<std::wstring,std::allocator<std::wstring>>>>> & opts, int new_argc, const wchar_t * * new_argv, host_mode_t mode, wchar_t * out_buffer, int buffer_size, int * required_buffer_size) Línea 520	C++
 	hostfxr.dll!fx_muxer_t::handle_exec_host_command(const std::wstring & host_command, const host_startup_info_t & host_info, const std::wstring & app_candidate, const std::unordered_map<enum known_options,std::vector<std::wstring,std::allocator<std::wstring>>,known_options_hash,std::equal_to<enum known_options>,std::allocator<std::pair<enum known_options const ,std::vector<std::wstring,std::allocator<std::wstring>>>>> & opts, int argc, const wchar_t * * argv, int argoff, host_mode_t mode, wchar_t * result_buffer, int buffer_size, int * required_buffer_size) Línea 1001	C++
 	hostfxr.dll!fx_muxer_t::execute(const std::wstring host_command, const int argc, const wchar_t * * argv, const host_startup_info_t & host_info, wchar_t * result_buffer, int buffer_size, int * required_buffer_size) Línea 580	C++
 	hostfxr.dll!hostfxr_main_startupinfo(const int argc, const wchar_t * * argv, const wchar_t * host_path, const wchar_t * dotnet_root, const wchar_t * app_path) Línea 61	C++
 	MainApp.exe!exe_start(const int argc, const wchar_t * * argv) Línea 236	C++
 	MainApp.exe!wmain(const int argc, const wchar_t * * argv) Línea 302	C++
 	[External code]	
```

Output
```
D:\a\1\s\dev\MRTCore\mrt\Core\src\MRM.cpp(277)\MRM.dll!0FC2189F: (caller: 0FC23318) ReturnHr(1) tid(54e0) 80073B17  NamedResource not found.
D:\a\1\s\dev\MRTCore\mrt\Core\src\MRM.cpp(404)\MRM.dll!0FC23335: (caller: 0FBCBC79) ReturnHr(2) tid(54e0) 80073B17 NamedResource not found.
D:\a\1\s\dev\MRTCore\mrt\Core\src\MRM.cpp(756)\MRM.dll!0FC23550: (caller: 0FBCBC79) ReturnHr(3) tid(54e0) 80073B17 NamedResource not found.
D:\a\1\s\dev\MRTCore\mrt\Core\src\MRM.cpp(675)\MRM.dll!0FC225AF: (caller: 0FBCA71A) ReturnHr(4) tid(54e0) 80073B1F NamedResource not found.
Excepción producida en 0x761EB4B2 (KernelBase.dll) en MainApp.exe: WinRT originate error - 0x80073B1F : 'ResourceMap not found.'.
```
__________

**Managed and native with Frame.Navigate()**

`Frame.Navigate(pageType`

Callstack
```
 	Microsoft.ui.xaml.dll!DirectUI::ActivationAPI::ActivateInstance()	Desconocido
 	Microsoft.ui.xaml.dll!DirectUI::NavigationCache::GetContent()	Desconocido
 	Microsoft.ui.xaml.dll!DirectUI::Frame::PerformNavigation()	Desconocido
 	Microsoft.ui.xaml.dll!DirectUI::Frame::StartNavigation()	Desconocido
 	Microsoft.ui.xaml.dll!DirectUI::Frame::NavigateWithTransitionInfoImpl()	Desconocido
 	Microsoft.ui.xaml.dll!DirectUI::FrameGenerated::NavigateWithTransitionInfo(struct ABI::Windows::UI::Xaml::Interop::TypeName,struct IInspectable *,struct ABI::Microsoft::UI::Xaml::Media::Animation::INavigationTransitionInfo *,unsigned char *)	Desconocido
 	[External code]	
>	Company.WinUI.dll!Company.WinUI.Services.NavigationService.Navigate(System.Type pageType, object parameter, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo infoOverride) Línea 140	C#
 	MainApp.dll!MainApp.ViewModels.ShellViewModel.OnItemInvoked(Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args) Línea 439	C#
 	[External code]	
 	[Marco flotante] hostpolicy.dll!coreclr_t::execute_assembly(int) Línea 89	C++
 	hostpolicy.dll!run_app_for_context(const hostpolicy_context_t & context, int argc, const wchar_t * * argv) Línea 246	C++
 	hostpolicy.dll!run_app(const int argc, const wchar_t * * argv) Línea 275	C++
 	hostpolicy.dll!corehost_main(const int argc, const wchar_t * * argv) Línea 408	C++
 	hostfxr.dll!execute_app(const std::wstring & impl_dll_dir, corehost_init_t * init, const int argc, const wchar_t * * argv) Línea 147	C++
 	hostfxr.dll!`anonymous namespace'::read_config_and_execute(const std::wstring & host_command, const host_startup_info_t & host_info, const std::wstring & app_candidate, const std::unordered_map<enum known_options,std::vector<std::wstring,std::allocator<std::wstring>>,known_options_hash,std::equal_to<enum known_options>,std::allocator<std::pair<enum known_options const ,std::vector<std::wstring,std::allocator<std::wstring>>>>> & opts, int new_argc, const wchar_t * * new_argv, host_mode_t mode, wchar_t * out_buffer, int buffer_size, int * required_buffer_size) Línea 520	C++
 	hostfxr.dll!fx_muxer_t::handle_exec_host_command(const std::wstring & host_command, const host_startup_info_t & host_info, const std::wstring & app_candidate, const std::unordered_map<enum known_options,std::vector<std::wstring,std::allocator<std::wstring>>,known_options_hash,std::equal_to<enum known_options>,std::allocator<std::pair<enum known_options const ,std::vector<std::wstring,std::allocator<std::wstring>>>>> & opts, int argc, const wchar_t * * argv, int argoff, host_mode_t mode, wchar_t * result_buffer, int buffer_size, int * required_buffer_size) Línea 1001	C++
 	hostfxr.dll!fx_muxer_t::execute(const std::wstring host_command, const int argc, const wchar_t * * argv, const host_startup_info_t & host_info, wchar_t * result_buffer, int buffer_size, int * required_buffer_size) Línea 580	C++
 	hostfxr.dll!hostfxr_main_startupinfo(const int argc, const wchar_t * * argv, const wchar_t * host_path, const wchar_t * dotnet_root, const wchar_t * app_path) Línea 61	C++
 	MainApp.exe!exe_start(const int argc, const wchar_t * * argv) Línea 236	C++
 	MainApp.exe!wmain(const int argc, const wchar_t * * argv) Línea 302	C++
 	[External code]	
```

Output
```
Excepción producida en 0x7A97AF14 (Microsoft.ui.xaml.dll) en MainApp.exe: 0xC0000005: Infracción de acceso al leer la ubicación 0x00000000.
```
__________

**Managed only with Frame.Navigate()**

`Frame.Navigate(pageType`

Callstack
```
 	[External code]	
>	Company.WinUI.dll!Company.WinUI.Services.NavigationService.Navigate(System.Type pageType, object parameter, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo infoOverride) Línea 140	C#
 	MainApp.dll!MainApp.ViewModels.ShellViewModel.OnItemInvoked(Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args) Línea 439	C#
 	[External code]	
```

Output
```
Excepción no controlada del tipo 'System.AccessViolationException' en Microsoft.WinUI.dll
Attempted to read or write protected memory. This is often an indication that other memory is corrupt.
```
__________

Disabling winrt:hresult_error does not work for me.

namedresource uri problem?
This a section of a module pri file. Those namedresources are not being displayed/parsed by xaml page. 
Debug does not provides what namedresource is looking for when throwing the error (in native or mixed mode), any idea how to get it?

```
<ResourceMapSubtree name="Company.Core.Modules.WinUI.ModuleManager">
			<ResourceMapSubtree name="Resources">
				<ResourceMapSubtree name="ModuleInstalled">
					<NamedResource name="Header" uri="ms-resource://Company.Core.Modules.WinUI.ModuleManager/Company.Core.Modules.WinUI.ModuleManager/Resources/ModuleInstalled/Header">
						<Candidate qualifiers="Language-ES-ES" type="String">
							<Value>Installed</Value>
						</Candidate>
						<Candidate qualifiers="Language-EN-US" isDefault="true" type="String">
							<Value>Installed</Value>
						</Candidate>
					</NamedResource>
				</ResourceMapSubtree>
				<ResourceMapSubtree name="ModuleList">
					<NamedResource name="Text" uri="ms-resource://Company.Core.Modules.WinUI.ModuleManager/Company.Core.Modules.WinUI.ModuleManager/Resources/ModuleList/Text">
						<Candidate qualifiers="Language-ES-ES" type="String">
							<Value>Module List</Value>
						</Candidate>
						<Candidate qualifiers="Language-EN-US" isDefault="true" type="String">
							<Value>Module List</Value>
						</Candidate>
					</NamedResource>
				</ResourceMapSubtree>
			</ResourceMapSubtree>
		</ResourceMapSubtree>
```

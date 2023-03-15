using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace PV_Key_Scraper_and_Checker.My
{
	[CompilerGenerated]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed class MySettings : ApplicationSettingsBase
	{
		private static MySettings defaultInstance;

		public static MySettings Default
		{
			get
			{
				return MySettings.defaultInstance;
			}
		}

		static MySettings()
		{
			MySettings.defaultInstance = (MySettings)SettingsBase.Synchronized(new MySettings());
		}

		public MySettings()
		{
		}
	}
}
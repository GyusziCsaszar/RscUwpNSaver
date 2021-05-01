using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;
using Windows.Storage;
using Windows.UI.Popups;

namespace Ressive.WinXMo
{
	
	public static class MessageBox
	{
		
		public static async void Show(string sMsg)
		{
			// SRC: https://stackoverflow.com/questions/46565499/message-box-in-uwp-not-working

            var dialog = new MessageDialog("Hi!");
            await dialog.ShowAsync();
		}
		
	}

	public static class DeviceNetworkInformation
	{

		public static bool IsWiFiEnabled
		{
			get
			{
				var task = IsWifiOn();
				task.Wait();
				return task.Result;
			}
		}

		public static async Task<bool> IsWifiOn()
		{
			try
			{
				await Radio.RequestAccessAsync();

				var radios = await Radio.GetRadiosAsync();
				foreach (var radio in radios)
				{
					if (radio.Kind == RadioKind.WiFi)
					{
						return radio.State == RadioState.On;
					}
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

	}

	public static class Storage
	{

		public static long GetAvailableFreeSpace(string sDrive)
		{
			var task = GetFreeSpace(sDrive);
			task.Wait();
			return task.Result;
		}

		public static async Task<long> GetFreeSpace(string sDrive)
		{

			long lFreeSpace = 0L;

			const String k_freeSpace = "System.FreeSpace";
			const String k_totalSpace = "System.Capacity";

			/*
			DriveInfo[] allDrives = DriveInfo.GetDrives();
			foreach (DriveInfo d in allDrives)
			{
				if (d.Name[0] != sDrive[0])
				{
					continue;
				}
				*/

				try
				{
					StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(sDrive + "\\"); // d.RootDirectory.FullName);
					var props = await folder.Properties.RetrievePropertiesAsync(new string[] { k_freeSpace, k_totalSpace });
					lFreeSpace = (long) (UInt64)props[k_freeSpace];
				}
				catch (Exception)
				{
					//Debug.WriteLine(String.Format("Couldn't get info for drive {0}.  Does it have media in it?", d.Name));
				}

				/*
			}
			*/

			return lFreeSpace;
		}
	}

}
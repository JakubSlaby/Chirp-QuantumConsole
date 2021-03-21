using System.Threading.Tasks;
using QFSW.QC;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC.Commands
{
	public static class DebugConsoleCommands
	{
		private const string s_Lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

		private static string GetRandomLipsum(int length = 0)
		{
			int l = Mathf.Min(length <= 0 ? Random.Range(50, 150) : length, s_Lipsum.Length);
			int i = Random.Range(0, s_Lipsum.Length);
			if (i + l >= s_Lipsum.Length)
				i = s_Lipsum.Length - l;

			return s_Lipsum.Substring(i, l).Trim();
		}
		
		[Command()]
		private static async Task LogRepeated(int count)
		{
			string log = GetRandomLipsum();
			
			while (count-- > 0)
			{
				Debug.Log(log);
				await Task.Delay(100);
			}
			
		}
		[Command()]
		private static async Task LogTimes(int count)
		{
			while (count-- > 0)
			{
				Debug.Log(GetRandomLipsum());
				await Task.Delay(100);
			}
		}
	}
}
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC
{
	public static class ExportPackage
	{
		[MenuItem("Tools/Chirp Logger/Quantum Console/Export Package", priority = 600)]
		public static void ExportChirpPackage()
		{
			ExportPackageFromDirectory("WhiteSparrow/Integrations/QuantumConsole", "Plugins/WhiteSparrow/Integrations/QuantumConsole", $"ChirpQuantumConsole_{ChirpQuantumConsole.Version}");
			ExportPackageFromDirectory("WhiteSparrow/Integrations/QuantumConsoleChirp", "Plugins/WhiteSparrow/Integrations/QuantumConsoleLogger", $"ChirpQuantumConsoleLogger_{ChirpQuantumConsole.Version}");
		}

		private static void ExportPackageFromDirectory(string content, string export, string name)
		{
			var rootDirectory = new DirectoryInfo(QuantumConsoleExtensionDevelopment.RepositoryRoot);
			var contentDirectory = new DirectoryInfo(Path.Combine(rootDirectory.FullName, content));
			var targetDirectory = new DirectoryInfo(Path.Combine(Application.dataPath, export));
			if (targetDirectory.Exists)
			{
				EditorUtility.DisplayDialog("Chirp: Exporting Package",
					$"The target packaging directory is already created - unable to move the files for packaging.\npath: {targetDirectory.FullName}",
					"Ok");
				return;
			}

			if (targetDirectory.Parent != null && !targetDirectory.Parent.Exists)
			{
				Directory.CreateDirectory(targetDirectory.Parent.FullName);
			}
			

			var from = QuantumConsoleExtensionDevelopment.MakePathRelative(contentDirectory.FullName);
			var to = QuantumConsoleExtensionDevelopment.MakePathRelative(targetDirectory.FullName);
			FileUtil.MoveFileOrDirectory(from, to);
			AssetDatabase.Refresh();

			var files = AssetDatabase.FindAssets("*", new[] {to.TrimEnd('/')});
			if (files.Length == 0)
			{
				EditorUtility.DisplayDialog("Chirp: Exporting Package", "Couldn't find any files to export", "Close");
				return;
			}

			var paths = new string[files.Length];
			for (var i = 0; i < files.Length; i++) paths[i] = AssetDatabase.GUIDToAssetPath(files[i]);
			AssetDatabase.ExportPackage(paths, $"{name}.unitypackage", ExportPackageOptions.Recurse);
			FileUtil.MoveFileOrDirectory(to, from);
			AssetDatabase.Refresh();
		}

		
	}
}
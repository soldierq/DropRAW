using System;
using System.Collections.Generic;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace DropRAW
{
    public class Program
    {
        static CommandLineApplication app;
        static CommandOption optionBaseFolder;
        static CommandOption optionTargetFolder;


        public static int Main(string[] args)
        {
            try
            {
                app = new CommandLineApplication();

                app.HelpOption();
                optionBaseFolder = app.Option("-b|--base <BASE_FOLDER>", "The base folder as a reference", CommandOptionType.SingleValue);
                optionTargetFolder = app.Option("-t|--target <TARGET_FOLDER>", "The target folder from where the files will be removed", CommandOptionType.SingleValue);

                Action handler = OnExecuteHandler;
                app.OnExecute(handler);

                return app.Execute(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private static void OnExecuteHandler()
        {
            ConsoleOutput output = new ConsoleOutput();

            try
            {
                string baseFolder = optionBaseFolder.HasValue()
                    ? optionBaseFolder.Value()
                    : string.Empty;

                string targetFolder = optionTargetFolder.HasValue()
                    ? optionTargetFolder.Value()
                    : string.Empty;

                //Exit if missing args
                if (string.IsNullOrEmpty(baseFolder) || string.IsNullOrEmpty(targetFolder))
                {
                    app.ShowHelp();
                    return;
                }

                //Check if folders exist
                if (!Directory.Exists(baseFolder))
                {
                    output.Error(string.Format("Base folder \"{0}\" does not exist!", baseFolder));
                    return;
                }
                if (!Directory.Exists(targetFolder))
                {
                    output.Error(string.Format("Target folder \"{0}\" does not exist!", targetFolder));
                    return;
                }

                //Check how many files needs to be removed
                output.Info("Checking folders...");
                List<string> filesToRemoveList = new List<string>(100);
                string[] baseFiles = Directory.GetFiles(baseFolder);
                string[] targetFiles = Directory.GetFiles(targetFolder);

                //Normailization file names
                Dictionary<string, string> baseFilesList = new Dictionary<string, string>(baseFiles.Length);
                Dictionary<string, string> targetFilesList = new Dictionary<string, string>(targetFiles.Length);

                //Always ignore file extensions when comparing file names
                foreach (string b in baseFiles)
                {
                    baseFilesList.Add(Path.GetFileNameWithoutExtension(b).ToLower(), b);
                }
                foreach (string t in targetFiles)
                {
                    targetFilesList.Add(Path.GetFileNameWithoutExtension(t).ToLower(), t);
                }

                //Comparing base and target folders
                bool found = false;
                foreach (KeyValuePair<string, string> t in targetFilesList)
                {
                    found = false;
                    foreach (KeyValuePair<string, string> b in baseFilesList)
                    {
                        if (t.Key == b.Key)
                        {
                            //Found, keep target file
                            found = true;
                            break;
                        }
                    }

                    //Not found, add file to removing list
                    if (!found)
                    {
                        filesToRemoveList.Add(t.Value);
                    }
                }

                int count = filesToRemoveList.Count;
                //Exit if no file needs to be removed
                if (count == 0)
                {
                    output.Info("No files detected to be removed.");
                    return;
                }

                output.Info(string.Format("{0} files detected to be removed, continue? (Y = Yes, L = List The Files, Any Other Key = Abort)", count.ToString()));
                ConsoleKeyInfo inputKey = Console.ReadKey();
                Console.WriteLine();
                switch (inputKey.Key)
                {
                    case ConsoleKey.L:
                        {
                            foreach (string f in filesToRemoveList)
                            {
                                output.Info(Path.GetFileName(f));
                            }

                            output.Info("Confirm to remove? (Y = Yes, Any Other Key = No)");
                            inputKey = Console.ReadKey();
                            Console.WriteLine();
                            if (inputKey.Key == ConsoleKey.Y) goto case ConsoleKey.Y;
                        }
                        break;
                    case ConsoleKey.Y:
                        {
                            output.Info("Deleting files...");
                            int i = 0;
                            foreach (string f in filesToRemoveList)
                            {
                                File.Delete(f);
                                i++;
                            }
                            output.Info(string.Format("{0} files removed!", i.ToString()));
                        }
                        break;
                    default:
                        {
                            output.Info("User Abort!");
                            return;
                        }
                }
            }
            catch(Exception ex)
            {
                output.Error(ex);
                return;
            }
        }
    }
}

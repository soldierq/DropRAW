using System;
using McMaster.Extensions.CommandLineUtils;
using System.Runtime.InteropServices;

namespace DropRAW
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();

            app.HelpOption();
            var optionSubject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var subject = optionSubject.HasValue()
                    ? optionSubject.Value()
                    : "world";

                Console.WriteLine($"Hello {subject}!");

                Console.ReadLine();

                return 0;
            });


            return app.Execute(args);
        }
    }
}

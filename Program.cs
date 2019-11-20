namespace Rover45
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            ProgramMethods programMethods = new ProgramMethods();

            bool closeSession = false;

            Console.WriteLine(
                "Welcome to the Rover45 Interface!\n" +
                "**************************************************************\n" +
                "Enter 'position' or 'pos' to get the Rover's current position.\n" +
                "Enter 'new' or 'reset' to clear the Rover's current position.\n" +
                "Enter 'X' or 'Exit' at any time to leave the program.\n\n");

            while (!closeSession)
            {
                programMethods.SetOutputMessage();

                string input = Console.ReadLine();

                programMethods.CheckInput(input.Trim());
            }

            Console.WriteLine("The Program has been terminated. Goodbye!");
            return;
        }
    }
}

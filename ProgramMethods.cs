namespace Rover45
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ProgramMethods
    {
        private const string PromptInputZoneSize = "Please enter the zone size:\n";

        private const string PromptInputStartPoint = "Please enter the Rover starting values:\n";

        private const string PromptInputCommands = "Please enter the rover movement commands:\n";

        private const string ErrIncorrectZoneSize = "The provided input is not recognised as a zone boundary.\n" +
                "Please provide a 2-digit numeric value for the zone boundary.\n";

        private const string ErrIncorrectStartArguments = "The value entered is not recognised as a valid starting location.\n" +
                "Please ensure that the location is provided in the format '[Rover starting co-ordinates] [cardinal direction]'.\n";

        private const string ErrStartOutOfBounds = "The given starting point is outside of the zone boundaries.\n" +
                                "Please specify a staring point within the provided zone.\n";

        private const string ErrUnrecognisedStartPoint = "The given starting point is outside of the zone boundaries.\n" +
                                "Please specify a staring point within the provided zone.\n";

        private const string ErrUnrecognisedDirection = "The direction specified is not a recognised cardinal direction.\n" +
                            "Please Provide one of the following cardinal directions:\n" +
                            "\tN - North\n" +
                            "\tE - East\n" +
                            "\tS - South\n" +
                            "\tW - West\n";

        private const string ErrCommandsNotRecognised = "The command list entered is not recognised as valid.\n" +
                    "Please ensure that the command list only contains the following commands with no space between:\n" +
                    "\tM - Move a space forward in the current direction.\n" +
                    "\tR - Rotate 90 degrees to the right\n" +
                    "\tL - Rotate 90 degrees to the left.\n";

        private const string ErrCommandOutOfBounds = "The given commands places the Rover of the zone boundaries.\n" +
                                "Please specify commands within the provided zone.\n";

        private Dictionary<string, decimal> Directions = new Dictionary<string, decimal>()
        {
            { "N", 0 },
            { "E", 0.25m },
            { "S", 0.5m },
            { "W", 0.75m }
        };

        private decimal RotateIncrement = 0.25m;


        public string CurrentBearing;
        public decimal CurrentBearingValue;

        public int XOrdinate;
        public int YOrdinate;
        public int XBoundary;
        public int YBoundary;

        public string ZoneSize;
        public string StartingValues;
        public string RouteCommands;

        public string OutputMessage;

        public ProgramMethods()
        {
            initialise();
        }

        public void SetOutputMessage()
        {
            if (string.IsNullOrEmpty(OutputMessage))
            {
                if (string.IsNullOrEmpty(ZoneSize))
                {
                    OutputMessage = PromptInputZoneSize;
                }
                else if (string.IsNullOrEmpty(StartingValues))
                {
                    OutputMessage = PromptInputStartPoint;
                }
                else if (string.IsNullOrEmpty(RouteCommands))
                {
                    OutputMessage = PromptInputCommands;
                }
            }

            Console.WriteLine(OutputMessage);

            OutputMessage = null;
        }

        public bool CheckInput(string input)
        {
            bool closeSession = false;

            if (!string.IsNullOrEmpty(input))
            {
                if (input.Equals("X", StringComparison.InvariantCultureIgnoreCase) || input.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    closeSession = true;
                }
                else if (input.Equals("new", StringComparison.InvariantCultureIgnoreCase) || input.Equals("reset", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Rover Reset!");
                    initialise();
                }
                else if (input.Equals("position", StringComparison.InvariantCultureIgnoreCase) || input.Equals("pos", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (XOrdinate == 0 && YOrdinate == 0 && string.IsNullOrEmpty(CurrentBearing))
                    {
                        Console.WriteLine("The zone information has not yet been set.\n");
                        OutputMessage = null;
                    }
                    else
                    {
                        Console.WriteLine("The Rover's current Position is:\n");
                        ShowCurrentPosition();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ZoneSize))
                    {
                        ValidateZoneSize(input);
                    }
                    else if (string.IsNullOrEmpty(StartingValues))
                    {
                        ValidateStartingPoint(input);
                    }
                    else if (string.IsNullOrEmpty(RouteCommands))
                    {
                        bool isValid = ValidateCommands(input);

                        if (isValid)
                        {
                            CalculateNewPosition();
                            Console.WriteLine("The Rover's new position is:\n");
                            ShowCurrentPosition();
                        }
                    }
                }
            }

            return closeSession;
        }

        private void ShowCurrentPosition()
        {
            OutputMessage = $"{XOrdinate}{YOrdinate} {CurrentBearing}\n";
        }

        private void initialise()
        {
            CurrentBearing = null;

            XOrdinate = 0;
            YOrdinate = 0;
            XBoundary = 0;
            YBoundary = 0;

            ZoneSize = null;
            StartingValues = null;
            RouteCommands = null;

            OutputMessage = null;
        }

        private bool ValidateZoneSize(string input)
        {
            OutputMessage = null;

            bool validation = false;

            int numericInput;

            if (input.Length == 2 && int.TryParse(input, out numericInput))
            {
                ZoneSize = input;
                XBoundary = int.Parse(ZoneSize[0].ToString());
                YBoundary = int.Parse(ZoneSize[1].ToString());

                validation = true;
            }
            else
            {
                OutputMessage = ErrIncorrectZoneSize;
            }

            return validation;
        }

        private bool ValidateStartingPoint(string input)
        {
            OutputMessage = null;

            bool validation = false;

            string direction = null;
            int xPosition = 0;
            int yPosition = 0;

            string[] inputValues = input.Split(' ', '\t');

            if (inputValues.Length == 2)
            {
                if (inputValues[0].Length == 2 && inputValues[1].Length == 1)
                {
                    if (Directions.Keys.Any(x => x.Equals(inputValues[1])))
                    {
                        validation = true;

                        direction = inputValues[1];
                    }
                    else
                    {
                        validation = false;

                        OutputMessage = ErrUnrecognisedDirection;
                    }

                    int numericInput;

                    if (validation && int.TryParse(inputValues[0], out numericInput))
                    {
                        StartingValues = input;

                        xPosition = int.Parse(inputValues[0][0].ToString());
                        yPosition = int.Parse(inputValues[0][1].ToString());

                        //Check if the starting point is within the zone
                        if (XBoundary < xPosition || YBoundary < yPosition)
                        {
                            validation = false;

                            OutputMessage = ErrStartOutOfBounds;
                        }
                    }
                    else if (validation)
                    {
                        validation = false;

                        OutputMessage = ErrUnrecognisedStartPoint;
                    }
                }
            }
            else
            {
                OutputMessage = ErrIncorrectStartArguments;
            }

            if (validation)
            {
                CurrentBearing = direction;
                CurrentBearingValue = Directions.FirstOrDefault(x => x.Key == CurrentBearing).Value;
                XOrdinate = xPosition;
                YOrdinate = yPosition;
            }

            return validation;
        }

        private bool ValidateCommands(string input)
        {
            OutputMessage = null;

            bool validation = false;

            if (Regex.IsMatch(input, "^[MLR]+$"))
            {
                RouteCommands = input;

                validation = true;
            }
            else
            {
                OutputMessage = ErrCommandsNotRecognised;
            }
            return validation;
        }

        private void CalculateNewPosition()
        {
            string currentDirection = CurrentBearing;
            decimal bearing = CurrentBearingValue;
            int xOrdinate = XOrdinate;
            int yOrdinate = YOrdinate;

            foreach (char command in RouteCommands)
            {
                switch (command)
                { 
                    case 'M':
                        {
                            switch (currentDirection)
                            {
                                case "N":
                                    {
                                        yOrdinate++;
                                        break;
                                    }
                                case "E":
                                    {
                                        xOrdinate++;
                                        break;
                                    }
                                case "W":
                                    {
                                        xOrdinate--;
                                        break;
                                    }
                                case "S":
                                    {
                                        yOrdinate--;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 'L':
                        {
                            bearing -= RotateIncrement;

                            //Get just the value after the decimal
                            bearing -= Math.Floor(bearing);

                            currentDirection = Directions.FirstOrDefault(x => x.Value == bearing).Key;

                            break;
                        }

                    case 'R':
                        {
                            bearing += RotateIncrement;

                            //Get just the value after the decimal
                            bearing -= Math.Floor(bearing);

                            currentDirection = Directions.FirstOrDefault(x => x.Value == bearing).Key;

                            break;
                        }
                }

            }

            //Check that the new location is within bounds
            if (xOrdinate <= XBoundary && yOrdinate <= YBoundary)
            {
                currentDirection = Directions.FirstOrDefault(x => x.Value == bearing).Key;

                XOrdinate = xOrdinate;
                YOrdinate = yOrdinate;

                CurrentBearingValue = bearing;
                CurrentBearing = currentDirection;
            }
            else
            {
                OutputMessage = ErrCommandOutOfBounds;
            }
            
            //Reset the commands so that it's reusable
            RouteCommands = null;
        }
    }
}

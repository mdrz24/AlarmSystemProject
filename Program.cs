using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace talent_show
{

    // **************************************************
    //
    // Title: Finch Talent show
    // Description: Shows the prompt of finch robots actions
    // Application Type: Console
    // Author: Drzewiecki, Marcus
    // Dated Created: 2/18/2020
    // Last Modified: 2/26/2020
    //
    // **************************************************   

}
class Program
{

    /// <summary>
    /// first method run when the app starts up
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        SetTheme();

        DisplayWelcomeScreen();
        DisplayMenuScreen();
        DisplayClosingScreen();
    }

    /// <summary>
    /// setup the console theme
    /// </summary>
    static void SetTheme()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Yellow;
    }

    /// <summary>
    /// *****************************************************************
    /// *                     Main Menu                                 *
    /// *****************************************************************
    /// </summary>
    static void DisplayMenuScreen()
    {
        Console.CursorVisible = true;

        bool quitApplication = false;
        string menuChoice;
        bool connectFinch = false;


        Finch finchRobot = new Finch();

        do
        {
            DisplayScreenHeader("Main Menu");

            //
            // get user menu choice
            //
            Console.WriteLine("\ta) Connect Finch Robot");
            Console.WriteLine("\tb) Talent Show");
            Console.WriteLine("\tc) Data Recorder");
            Console.WriteLine("\td) Alarm System");
            Console.WriteLine("\te) User Programming");
            Console.WriteLine("\tf) Disconnect Finch Robot");
            Console.WriteLine("\tg) Quit");
            Console.Write("\t\tEnter Choice:");
            menuChoice = Console.ReadLine().ToLower();

            //
            // process user menu choice
            //
            switch (menuChoice)
            {
                case "a":
                    connectFinch = DisplayConnectFinchRobot(finchRobot);
                    break;

                case "b":
                    if (connectFinch) DisplayTalentShowMenuScreen(finchRobot);
                    else DisplayMenuScreen();
                    break;

                case "c":
                    if (connectFinch) DisplayDataRecorderScreen(finchRobot);
                    else DisplayMenuScreen();
                    break;

                case "d":
                    if (connectFinch) DisplayAlarmSystemScreen(finchRobot);
                    else DisplayMenuScreen();
                    break;

                case "e":

                    break;

                case "f":
                    DisplayDisconnectFinchRobot(finchRobot);
                    break;

                case "g":
                    DisplayDisconnectFinchRobot(finchRobot);
                    quitApplication = true;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a letter for the menu choice.");
                    DisplayContinuePrompt();
                    break;
            }

        } while (!quitApplication);
    }
    #region Data Recorder
    //
    // data recorder menu
    //
    static void DisplayDataRecorderScreen(Finch finchRobot)
    {
        int numberOfDataPoints = 0;
        double dataPointFrequency = 0;
        double[] temperatures = null;
        Console.CursorVisible = true;

        bool quitTalentShowMenu = false;
        string menuChoice;

        do
        {
            DisplayScreenHeader("Data Recorder Menu");

            //
            // get user menu choice
            //
            Console.WriteLine("\ta) number of data points");
            Console.WriteLine("\tb) frequency of data points");
            Console.WriteLine("\tc) get data");
            Console.WriteLine("\td) Show data ");
            Console.WriteLine("\tg) Main Menu");
            Console.Write("\t\tEnter Choice:");
            menuChoice = Console.ReadLine().ToLower();

            //
            // process user menu choice
            //
            switch (menuChoice)
            {
                case "a":
                    numberOfDataPoints = DisplayNumberOfDataPointsScreen();
                    break;

                case "b":
                    dataPointFrequency = DisplayDataFrequency();
                    break;

                case "c":
                    temperatures = DisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                    break;

                case "d":
                    DisplayShowData(temperatures);
                    break;

                case "g":
                    quitTalentShowMenu = true;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a letter for the menu choice.");
                    DisplayContinuePrompt();
                    break;
            }

        } while (!quitTalentShowMenu);

    }

    static void DisplayShowData(double[] temperatures)
    {
        DisplayScreenHeader("Show Data");

        DisplayTable(temperatures);

        DisplayContinuePrompt();
    }

    static void DisplayTable(double[] temperatures)
    {
        //
        // display table headers
        //
        Console.WriteLine(
            "Recording #".PadLeft(15) +
            "Temperature".PadLeft(15));

        Console.WriteLine(
            "-----------".PadLeft(15) +
            "-----------".PadLeft(15));

        //
        // display table data
        //
        for (int index = 0; index < temperatures.Length; index++)
        {
            Console.WriteLine(
            (index + 1).ToString().PadLeft(15) +
            temperatures[index].ToString("n2").PadLeft(15));
        }
    }
    static double[] DisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
    {
        double[] temperatures = new double[numberOfDataPoints];
        DisplayScreenHeader("Get Data");

        Console.WriteLine($"\tNumber of data points: {numberOfDataPoints}");
        Console.WriteLine($"\tData point frequency: {dataPointFrequency}");
        Console.WriteLine();
        Console.WriteLine("The Finch robot will now record the temperature data.");
        DisplayContinuePrompt();

        for (int index = 0; index < numberOfDataPoints; index++)
        {
            temperatures[index] = finchRobot.getTemperature();
            Console.WriteLine($"\tReading{index + 1}: {temperatures[index].ToString("n2")}");
            int waitInSeconds = (int)(dataPointFrequency * 1000);
            finchRobot.wait(waitInSeconds);
        }

        DisplayContinuePrompt();
        DisplayScreenHeader("Get Data");

        Console.WriteLine();
        Console.WriteLine("\tTempertures Table");
        Console.WriteLine();
        DisplayTable(temperatures);

        DisplayContinuePrompt();

        return temperatures;
    }

    /// <summary>
    /// get the frequency of data points 
    /// </summary>
    /// <returns>frequency of data points</returns>
    static double DisplayDataFrequency()
    {
        double dataPointFrequency;
        string userResponse;

        DisplayScreenHeader("Data Point Frequency");

        Console.Write(" Frequency of Data Points: ");
        userResponse = Console.ReadLine();

        //validate user input
        double.TryParse(userResponse, out dataPointFrequency);

        DisplayContinuePrompt();

        return dataPointFrequency;

    }

    /// <summary>
    /// get data from the user
    /// </summary>
    /// <returns></returns>
    static int DisplayNumberOfDataPointsScreen()
    {
        int numberOfDataPoints;
        string userResponse;

        DisplayScreenHeader("Number of Data Points");

        Console.WriteLine("Number of Data Points");
        userResponse = Console.ReadLine();

        //validate user input
        int.TryParse(userResponse, out numberOfDataPoints);

        DisplayContinuePrompt();

        return numberOfDataPoints;
    }

    #endregion

    #region TALENT SHOW

    /// <summary>
    /// *****************************************************************
    /// *                     Talent Show Menu                          *
    /// *****************************************************************
    /// </summary>
    static void DisplayTalentShowMenuScreen(Finch myFinch)
    {
        Console.CursorVisible = true;

        bool quitTalentShowMenu = false;
        string menuChoice;

        do
        {
            DisplayScreenHeader("Talent Show Menu");

            //
            // get user menu choice
            //
            Console.WriteLine("\ta) Light and Sound");
            Console.WriteLine("\tb)  move around");
            Console.WriteLine("\tc) re-connect robot");
            Console.WriteLine("\td) ");
            Console.WriteLine("\tg) Main Menu");
            Console.Write("\t\tEnter Choice:");
            menuChoice = Console.ReadLine().ToLower();

            //
            // process user menu choice
            //
            switch (menuChoice)
            {
                case "a":
                    DisplayLightAndSound(myFinch);
                    break;

                case "b":
                    DisplayFinchMovement(myFinch);
                    break;

                case "c":
                    DisplayConnectFinchRobot(myFinch);
                    break;

                case "d":

                    break;

                case "g":
                    quitTalentShowMenu = true;
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a letter for the menu choice.");
                    DisplayContinuePrompt();
                    break;
            }

        } while (!quitTalentShowMenu);
    }
    /// <summary>
    /// *****************************************************************
    /// *               Talent Show > movement                  *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>

    static void DisplayFinchMovement(Finch finchRobot)
    {
        string userResponse;
        int leftSpeed;
        int rightSpeed;
        Console.CursorVisible = false;

        DisplayScreenHeader("movement");

        Console.WriteLine("\t how fast do you want the left wheel to go? (please answer from a speed rating of 1-255)");
        Console.WriteLine();
        userResponse = Console.ReadLine();
        leftSpeed = int.Parse(userResponse);

        Console.WriteLine("\t Now lets set the speed for the right wheel?(please answer from a speed rating of 1-255)");
        Console.WriteLine();
        userResponse = Console.ReadLine();
        rightSpeed = int.Parse(userResponse);

        DisplayContinuePrompt();

        int leftMotor = leftSpeed;
        int rightMotor = rightSpeed;
        finchRobot.setMotors(leftMotor, rightMotor);

        Console.WriteLine(" Press any key to disconnet Robot");
        Console.ReadKey();
        finchRobot.disConnect();

        DisplayMenuPrompt("Talent Show Menu");
    }

    /// <summary>
    /// *****************************************************************
    /// *               Talent Show > Light and Sound                   *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    static void DisplayLightAndSound(Finch finchRobot)
    {
        Console.CursorVisible = false;

        DisplayScreenHeader("Light and Sound");

        Console.WriteLine("\tThe Finch robot will now light up your world and sing!");
        DisplayContinuePrompt();

        for (int lightSoundLevel = 60; lightSoundLevel < 500; lightSoundLevel++)
        {
            finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
            finchRobot.noteOn(lightSoundLevel * 200);
        }

        DisplayMenuPrompt("Talent Show Menu");
    }

    #endregion

    #region FINCH ROBOT MANAGEMENT

    /// <summary>
    /// *****************************************************************
    /// *               Disconnect the Finch Robot                      *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    static void DisplayDisconnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        DisplayScreenHeader("Disconnect Finch Robot");

        Console.WriteLine("\tAbout to disconnect from the Finch robot.");
        DisplayContinuePrompt();

        finchRobot.disConnect();

        Console.WriteLine("\tThe Finch robot is now disconnect.");

        DisplayMenuPrompt("Main Menu");
    }

    /// <summary>
    /// *****************************************************************
    /// *                  Connect the Finch Robot                      *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    /// <returns>notify if the robot is connected</returns>
    static bool DisplayConnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        bool robotConnected;

        DisplayScreenHeader("Connect Finch Robot");

        Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
        DisplayContinuePrompt();

        robotConnected = finchRobot.connect();

        // TODO test connection and provide user feedback - text, lights, sounds

        DisplayMenuPrompt("Main Menu");

        //
        // reset finch robot
        //
        finchRobot.setLED(0, 0, 0);
        finchRobot.noteOff();

        return robotConnected;
    }

    #endregion

    #region USER INTERFACE

    /// <summary>
    /// *****************************************************************
    /// *                     Welcome Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayWelcomeScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tFinch Control");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// *****************************************************************
    /// *                     Closing Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayClosingScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tThank you for using Finch Control!");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// display continue prompt
    /// </summary>
    static void DisplayContinuePrompt()
    {
        Console.WriteLine();
        Console.WriteLine("\tPress any key to continue.");
        Console.ReadKey();
    }

    /// <summary>
    /// display menu prompt
    /// </summary>
    static void DisplayMenuPrompt(string menuName)
    {
        Console.WriteLine();
        Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
        Console.ReadKey();
    }

    /// <summary>
    /// display screen header
    /// </summary>
    static void DisplayScreenHeader(string headerText)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\t" + headerText);
        Console.WriteLine();
    }
    #endregion

    #region AlarmSystem
    static void DisplayAlarmSystemScreen(Finch finchRobot)
    {
        string alarmType;
        int maxLimit;
        double maxThreshold;
        double minThreshold;
        bool thresholdExceeded;

        DisplayScreenHeader("Alarm System");

        alarmType = DisplayGetAlarmType();
        maxLimit = DisplayGetMaxLimit();
        DisplayGetThreshhold(finchRobot, alarmType, out minThreshold, out maxThreshold);
        Console.WriteLine();
        Console.WriteLine("The Alarm is now set");
        DisplayContinuePrompt();
        finchRobot.setLED(255, 0, 255);
        thresholdExceeded = MonitorLevels(finchRobot, minThreshold, maxThreshold, maxLimit, alarmType);

        if (thresholdExceeded)
        {
            if (alarmType == "light")
            {
                Console.WriteLine("Maximum or minimum light level exceeded");

            }
            else
            {
                Console.WriteLine("Maximum or minimum temperature level exceeded");
            }

            finchRobot.setLED(255, 0, 0);
            for (int i = 0; i < 5; i++)
            {
                finchRobot.noteOn(10000);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.wait(300);
            }

        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Maximum time exceeded");
        }


        DisplayContinuePrompt();
        finchRobot.noteOff();
        finchRobot.setLED(0, 0, 0);
    }
    static string DisplayGetAlarmType()
    {
        string alarmType;
        Console.WriteLine("Enter Alarm Type [Light or Temperature]:");

        do
        {
            alarmType = Console.ReadLine().ToLower();
            switch (alarmType)
            {
                case "light":
                    Console.WriteLine("Light alarm selected.");
                    break;
                case "temperature":
                    Console.WriteLine("Temperature alarm selected");
                    break;
                default:
                    alarmType = "retry";
                    Console.WriteLine("Please enter light or temperature");
                    break;
            }
        } while (alarmType == "retry");

        DisplayContinuePrompt();
        return alarmType;

    }
    static int DisplayGetMaxLimit()
    {
        int maxLimit;
        bool ifNumber;
        Console.WriteLine("How many seconds do you want the alarm to last?");
        do
        {
            ifNumber = int.TryParse(Console.ReadLine(), out maxLimit);

            if (!ifNumber)
            {
                Console.WriteLine();
                Console.Write("Please enter a number:");
            }

        } while (!ifNumber);

        DisplayContinuePrompt();
        return maxLimit;
    }
    static void DisplayGetThreshhold(Finch finchRobot, string alarmType, out double minThreshold, out double maxThreshold)
    {
        bool ifNumber;
        minThreshold = 0;
        maxThreshold = 0;
        DisplayScreenHeader("Threshold Value");


        switch (alarmType)
        {
            case "light":
                Console.Write($"Current Light Level : {finchRobot.getLeftLightSensor()}");
                Console.WriteLine();
                Console.WriteLine("Enter maximum light level[0-255]");
                do
                {
                    ifNumber = double.TryParse(Console.ReadLine(), out maxThreshold);
                    if (!ifNumber)
                    {
                        Console.WriteLine();
                        Console.Write("Please enter a number:");
                    }
                    else if (maxThreshold > 255)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Maximum light level can't be higher than 255, please enter a valid number:");
                        ifNumber = false;
                    }
                    else if (maxThreshold < finchRobot.getLeftLightSensor())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Maximum light level cannot be below ambient, please enter a valid number:");
                        ifNumber = false;
                    }

                } while (!ifNumber);

                Console.WriteLine();
                Console.WriteLine("Enter minimum light level[0-255]");
                do
                {
                    ifNumber = double.TryParse(Console.ReadLine(), out minThreshold);
                    if (!ifNumber)
                    {
                        Console.WriteLine();
                        Console.Write("Please enter a number:");
                    }
                    else if (minThreshold < 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Minimum light level can't be less than 0, please enter a valid number:");
                        ifNumber = false;
                    }
                    else if (minThreshold > finchRobot.getLeftLightSensor())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Minimum light level cannot be above ambient, please enter a valid number:");
                        ifNumber = false;
                    }

                } while (!ifNumber);

                break;

            case "temperature":
                Console.Write($"Current Temperature : {(finchRobot.getTemperature())}");
                Console.WriteLine();
                Console.WriteLine("Enter maximum temperature in degrees Celcius");
                do
                {
                    ifNumber = double.TryParse(Console.ReadLine(), out maxThreshold);
                    if (!ifNumber)
                    {
                        Console.WriteLine();
                        Console.Write("Please enter a number:");
                    }
                    else if (maxThreshold < finchRobot.getLeftLightSensor())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Maximum temperature cannot be below ambient, please enter a valid number:");
                        ifNumber = false;
                    }

                } while (!ifNumber);

                Console.WriteLine();
                Console.WriteLine("Enter minimum temperature in degrees Celcius");
                do
                {
                    ifNumber = double.TryParse(Console.ReadLine(), out minThreshold);
                    if (!ifNumber)
                    {
                        Console.WriteLine();
                        Console.Write("Please enter a number:");
                    }
                    else if (minThreshold > finchRobot.getLeftLightSensor())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Minimum temperature cannot be above ambient, please enter a valid number:");
                        ifNumber = false;
                    }

                } while (!ifNumber);

                break;


            default:
                throw new FormatException();
                //break;
        }

        DisplayContinuePrompt();
    }
    static bool MonitorLevels(Finch finchRobot, double minThreshold, double maxThreshold, int maxLimit, string alarmType)
    {
        bool thresholdExceeded = false;
        int currentLevel;
        double currentTemp;
        double seconds = 0;
        switch (alarmType)
        {
            case "light":
                while (!thresholdExceeded && seconds <= maxLimit)
                {
                    currentLevel = finchRobot.getLeftLightSensor();

                    DisplayScreenHeader("Monitoring Light Levels...");
                    Console.WriteLine();
                    Console.WriteLine($"Maximum light level: {maxThreshold}");
                    Console.WriteLine($"Minimum light level: {minThreshold}");
                    Console.WriteLine($"Current light level: {currentLevel}");

                    if (currentLevel > maxThreshold)
                    {
                        thresholdExceeded = true;
                    }
                    else if (currentLevel < minThreshold)
                    {
                        thresholdExceeded = true;
                    }

                    finchRobot.wait(100);
                    seconds += 2.5;
                }
                break;

            case "temperature":
                while (!thresholdExceeded && seconds <= maxLimit)
                {
                    currentTemp = finchRobot.getTemperature();

                    DisplayScreenHeader("Monitoring Temperature Levels...");
                    Console.WriteLine();
                    Console.WriteLine($"Maximum temperature: {(maxThreshold)}");
                    Console.WriteLine($"Minimum light level: {(minThreshold)}");
                    Console.WriteLine($"Current light level: {(currentTemp)}");
                    if (currentTemp > maxThreshold)
                    {
                        thresholdExceeded = true;
                    }
                    else if (currentTemp < minThreshold)
                    {
                        thresholdExceeded = true;
                    }

                    finchRobot.wait(1000);
                    seconds += 1.0;
                }
                break;

            default:
                break;
        }

        return thresholdExceeded;
    }


    #endregion
}

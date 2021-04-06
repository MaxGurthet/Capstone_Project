using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Capstone_Project
{
    // **************************************************
    // Title: Capstone Project (RPG Character Creator)
    // Description: A program that will allow the user to
    //              login and create a character for a
    //              RPG game such as D&D.
    // Application Type: Console
    // Author: Gurthet, Max
    // Dated Created: 4/6/2021
    // Last Modified: 4/6/2021
    // **************************************************
    class Program
    {
        /// <summary>
        /// runs operations
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();
            DisplayWelcomeScreen();
            DisplayLoginRegisterMenu();
            ThemeMenuOperation();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        #region USER INTERFACE

        /// <summary>
        /// welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tWelcome to the RPG Character Creator Program!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using the RPG Character Creator!");
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
            Console.WriteLine($"\tPress any key to return to the {menuName}.");
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

        #region LOGIN/REGISTER INTERFACE

        /// <summary>
        /// dispay the login/register menu
        /// </summary>
        static void DisplayLoginRegisterMenu()
        {
            DisplayScreenHeader("Login/Register");

            Console.Write("\tAre you currently a registered user, yes or no?: ");
            if (Console.ReadLine().ToLower() == "yes")
            {
                DisplayLoginScreen();
                Console.WriteLine("\tYou will now be redirected to the theme select screen.");
                DisplayContinuePrompt();
            }
            else
            {
                DisplayRegisterScreen();
                DisplayLoginScreen();
                Console.WriteLine("\tYou will now be redirected to the theme select screen.");
                DisplayContinuePrompt();
            }
        }

        /// <summary>
        /// user login screen
        /// </summary>
        static void DisplayLoginScreen()
        {
            string userName;
            string password;
            bool validAns;

            do
            {
                DisplayScreenHeader("User Login");

                Console.WriteLine();
                Console.Write("\tPlease enter your username: ");
                userName = Console.ReadLine();
                Console.Write("\tPlease enter your password: ");
                password = Console.ReadLine();

                validAns = ValidLogin(userName, password);

                Console.WriteLine();
                if (validAns)
                {
                    Console.WriteLine("\tYou have successfully logged in.");
                }
                else
                {
                    Console.WriteLine("\tLogin failed, please check username and password and try again.");
                }
            } while (!validAns);
        }

        /// <summary>
        /// validates the user's login information
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static bool ValidLogin(string userName, string password)
        {
            (string userName, string password) userInfo;
            bool validAns;

            userInfo = ReadLoginInfo();

            validAns = (userInfo.userName == userName) && (userInfo.password == password);

            return validAns;
        }

        /// <summary>
        /// reads the user's login info from a data file
        /// </summary>
        /// <returns></returns>
        static (string userName, string password) ReadLoginInfo()
        {
            string dataPath = @"Data/Logins.txt";

            string loginInfoText;
            string[] loginInfoArray;
            (string userName, string password) loginInfoTuple;

            loginInfoText = File.ReadAllText(dataPath);
            loginInfoArray = loginInfoText.Split(',');
            loginInfoTuple.userName = loginInfoArray[0];
            loginInfoTuple.password = loginInfoArray[1];

            return loginInfoTuple;
        }

        /// <summary>
        /// writes login info to a data file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        static void WriteLoginInfo(string userName, string password)
        {
            string dataPath = @"Data/Logins.txt";
            string loginInfoText;

            loginInfoText = userName + "," + password;

            File.WriteAllText(dataPath, loginInfoText);
        }

        /// <summary>
        /// user register screen
        /// </summary>
        static void DisplayRegisterScreen()
        {
            string userName;
            string password;

            DisplayScreenHeader("Register User");

            Console.Write("\tPlease enter your desired username: ");
            userName = Console.ReadLine();
            Console.Write("\tPlease enter your desired password: ");
            password = Console.ReadLine();

            WriteLoginInfo(userName, password);

            Console.WriteLine();
            Console.WriteLine("\tThe following information you have entered will be saved.");
            Console.WriteLine($"\tUsername: {userName}");
            Console.WriteLine($"\tPassword: {password}");

            DisplayContinuePrompt();
        }

        #endregion

        #region THEME SELECT INTERFACE

        /// <summary>
        /// code that goes into the main
        /// </summary>
        static void ThemeMenuOperation()
        {
            DisplayReadAndSetTheme();
            DisplaySetNewTheme();
        }
        /// <summary>
        /// read and set the theme screen
        /// </summary>
        static void DisplayReadAndSetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            string fileIOStatusMessage;

            themeColors = ReadThemeDataExceptions(out fileIOStatusMessage);
            if (fileIOStatusMessage == "Complete")
            {
                Console.ForegroundColor = themeColors.foregroundColor;
                Console.BackgroundColor = themeColors.backgroundColor;
                Console.Clear();

                DisplayScreenHeader("Read Theme from Data File");
                Console.WriteLine("\n\tTheme read from data file.\n");
            }
            else
            {
                DisplayScreenHeader("Read Theme from Data File");
                Console.WriteLine("\n\tTheme not read from data file.");
                Console.WriteLine($"\t*** {fileIOStatusMessage} ***\n");
            }
            DisplayContinuePrompt();
        }

        /// <summary>
        /// prompts the user to either select new theme or continue with default
        /// </summary>
        static void DisplaySetNewTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;
            string fileIOStatusMessage;

            DisplayScreenHeader("Theme Select Menu");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme, yes or no?: ");
            if (Console.ReadLine().ToLower() == "no")
            {
                do
                {
                    themeColors.foregroundColor = UserConsoleColor("foreground");
                    themeColors.backgroundColor = UserConsoleColor("background");

                    //
                    // set new theme
                    //
                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?: ");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        fileIOStatusMessage = WriteThemeExceptions(themeColors.foregroundColor, themeColors.backgroundColor);
                        if (fileIOStatusMessage == "Complete")
                        {
                            Console.WriteLine("\tNew theme written to data file.");
                        }
                        else
                        {
                            Console.WriteLine("\tNew theme not written to data file.");
                            Console.WriteLine($"\t{fileIOStatusMessage}");
                        }
                    }

                } while (!themeChosen);
            }
            DisplayContinuePrompt();
        }

        /// <summary>
        /// get a console color from the user
        /// </summary>
        /// <param name="property">foreground or background</param>
        /// <returns>user's console color</returns>
        static ConsoleColor UserConsoleColor(string property)
        {
            ConsoleColor consoleColor;
            bool validAns;

            do
            {
                Console.Write($"\tEnter a value for the {property}: ");
                validAns = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validAns)
                {
                    Console.WriteLine("\tYou have entered an invalid option. Please try again.");
                }
                else
                {
                    validAns = true;
                }
            } while (!validAns);

            return consoleColor;
        }

        /// <summary>
        /// read the theme exceptions
        /// </summary>
        /// <returns>tuple of foreground and background</returns>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeDataExceptions(out string fileIOStatusMessage)
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor = ConsoleColor.White;
            ConsoleColor backgroundColor = ConsoleColor.Black;

            try
            {
                themeColors = File.ReadAllLines(dataPath);
                if (Enum.TryParse(themeColors[0], true, out foregroundColor) &&
                    Enum.TryParse(themeColors[1], true, out backgroundColor))
                {
                    fileIOStatusMessage = "Complete";
                }
                else
                {
                    fileIOStatusMessage = "Data file incorrectly formated.";
                }
            }
            catch (DirectoryNotFoundException)
            {
                fileIOStatusMessage = "Unable to locate the folder for the data file.";
            }
            catch (Exception)
            {
                fileIOStatusMessage = "Unable to read data file.";
            }

            return (foregroundColor, backgroundColor);
        }

        /// <summary>
        /// write theme info to a data file
        /// </summary>
        /// <returns>tuple of foreground and background</returns>
        static string WriteThemeExceptions(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";
            string fileIOStatusMessage = "";

            try
            {
                File.WriteAllText(dataPath, foreground.ToString() + "\n");
                File.AppendAllText(dataPath, background.ToString());
                fileIOStatusMessage = "Complete";
            }
            catch (DirectoryNotFoundException)
            {
                fileIOStatusMessage = "Unable to locate the folder for the data file.";
            }
            catch (Exception)
            {
                fileIOStatusMessage = "Unable to write to data file.";
            }

            return fileIOStatusMessage;
        }
        #endregion
    }
}

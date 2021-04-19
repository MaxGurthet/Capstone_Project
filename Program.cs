using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Capstone_Project
{
    // ********************************************************
    // Title: Capstone Project (RPG Character Creator)
    // Description: A program that will allow one user to login
    //              and create/view a character for an RPG game
    //              such as D&D. Only basic stats will be
    //              available as of now, meaning bonuses given
    //              for class and race will not be applied to
    //              overall stats.
    // Application Type: Console
    // Author: Gurthet, Max
    // Dated Created: 4/6/2021
    // Last Modified: 4/8/2021
    // ********************************************************
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
            DisplayReadAndSetTheme();
            DisplaySetNewTheme();
            DisplayMenuScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// main menu screen
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;
            string charName = null;
            string charRace = null;
            string charClass = null;
            int hitPoints = 0;
            int str = 0;
            int dex = 0;
            int con = 0;
            int intell = 0;
            int wis = 0;
            int charis = 0;

            do
            {
                DisplayScreenHeader("Main Menu");

                Console.WriteLine("\ta) Character Name");
                Console.WriteLine("\tb) Character Race");
                Console.WriteLine("\tc) Character Class");
                Console.WriteLine("\td) Ability Scores");
                Console.WriteLine("\te) Hit Points");
                Console.WriteLine("\tf) Display Character");
                Console.WriteLine("\tq) Quit Application");
                Console.Write("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "a":
                        charName = GetCharacterName();
                        break;

                    case "b":
                        charRace = CharacterRace();
                        break;

                    case "c":
                        charClass = CharacterClass();
                        break;

                    case "d":
                        str = StrengthRoll();
                        dex = DexterityRoll();
                        con = ConstitutionRoll();
                        intell = IntelligenceRoll();
                        wis = WisdomRoll();
                        charis = CharismaRoll();

                        break;

                    case "e":
                        hitPoints = HitPoints();
                        break;

                    case "f":
                        DisplayCharacterAll(charName, charRace, charClass, hitPoints, str, dex, con, intell, wis, charis);
                        break;

                    case "q":
                        QuitApp();
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

        /// <summary>
        /// quits the app
        /// </summary>
        static void QuitApp()
        {
            Console.CursorVisible = false;

            Console.Clear();
            DisplayScreenHeader("Quit Application");

            Console.WriteLine("\tYou are about to exit the program.");
            DisplayContinuePrompt();

            DisplayClosingScreen();
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
            List<(string userName, string password)> registeredUserInfo = new List<(string userName, string password)>();
            bool validAns = false;

            registeredUserInfo = ReadLoginInfo();

            foreach ((string userName, string password) userLoginInfo in registeredUserInfo)
            {
                if ((userLoginInfo.userName == userName) && (userLoginInfo.password == password))
                {
                    validAns = true;
                    break;
                }
            }
            return validAns;
        }

        /// <summary>
        /// reads the user's login info from a data file
        /// </summary>
        /// <returns></returns>
        static List<(string userName, string password)> ReadLoginInfo()
        {
            string dataPath = @"Data/Logins.txt";

            string[] loginInfoArray;
            (string userName, string password) loginInfoTuple;

            List<(string userName, string password)> registeredUserInfo = new List<(string userName, string password)>();

            loginInfoArray = File.ReadAllLines(dataPath);

            foreach (string loginInfoText in loginInfoArray)
            {
                loginInfoArray = loginInfoText.Split(',');

                loginInfoTuple.userName = loginInfoArray[0];
                loginInfoTuple.password = loginInfoArray[1];

                registeredUserInfo.Add(loginInfoTuple);
            }

            return registeredUserInfo;
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

        #region THEME SELECT
        /// <summary>
        /// read the theme data already entered in data file
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
        /// set the theme screen
        /// </summary>
        static void DisplaySetNewTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;
            string fileIOStatusMessage;

            DisplayScreenHeader("Set New Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme yes or no?: ");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set New Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?: ");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        fileIOStatusMessage = WriteThemeDataExceptions(themeColors.foregroundColor, themeColors.backgroundColor);
                        if (fileIOStatusMessage == "Complete")
                        {
                            Console.WriteLine("\n\tNew theme written to data file.\n");
                        }
                        else
                        {
                            Console.WriteLine("\n\tNew theme not written to data file.");
                            Console.WriteLine($"\t*** {fileIOStatusMessage} ***\n");
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
        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}: ");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid color. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        /// <summary>
        /// read theme info to data file with try/catch block
        /// returning a file IO status message using an out parameter
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
        /// write theme info to data file with try/catch block
        /// returning a file IO status message using an out parameter
        /// </summary>
        /// <returns>tuple of foreground and background</returns>
        static string WriteThemeDataExceptions(ConsoleColor foreground, ConsoleColor background)
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

        #region CHARACTER NAME
        /// <summary>
        /// asks the user to enter their character's name
        /// </summary>
        static string GetCharacterName()
        {
            DisplayScreenHeader("Character Name");

            Console.Write("\tPlease enter the desired name for your character: ");
            string charName = Console.ReadLine();

            return charName;
        }

        #endregion

        #region CHARACTER RACE
        /// <summary>
        /// prompt the user to enter their character's race
        /// </summary>
        static string CharacterRace()
        {
            DisplayScreenHeader("Character Race");

            Console.WriteLine("\t**** PLEASE NOTE THAT BONUSES FOR RACE WILL NOT BE APPLIED TO THE OVERALL STATS ****");
            Console.WriteLine();
            Console.Write("\tPlease enter the desired race for your character: ");
            string charRace = Console.ReadLine();

            return charRace;
        }

        #endregion

        #region CHARACTER CLASS
        /// <summary>
        /// prompts the user to enter their character's class
        /// </summary>
        /// <returns></returns>
        static string CharacterClass()
        {
            DisplayScreenHeader("Character Name");

            Console.Write("\tPlease enter the desired class for your character: ");
            string charClass = Console.ReadLine();

            return charClass;
        }

        #endregion

        #region ABILITY SCORES
        /// <summary>
        /// prompts the user to enter their strength roll
        /// </summary>
        /// <returns></returns>
        static int StrengthRoll()
        {
            int str;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Strength Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out str))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return str;
        }

        /// <summary>
        /// prompts the user to enter their dexterity roll
        /// </summary>
        /// <returns></returns>
        static int DexterityRoll()
        {
            int dex;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Dexterity Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out dex))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return dex;
        }

        /// <summary>
        /// prompts the user to enter their constitution roll
        /// </summary>
        /// <returns></returns>
        static int ConstitutionRoll()
        {
            int con;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Constitution Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out con))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return con;
        }

        /// <summary>
        /// prompts the user to enter their intelligence roll
        /// </summary>
        /// <returns></returns>
        static int IntelligenceRoll()
        {
            int intell;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Intelligence Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out intell))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return intell;
        }

        /// <summary>
        /// prompts the user to enter their charisma roll
        /// </summary>
        /// <returns></returns>
        static int CharismaRoll()
        {
            int charis;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Charisma Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out charis))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return charis;
        }

        /// <summary>
        /// prompts the user to enter their wisdom roll
        /// </summary>
        /// <returns></returns>
        static int WisdomRoll()
        {
            int wis;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Wisdom Roll");

                Console.WriteLine("\tUsing a d20, roll five times and choose the highest number.");
                Console.WriteLine("\tIf you do not have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the highest number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out wis))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return wis;
        }

        #endregion

        #region HIT POINTS

        /// <summary>
        /// prompts the user to roll for their hit points
        /// </summary>
        /// <returns></returns>
        static int HitPoints()
        {
            int hitPoints;
            string userResponse;
            bool validResponse;

            do
            {
                DisplayScreenHeader("Hit Points");
                Console.WriteLine("\t****PLEASE NOTE THAT CONSTITUION MODIFIERS WILL NOT BE ADDED TO HIT POINTS****");
                Console.WriteLine();
                Console.WriteLine("\tEach class has different reuqirements for hit dice. Below is a list for the different hit dice each class uses.");
                Console.WriteLine();
                Console.WriteLine("\td6: sorcerer, wizard");
                Console.WriteLine("\td8: artificer, bard, cleric, druid, monk, rogue, warlock.");
                Console.WriteLine("\td10: fighter, paladin, ranger");
                Console.WriteLine("\td12: barbarian");
                Console.WriteLine();
                Console.WriteLine("\tUsing the appropriate dice, roll for hit points and add your consitution modifier.");
                Console.WriteLine("\tIf you don't have dice available, search dice roller in google.");
                Console.WriteLine();
                Console.Write("\tPlease enter the number you rolled: ");
                userResponse = Console.ReadLine();

                if (!int.TryParse(userResponse, out hitPoints))
                {
                    validResponse = false;
                    Console.WriteLine("It appears you have entered an invalid input. Please enter a valid number.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    validResponse = true;
                }

            } while (!validResponse);

            return hitPoints;
        }

        #endregion

        #region DISPLAY CHARACTER
        /// <summary>
        /// dsiplays all the entered values from all previous functions
        /// </summary>
        static void DisplayCharacterAll(string charName, string charRace, string charClass, int hitPoints, int str, int dex, int con, int intell, int wis, int charis)
        {
            DisplayScreenHeader("Display Character");

            Console.WriteLine("\t***********************");
            Console.WriteLine($"\tName: {charName}");
            Console.WriteLine();
            Console.WriteLine($"\tRace: {charRace}");
            Console.WriteLine();
            Console.WriteLine($"\tClass: {charClass}");
            Console.WriteLine();
            Console.WriteLine($"\tHit Points: {hitPoints}");
            Console.WriteLine();
            Console.WriteLine($"\tStrength: {str}");
            Console.WriteLine();
            Console.WriteLine($"\tDexterity: {dex}");
            Console.WriteLine();
            Console.WriteLine($"\tConstitution: {con}");
            Console.WriteLine();
            Console.WriteLine($"\tIntelligence: {intell}");
            Console.WriteLine();
            Console.WriteLine($"\tWisdom: {wis}");
            Console.WriteLine();
            Console.WriteLine($"\tCharisma: {charis}");
            Console.WriteLine("\t***********************");
            Console.WriteLine();

            DisplayMenuPrompt("Main Menu");
        }
        #endregion
    }
}

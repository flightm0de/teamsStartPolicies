using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace teamsStartPolicies_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("Run", true);
            key.CreateSubKey("Run");
            //Console.WriteLine(key.GetValue("com.squirrel.Teams.Teams").ToString());
            key.SetValue("com.squirrel.Teams.Teams", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Microsoft\\Teams\\Update.exe --processStart \"Teams.exe\" --process-start-args \"--system-initiated\"");
            key.Close();
            string pathOld = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config.json";
            string pathNew = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config_new.json";
            string path = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config_old.json";

            string pathOldSpecial = "C:\\Users\\" + Environment.UserName + "." + Environment.UserDomainName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config.json";
            string pathNewSpecial = "C:\\Users\\" + Environment.UserName + "." + Environment.UserDomainName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config_new.json";
            string pathSpecial = "C:\\Users\\" + Environment.UserName + "." + Environment.UserDomainName + "\\AppData\\Roaming\\Microsoft\\Teams\\desktop-config_old.json";

            string text = "undefined";

            try
            {
                text = File.ReadAllText(pathOld);
                File.WriteAllText(pathNew, "");
            }
            catch
            {
                pathOld = pathOldSpecial;
                pathNew = pathNewSpecial;
                path = pathSpecial;
                text = File.ReadAllText(pathOld);
                File.WriteAllText(pathNew, "");
            }
            string currVar = "";
            string currVarVal = "";
            bool doppelpunktFound = false;
            bool falseInserted = false;

            foreach (char c in text)
            {
                if (c != '{' && c != '}' && c != '"' && c != ':' && c != ',')
                {
                    if (doppelpunktFound) currVarVal += c;
                    else currVar += c;
                }
                else if (c == ':')
                {
                    doppelpunktFound = true;
                    if (!falseInserted)
                    {
                        if (currVar == "openAtLogin")
                        {
                            File.AppendAllText(pathNew, ":true");
                            falseInserted = true;
                        }
                        else if (currVar == "openAsHidden")
                        {
                            File.AppendAllText(pathNew, ":true");
                            falseInserted = true;
                        }
                        else if (currVar == "runningOnClose")
                        {
                            File.AppendAllText(pathNew, ":true");
                            falseInserted = true;
                        }
                    }
                }
                else if (c == ',' || c == '{')
                {
                    //Console.WriteLine(currVar + ": " + currVarVal);
                    currVar = "";
                    currVarVal = "";
                    doppelpunktFound = false;
                    falseInserted = false;
                }

                if (!falseInserted)
                {
                    File.AppendAllText(pathNew, c.ToString());
                }
            }

            File.Delete(path);
            File.Move(pathOld, path);
            File.Move(pathNew, pathOld);

            //Console.WriteLine(path);
            //Console.WriteLine(pathOld);
            //Console.WriteLine(pathNew);

            //Console.ReadLine();
        }
    }
}

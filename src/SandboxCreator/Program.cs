using System;
using Neo4j.Driver.V1;

namespace SandboxCreator
{
    class Program
    {
        static ColoredConsoleWriter log = new ColoredConsoleWriter();
        static string _action;
        static string _connectionString;
        static string _driverUri;
        static string _userId;
        static string _password; 

        static void Main(string[] args)
        {
            EvaluateParams(args);
            ExecuteAction();
        }

        private static void ExecuteAction()
        {
            if (_action.IsNullOrEmpty() || _action.StartsWith("h"))
            {
                PrintHelpScreen();
            } else 
            if (_action.StartsWith("c"))
            {
                log.WriteInfo("Creating test data.");
                CreateSandBox();
            }
            else
            {
                log.WriteError($"'{_action}' is an unknown command.");
            }
        }

        private static void PrintHelpScreen()
        {
            log.WriteInfo("Usage:");
            log.WriteInfo("\tdotnet SandboxCreator.dll h[elp]");
            log.WriteInfo("\t\tShows this screen.");
            log.WriteInfo("\tdotnet SandboxCreator.dll c[reate] -driveruri:[driveruri] -connectionstring:[connectionstring] -userid:[userid] -password:[password]");   
            log.WriteInfo("\t\tPopulates the db with random data.");
        }

        private static void CreateSandBox()
        {
            if (ValidateParams())
            {
                try
                {
                    var client = new Neo4JClient(_driverUri, _connectionString, _userId, _password);
                    //client.CreateSandBox();
                }
                catch (Exception ex)
                {

                    log.WriteError(ex.ToString());
                }                
            }
        }

        private static bool ValidateParams()
        {
            try
            {
                _connectionString = StripAttribute("connectionstring", _connectionString);
                _driverUri = StripAttribute("driveruri", _driverUri);
                _userId = StripAttribute("userid", _userId);
                _password = StripAttribute("password", _password);
                return true;
            }
            catch (Exception e)
            {
                log.WriteError(e.ToString());             
            }
            return false;
            
        }

        private static string StripAttribute(string attributeName, string param)
        {
            if (!param.StartsWith("-" + attributeName) || !param.Contains(":"))
                throw new ArgumentException(attributeName);
            var value = param.Substring(param.IndexOf(':')+1).Trim();
            //Debug
            log.WriteInfo($"{attributeName}={value}");
            if (value.IsNullOrEmpty())
                throw new ArgumentOutOfRangeException(attributeName);
            return value;            
        }

        private static void EvaluateParams(string[] args)
        {
            _action = args.Length>0? args[0].ToLower():"";            
            _driverUri = args.Length > 1 ? args[1].ToLower() : "";
            _connectionString = args.Length > 2 ? args[2].ToLower() : "";
            _userId = args.Length > 3 ? args[3].ToLower() : "";
            _password = args.Length > 4 ? args[4].ToLower() : "";
        }
    }
}

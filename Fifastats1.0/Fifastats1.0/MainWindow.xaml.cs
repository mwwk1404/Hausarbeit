using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fifastats1._0
{
    class Program
    {
        public static bool DEBUG { get; private set; } = true;

        private static int numberOfIterations = 10000;

        public class SaltAndHash
        {
            public string salt { get; set; }

            public string hash { get; set; }

            public SaltAndHash(string salt, string hash)
            {
                this.salt = salt;
                this.hash = hash;
            }
        }

        // Überprüfen eines Passworts
        public static bool CheckPassword(int userID, string password)
        {
            // Salt-Wert aus Datenbank auslesen
            string salt = getSaltFromDB(userID);

            // Umwandeln des Salt in byte-Array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Bestimmen des Passwort-Hash-Wert für eingegebenes Passwort
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes);
            // Werte müssen identisch zu den Werten beim Generieren des Passwortes sein
            rfc2898DeriveBytes.IterationCount = numberOfIterations;
            byte[] enteredHash = rfc2898DeriveBytes.GetBytes(20);
            // Umwandeln von byte-Array in String
            string str = Convert.ToBase64String(enteredHash);
            // Erwarteten Hash-Wert aus Datenbank auslesen
            string expectedHash = getHashFromDB(userID);

            // Vergleichen der Hash-Werte (evtl. Sicherheitsrisiko)
            bool hashesMatch = str.Equals(expectedHash);
            if (DEBUG)
            {
                // Testausgabe
                Console.WriteLine($"Salt (aus DB):       {salt}");
                Console.WriteLine($"Hash (aus DB):       {expectedHash}");
                Console.WriteLine($"Hash (aus Eingabe):  {str}");
                Console.WriteLine($"Hash Werte gleich:   {hashesMatch}");
            }
            return hashesMatch;
        }

        public static bool CheckNutzername(int userID, string nutzername)
        {
            // Salt-Wert aus Datenbank auslesen
            string salt = getSaltFromDBNutzer(userID);

            // Umwandeln des Salt in byte-Array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Bestimmen des Passwort-Hash-Wert für eingegebenes Passwort
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(nutzername, saltBytes);
            // Werte müssen identisch zu den Werten beim Generieren des Passwortes sein
            rfc2898DeriveBytes.IterationCount = numberOfIterations;
            byte[] enteredHash = rfc2898DeriveBytes.GetBytes(20);
            // Umwandeln von byte-Array in String
            string str = Convert.ToBase64String(enteredHash);
            // Erwarteten Hash-Wert aus Datenbank auslesen
            string expectedHash = getHashFromDBNutzer(userID);

            // Vergleichen der Hash-Werte (evtl. Sicherheitsrisiko)
            bool hashesMatch = str.Equals(expectedHash);
            if (DEBUG)
            {
                // Testausgabe
                //Console.WriteLine($"Salt (aus DB):       {salt}");
                //Console.WriteLine($"Hash (aus DB):       {expectedHash}");
                //Console.WriteLine($"Hash (aus Eingabe):  {str}");
                //Console.WriteLine($"Hash Werte gleich:   {hashesMatch}");
            }
            return hashesMatch;
        }

        public static SaltAndHash GenerateSaltAndHash(string password)
        {
            // Bibliotheksklasse zum Erzeugen eines Hash-Wertes und eines Salt-Wertes
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32);
            // Anzahl der Iterationen (erhöht den Rechenaufwand)
            rfc2898DeriveBytes.IterationCount = numberOfIterations;
            // Auslesen des generierten Hash-Wertes
            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            // Auslesen des generierten Salt-Wertes
            byte[] salt = rfc2898DeriveBytes.Salt;

            // Umwandeln von einem byte-Array in einen String 
            string saltString = Convert.ToBase64String(salt);
            string passwordHash = Convert.ToBase64String(hash);
            // Ein Array mit Salt- und Hash-Wert werden zurück gegeben
            return new SaltAndHash(saltString, passwordHash);
        }

        // TODO: Hash-Wert des Nutzer-Passworts aus Datenbank auslesen.
        public static String getHashFromDB(int userID)
        {
            return "TXu5fDOOBJETNMGKoFDrqz8r/34=";
        }

        // TODO: Salt-Wert des Nutzer-Passworts aus Datenbank auslesen.
        public static String getSaltFromDB(int userID)
        {
            return "U91qHJfrynnhSj+1xY13YbRk5z10zYAhT79qVz5EQMo=";
        }

        public static String getHashFromDBNutzer(int userID)
        {
            return "Vext+HzWZXo8mwn7WEsw1uqzUW8=";
        }

        // TODO: Salt-Wert des Nutzer-Passworts aus Datenbank auslesen.
        public static String getSaltFromDBNutzer(int userID)
        {
            return "U91qHJfrynnhSj+1xY13YbRk5z10zYAhT79qVz5EQMo=";
        }
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Absenden_Click(object sender, RoutedEventArgs e)
        {
            int UserID1 = 1;
            string Username = TB_USername.Text;
            string password = passwordbox.Password;

            bool b= Program.CheckNutzername(UserID1, Username) ;
            bool a= Program.CheckPassword(UserID1, password);

            if (b == true && a == true)
            {
                Fifastats fifastats = new Fifastats();
                fifastats.Show();
                this.Close();
                
            }
            else
            {
                LogInerfolgreich.Content = "Log IN fehlgeschlagen";
            }
        }
    }
}


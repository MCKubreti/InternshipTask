using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InternshipTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //the application should be started from the debug folder
            int maxYears = 0, rating = 0;
            string path1 = args[0];
            string path2 = args[1];
            try
            {               
                Console.Write("Enter the maximum number of years the player has played in the league to qualify:");
                maxYears = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter the minimum rating the player should have to qualify:");
                rating = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Wrong input!");
            }
            IList<Player> players = LoadJson(path1);
            ExportToCSV(players, maxYears, rating, path2);


        }

        public static IList<Player> LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                players = players.OrderByDescending(x => x.rating).ToList();
                return players;
            }
        }

        public static void ExportToCSV(IList<Player> players, int maxYears, int rating, string path)
        {
            StringBuilder csvcontent = new StringBuilder();
            csvcontent.AppendLine("Name,Rating");
            foreach (var player in players)
            {
                if (DateTime.Now.Year - player.playingSince <= maxYears && player.rating >= rating)
                {
                    Console.WriteLine(player.name + " " + player.playingSince + " " + player.position + " " + player.rating);

                    csvcontent.AppendLine(player.name + ", " + player.rating.ToString());
                }
            }

            string csvpath = path + @"\NBA2.csv";
            File.AppendAllText(csvpath, csvcontent.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserItemDinges;
using System.Collections;

namespace UserItemDinges
{
    class Program
    {
        static void Main(string[] args)
        {
            Userpreference up = new Userpreference(); //Introducing a new UserPreference objects, which will execute and calculate
            Dictionary<int, ArrayList> methodResult = up.ReadDataFile("userItem.data"); //Loading Data from text file
            ArrayList userIDs = up.GetAllUserIds(methodResult); //Get all available user id's and store the result in a ArrayList
            int userToCompare = CompareUsers();
            BasicOperation(up, methodResult, userToCompare);
            
            //Tuple<List<double>, List<double>> userSimilarities = up.FilterMissingTwoUsers(methodResult, usersToCompare[0], usersToCompare[1]); //Fixed users now, needs to be user input
            //double pearson = up.PearsonCalculation(userSimilarities); //Calculation of the pearson coefficient between two users
            //Console.WriteLine(pearson);
            ;

        }

        //Asks the user for input; What kind of operation would the user want to execute?
        public static void BasicOperation(Userpreference up, Dictionary<int,ArrayList> methodResult, int userToCompare)
        {
            Console.WriteLine("Hit a key, pearson will be executed");
            string answer = Console.ReadLine();

            Dictionary<int, double> pearsonResults = up.GetAllPearsons(methodResult, userToCompare);
            int xx = 0;
            Dictionary<int, double> bestPearsons = new Dictionary<int, double>();
            foreach (KeyValuePair<int, double> item in pearsonResults)
            {
                if(xx < 3)
                {
                    bestPearsons.Add(item.Key, item.Value);
                    xx++;
                }

                Console.WriteLine("UserID: " + item.Key + ", pearson: " + item.Value);
            }
            Console.ReadLine();
            Console.WriteLine("Hit a key, Missing items of chosen user will be found");
            string answer2 = Console.ReadLine();
            ArrayList missingItemsChosenUser = CheckForMissingItems(userToCompare);
            Dictionary<int, int> missingItemsBothUsers = new Dictionary<int, int>(); //store items missing in both compared users
            foreach (ArrayList item in missingItemsChosenUser)
            {
                foreach(KeyValuePair<int, double> record in bestPearsons)
                {
                    ArrayList missing = CheckForMissingItems(record.Key);
                    if (missing.Contains(item))
                    {
                        var index = missing.IndexOf(item);
                        missingItemsBothUsers.Add(record.Key, Convert.ToInt32((missing[index].ToString())));
                    }
                }
            }

            Console.ReadLine();
            
            Console.ReadLine();

            //next step - Pak de rates van persoon y die bij persoon x ontbreken, doe per rate (pearson*rate) / pearson en sla uitkomst op als de nieuwe rate voor gebruiker x
        }

        private static int CompareUsers()
        {
            Userpreference up = new Userpreference();
            Dictionary<int, ArrayList> methodResult = up.ReadDataFile("userItem.data");
            ArrayList userIDs = up.GetAllUserIds(methodResult);
            Console.WriteLine("Select a User ID");

            for (int x = 0; x < userIDs.Count; x++)
            {
                Console.WriteLine(userIDs[x]);
            }
            int user = Convert.ToInt32(Console.ReadLine());

            return user;
        }

        //functie maken voor missing items checken meerdere 

        private static ArrayList CheckForMissingItems(int userToCompare)
        {
            Userpreference up = new Userpreference();
            Dictionary<int, ArrayList> methodResult = up.ReadDataFile("userItem.data");
            ArrayList userIDs = up.GetAllUserIds(methodResult);
            if (userIDs.Contains(userToCompare))
            {
                ArrayList missingItemsChosenUser = up.GetMissingItemIds(methodResult, userToCompare);
                Console.WriteLine("Item Id's with missing rating for this userid:" + userToCompare);
                if (missingItemsChosenUser.Count > 0)
                {
                    Console.WriteLine("Missing items for chosen user:");
                    foreach (string item in missingItemsChosenUser)
                    {
                        Console.WriteLine(item);
                    }
                    return missingItemsChosenUser;

                }
                else if (missingItemsChosenUser.Count == 0)
                {
                    Console.WriteLine("none");
                    return null;
                }

                Console.ReadLine();
                return null;

                //Andere beoordelingen zelfde item ophalen
                //Dictionary<int, ArrayList> sd = up.GetItemScoresOtherUsers(methodResult, 103);

                //foreach (KeyValuePair<int, ArrayList> itemId in sd)
                //{
                //    Console.WriteLine("item rating of other users:");
                //    Console.WriteLine(itemId.Key);
                //    Console.WriteLine(itemId.Value[0]);
                //    Console.WriteLine(itemId.Value[1]);

                //}
                //Console.ReadLine();
            }
            else
            {
                Console.WriteLine("User does not exist, try again...\n");
                CheckForMissingItems(userToCompare);
            }
            return null;
        }
    }
}
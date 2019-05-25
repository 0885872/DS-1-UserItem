using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UserItemDinges
{
    class Userpreference
    {

        public double[] preferences;


        public Dictionary<int, ArrayList> ReadDataFile(string fileName)
        {
            Dictionary<int, ArrayList> savedItems = new Dictionary<int, ArrayList>();
            var lines = File.ReadAllLines(fileName);
            for (var i = 0; i < lines.Length; i += 1)
            {
                var line = lines[i];
                string[] tempArray = line.Split(null).ToArray<string>();

                int candidateKey = Convert.ToInt32(tempArray[0]);

                if (!savedItems.ContainsKey(candidateKey))
                {
                    ArrayList valsToAdd = new ArrayList();
                    valsToAdd.Add(tempArray[1]);
                    valsToAdd.Add(tempArray[2]);
                    savedItems.Add(candidateKey, valsToAdd);
                }
                else if (savedItems.ContainsKey(candidateKey))
                {
                    ArrayList currentVals = savedItems[candidateKey];
                    currentVals.Add(tempArray[1]);
                    currentVals.Add(tempArray[2]);
                    savedItems[candidateKey] = currentVals;
                }

            }
            return savedItems;
        }

        //Get all id's of users with 1+ rated item
        public ArrayList GetAllUserIds(Dictionary<int, ArrayList> dict)
        {
            ArrayList userId = new ArrayList();
            foreach (KeyValuePair<int, ArrayList> key in dict)
            {
                userId.Add(key.Key);
            }
            return userId;
        }

        //Get all id's of unrated items
        public ArrayList GetMissingItemIds(Dictionary<int, ArrayList> dict, int userId)
        {
            ArrayList totalItems = new ArrayList();
            ArrayList userItems = new ArrayList();
            ArrayList missingItemsForUser = new ArrayList();
            ArrayList secondUserItems = new ArrayList();

            if (dict.ContainsKey(userId))
            {
                Console.WriteLine("All available Items:");
                foreach (KeyValuePair<int, ArrayList> user in dict)
                {
                    ArrayList items = user.Value;
                    for (int x = 0; x < items.Count; x += 2)
                    {
                        if (!totalItems.Contains(items[x]))
                        {
                            totalItems.Add(items[x]);
                            Console.WriteLine(items[x]);
                        }

                    }
                }
                ArrayList userItemsArrayList = dict[userId];
                for (int x = 0; x < userItemsArrayList.Count; x += 2)
                {
                    userItems.Add(userItemsArrayList[x]);
                }

                for (int i = 0; i < totalItems.Count; i++)
                {
                    string value = totalItems[i] as string;
                    if (!userItems.Contains(value))
                    {
                        missingItemsForUser.Add(value);
                    }
                }
            }

            return missingItemsForUser;
        }


        //public Dictionary<int, ArrayList> GetItemScoresOtherUsers(Dictionary<int, ArrayList> dict, int itemId)
        //{
        //    Dictionary<int, ArrayList> otherScores = new Dictionary<int, ArrayList>();
        //    //for each user
        //    foreach (KeyValuePair<int, ArrayList> user in dict)
        //    {
        //        //For each record 
        //        for (int x = 0; x < user.Value.Count; x++)
        //        {
        //            ArrayList values = new ArrayList();
        //            string userValue = user.Value[x].ToString();
        //            values.Add(userValue);
        //            if (x != (user.Value.Count - 1)){
        //                string userValue2 = user.Value[x + 1].ToString();
        //                values.Add(userValue2);
        //            }


        //            Console.WriteLine(userValue);
        //            if (userValue == Convert.ToString(itemId))
        //            {
        //                otherScores.Add(user.Key, values);
        //            }
        //        }
        //    }
        //    return otherScores;
        //}


        public void getRatesofProductsByUsers(int[] userIds, int[] productIds, Dictionary<int, ArrayList> dict)
        {
            Dictionary<int, int[]> finalDict = new Dictionary<int, int[]>();
            for (int x = 0; x < userIds.Length; x++)
            {
                for (int y = 0; y < productIds.Length; y++)
                {
                        foreach(KeyValuePair<int, ArrayList> item in dict)
                        {
                            foreach(int iD in item.Value)
                            {
                            if (item.Key == userIds[x] && iD == productIds[y])
                            {
                                finalDict.Add(item.Key, productIds)

                            }
                        }
                            
                        }

                }
            }
        }
        //Filters the uncommon ratings and returns the filtered ratings in lists, wrapped in a Tuple. Result is ready for pearson calculation.
        public Tuple<List<double>, List<double>> FilterMissingTwoUsers(Dictionary<int, ArrayList> dict, int userId1, int userId2)
        {
            List<double> firstUserItems = new List<double>();
            List<double> secondUserItems = new List<double>();

            if (dict.ContainsKey(userId1) && dict.ContainsKey(userId2))
            {
                ArrayList userOne = dict[userId1];
                ArrayList userTwo = dict[userId2];

                for (int x = 0; x < userOne.Count; x += 2)
                {
                    for (int y = 0; y < userTwo.Count; y += 2)
                    {
                        string first = userOne[x].ToString();
                        string second = userTwo[y].ToString();

                        if (first == second)
                        {
                            int extraX = x + 1;
                            string firstReview = userOne[x + 1].ToString();
                            string secondReview = userTwo[y + 1].ToString();
                            firstUserItems.Add(Convert.ToDouble(firstReview));
                            secondUserItems.Add(Convert.ToDouble(secondReview));
                        }
                    }
                }
                Tuple<List<double>, List<double>> result = new Tuple<List<double>, List<double>>(firstUserItems, secondUserItems);
                return result;
            }
            else { return null; }

        }

        // Calculate the Pearson coefficient
        public double PearsonCalculation(Tuple<List<double>, List<double>> parsed_ratings)
        {
            // Get the parsed data
            List<double> user1_ratings = parsed_ratings.Item1;
            List<double> user2_ratings = parsed_ratings.Item2;
            int n = user1_ratings.Count;

            // Set default values
            double sum_of_x = 0;
            double sum_of_y = 0;
            double sum_of_x_squared = 0;
            double sum_of_y_squared = 0;
            double sum_of_x_times_y = 0;

            // Calculate the values using sigma
            for (int i = 0; i < n; i++)
            {
                sum_of_x += user1_ratings[i];
                sum_of_y += user2_ratings[i];
                sum_of_x_squared += Math.Pow(user1_ratings[i], 2);
                sum_of_y_squared += Math.Pow(user2_ratings[i], 2);
                sum_of_x_times_y += user1_ratings[i] * user2_ratings[i];
            }

            //  Calculate the rest of the values using the values from the loop
            double avarage_of_sum_of_y_squared = (sum_of_y * sum_of_y) / n;
            double avarage_of_sum_of_x_squared = (sum_of_x * sum_of_x) / n;
            double avarage_of_sum_of_x_times_sum_of_y = (sum_of_x * sum_of_y) / n;

            // Calculate the final values
            double a = sum_of_x_times_y - avarage_of_sum_of_x_times_sum_of_y;
            double b = Math.Sqrt(sum_of_x_squared - avarage_of_sum_of_x_squared);
            double c = Math.Sqrt(sum_of_y_squared - avarage_of_sum_of_y_squared);

            // Calculate the result
            double result = a / (b * c);
            return result;
        }

        public Dictionary<int, double> GetAllPearsons(Dictionary<int, ArrayList> dict, int userId1)
        {
            Dictionary<int, double> pearsonsCalculated = new Dictionary<int, double>();
            ArrayList allUserIds = this.GetAllUserIds(dict);
            for (int x = 0; x < allUserIds.Count; x++)
            {
                string currentUser = allUserIds[x].ToString();
                string userid = Convert.ToString(userId1);
                if (currentUser == userid)
                {
                    //do nothing and break execution
                }
                else
                {
                    Tuple<List<double>, List<double>> filtered = FilterMissingTwoUsers(dict, userId1, Convert.ToInt32(currentUser));
                    double pearson = PearsonCalculation(filtered);
                    pearsonsCalculated.Add(Convert.ToInt32(currentUser), pearson);
                }

            }
            pearsonsCalculated = pearsonsCalculated.OrderByDescending(u => u.Value).ToDictionary(z => z.Key, y => y.Value);
            return pearsonsCalculated;
        }
    }
}

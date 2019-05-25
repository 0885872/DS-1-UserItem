using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserItemDinges
{
    class HashMap
    {
        public List<string> listToSave;
        Dictionary<int, Array> tempDict;


        private bool Add(string line)
        {
            List<string> tempList = line.Split(null).ToList();
            //if (!tempDict.ContainsKey(tempList[0]))
            //{
                //list = new List<int>();
                //dictionary.Add("foo", list);
            //}
            //tempDict.Add(id, reviews);
            return true;
        }
    }
}

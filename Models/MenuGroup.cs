using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Models
{
    public class MenuGroup
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

    }
}

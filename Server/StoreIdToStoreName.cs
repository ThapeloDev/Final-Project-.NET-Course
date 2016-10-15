using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class StoreIdToStoreName
    {
        private static Dictionary<string, string> _dictionary = new Dictionary<string, string> {
            {"7290103152017","אושר עד"}, {"7290027600007","שופרסל"}, {"7290058140886","רמי לוי"}
        };

        public static string GetStoreName(string storeId)
        {
            return _dictionary[storeId];
        }
    }
}

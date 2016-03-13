using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace Pizza.Mvc.Helpers
{
    public static class RouteTableHelper
    {
        public static List<string> GetApplicationAreaNames()
        {
            return RouteTable.Routes.OfType<Route>()
                .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
                .Select(r => r.DataTokens["area"]).Cast<string>()
                .ToList();
        }
    }
}
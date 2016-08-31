using System;

namespace Pizza.Mvc.GridConfig.Exceptions
{
    public class GridBuildingException : Exception
    {
        public GridBuildingException(string message) 
            : base(message) 
        {
        }
    }
}
using System;

namespace Pizza.Mvc.Grid.Metamodel.Exceptions
{
    public class GridBuildingException : Exception
    {
        public GridBuildingException(string message) 
            : base(message) 
        {
        }
    }
}
using Autofac;

namespace Pizza.Framework
{
    public class PizzaServerContext
    {
        public static void Initialize(IContainer container)
        {
            var context = new PizzaServerContext(container);
            Current = context;
        }

        public static PizzaServerContext Current { get; private set; }

        private PizzaServerContext(IContainer container)
        {
            this.Container = container;
        }

        public IContainer Container { get; private set; }
    }
}
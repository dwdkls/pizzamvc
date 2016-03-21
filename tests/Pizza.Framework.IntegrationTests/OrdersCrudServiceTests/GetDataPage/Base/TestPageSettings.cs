namespace Pizza.Framework.IntegrationTests.OrdersCrudServiceTests.GetDataPage.Base
{
    internal class TestPageSettings
    {
        public int ExpectedLoaded { get; private set; }
        public int ExpectedTotal { get; private set; }

        public TestPageSettings(int expectedLoaded, int expectedTotal)
        {
            this.ExpectedLoaded = expectedLoaded;
            this.ExpectedTotal = expectedTotal;
        }
    }
}
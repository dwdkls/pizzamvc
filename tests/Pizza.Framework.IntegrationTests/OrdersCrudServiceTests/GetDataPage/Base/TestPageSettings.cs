namespace Pizza.Framework.IntegrationTests.OrdersCrudServiceTests.GetDataPage.Base
{
    internal class TestPageSettings
    {
        public int ExpectedLoaded { get; }
        public int ExpectedTotal { get; }

        public TestPageSettings(int expectedLoaded, int expectedTotal)
        {
            this.ExpectedLoaded = expectedLoaded;
            this.ExpectedTotal = expectedTotal;
        }
    }
}
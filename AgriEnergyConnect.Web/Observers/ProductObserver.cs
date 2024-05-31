namespace AgriEnergyConnect.Web.Observers
{
    public class ProductObserver : IObserver
    {
        private readonly ILogger<ProductObserver> _logger;

        public ProductObserver(ILogger<ProductObserver> logger)
        {
            _logger = logger;
        }

        public void Update(string message)
        {
            _logger.LogInformation("Product update: {Message}", message);
        }
    }

}

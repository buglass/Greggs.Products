namespace Greggs.Products.Api.PriceCalculation
{
	public class CurrencyPriceCalculator : IRegionalPriceCalculation
	{
		public decimal CalculatePrice(string currency, decimal priceInPounds)
		{
			return priceInPounds;
		}
	}
}

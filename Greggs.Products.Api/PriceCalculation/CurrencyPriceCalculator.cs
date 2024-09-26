namespace Greggs.Products.Api.PriceCalculation
{
	public class CurrencyPriceCalculator : IPriceCalculation
	{
		public decimal CalculatePrice(string currency, decimal priceInPounds)
		{
			return priceInPounds;
		}
	}
}

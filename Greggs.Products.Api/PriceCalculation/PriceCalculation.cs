namespace Greggs.Products.Api.PriceCalculation
{
	public class PriceCalculation : IPriceCalculation
	{
		public decimal CalculatePrice(string currency, decimal priceInPounds)
		{
			return priceInPounds;
		}
	}
}

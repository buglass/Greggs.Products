namespace Greggs.Products.Api.PriceCalculation
{
	public interface ICurrencyPriceConverter
	{
		decimal GetPrice(string currency, decimal priceInPounds);
	}
}

using System.Reflection.Metadata.Ecma335;

namespace Greggs.Products.Api.PriceCalculation
{
	public interface IRegionalPriceCalculation
	{
		decimal CalculatePrice(string currency, decimal priceInPounds);
	}
}

using System.Collections.Generic;

namespace Greggs.Products.Api.CurrencyPrices
{
	public interface IConversionRates
	{
		Dictionary<string, decimal> Rates { get; }
	}
}

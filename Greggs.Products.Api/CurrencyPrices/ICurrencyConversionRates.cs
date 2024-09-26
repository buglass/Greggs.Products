using System.Collections.Generic;

namespace Greggs.Products.Api.CurrencyPrices
{
	public interface ICurrencyConversionRates
	{
		Dictionary<string, decimal> ConversionRates { get; }
	}
}

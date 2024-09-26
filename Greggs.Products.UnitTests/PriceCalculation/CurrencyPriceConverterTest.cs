using System;
using System.Collections.Generic;
using Greggs.Products.Api.CurrencyPrices;
using Greggs.Products.Api.PriceCalculation;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class CurrencyPriceConverterTest
{
	[Fact]
	public void PriceConvertedAsExpected()
	{
		var currencyConversionRates = new Mock<ICurrencyConversionRates>();
		currencyConversionRates.Setup(ccr => ccr.ConversionRates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 1.11m }
			}
		);

		Assert.Equal(
			2.21m,
			new CurrencyPriceConverter(currencyConversionRates.Object).GetPrice("CUR", 1.99m)
		);
	}

	[Fact]
	public void UnsupportedCurrencyThrowsException()
	{
		var currencyConversionRates = new Mock<ICurrencyConversionRates>();
		currencyConversionRates.Setup(ccr => ccr.ConversionRates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 1.00m }
			}
		);

		Assert.Throws<ArgumentException>(
			() => new CurrencyPriceConverter(currencyConversionRates.Object).GetPrice("FOO", 1.00m)
		);
	}

	[Fact]
	public void ConversionRateOfZeroThrowsException()
	{
		var currencyConversionRates = new Mock<ICurrencyConversionRates>();
		currencyConversionRates.Setup(ccr => ccr.ConversionRates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 0.00m }
			}
		);

		Assert.Throws<InvalidOperationException>(
			() => new CurrencyPriceConverter(currencyConversionRates.Object).GetPrice("CUR", 1.00m)
		);
	}
}

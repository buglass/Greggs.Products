using System;
using System.Collections.Generic;
using Greggs.Products.Api.CurrencyPrices;
using Greggs.Products.Api.PriceCalculation;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class PriceConverterTest
{
	[Fact]
	public void PriceConvertedAsExpected()
	{
		var conversionRates = new Mock<IConversionRates>();
		conversionRates.Setup(cr => cr.Rates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 1.11m }
			}
		);

		Assert.Equal(
			2.21m,
			new PriceConverter(conversionRates.Object).GetPrice("CUR", 1.99m)
		);
	}

	[Fact]
	public void UnsupportedCurrencyThrowsException()
	{
		var conversionRates = new Mock<IConversionRates>();
		conversionRates.Setup(cr => cr.Rates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 1.00m }
			}
		);

		Assert.Throws<ArgumentException>(
			() => new PriceConverter(conversionRates.Object).GetPrice("FOO", 1.00m)
		);
	}

	[Fact]
	public void ConversionRateOfZeroThrowsException()
	{
		var conversionRates = new Mock<IConversionRates>();
		conversionRates.Setup(cr => cr.Rates).Returns(
			new Dictionary<string, decimal>
			{
				{ "CUR", 0.00m }
			}
		);

		Assert.Throws<InvalidOperationException>(
			() => new PriceConverter(conversionRates.Object).GetPrice("CUR", 1.00m)
		);
	}
}

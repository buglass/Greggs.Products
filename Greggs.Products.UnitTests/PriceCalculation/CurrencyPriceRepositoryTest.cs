using System;
using Greggs.Products.Api.CurrencyPrices;
using Greggs.Products.Api.PriceCalculation;
using Xunit;

namespace Greggs.Products.UnitTests;

public class CurrencyPriceRepositoryTest
{
	[Fact]
	public void GBPReturnsSamePrice()
	{
		Assert.Equal(1.00m, new CurrencyPriceRepository(new CurrencyConversionRates()).GetPrice("GBP", 1.00m));
	}

	[Fact]
	public void EURReturnsAdjustedPrice()
	{
		Assert.Equal(1.11m, new CurrencyPriceRepository(new CurrencyConversionRates()).GetPrice("EUR", 1.00m));
	}

	[Fact]
	public void EURReturnsAdjustedPriceForDecimal()
	{
		Assert.Equal(2.21m, new CurrencyPriceRepository(new CurrencyConversionRates()).GetPrice("EUR", 1.99m));
	}

	[Fact]
	public void UnsupportedCurrencyThrowsArgumentException()
	{
		Assert.Throws<ArgumentException>(() => new CurrencyPriceRepository(new CurrencyConversionRates()).GetPrice("USD", 1.00m));
	}
}

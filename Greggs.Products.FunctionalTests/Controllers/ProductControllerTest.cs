using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.CurrencyPrices;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.PriceCalculation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.FunctionalTests;

public class ProductControllerTest
{
	[Fact]
	public void GetWithNoParametersReturnsDefaultPage()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", Price = 1m },
					new Product { Name = "Vegan Sausage Roll", Price = 1.1m },
					new Product { Name = "Steak Bake", Price = 1.2m },
					new Product { Name = "Yum Yum", Price = 0.7m },
					new Product { Name = "Pink Jammie", Price = 0.5m },
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					new ProductAccess(),
					new PriceConverter(new ConversionRates())
				).Get()
		);
	}

	[Fact]
	public void GetWithEurosCurrencyParametersReturnsEurosPrices()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", Price = 1.11m },
					new Product { Name = "Vegan Sausage Roll", Price = 1.22m },
					new Product { Name = "Steak Bake", Price = 1.33m },
					new Product { Name = "Yum Yum", Price = 0.78m },
					new Product { Name = "Pink Jammie", Price = 0.56m },
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					new ProductAccess(),
					new PriceConverter(new ConversionRates())
				).Get(currencyCode: "EUR")
		);
	}

	[Fact]
	public void GetWithPageLimitReturnsRequestedItems()
	{
		Assert.Equivalent(
		expected:
		new List<Product>
		{
			new Product { Name = "Sausage Roll", Price = 1m },
			new Product { Name = "Vegan Sausage Roll", Price = 1.1m },
		},
		actual:
		new ProductController(
		new Mock<ILogger<ProductController>>().Object,
		new ProductAccess(),
		new PriceConverter(new ConversionRates())
		).Get(pageStart: 0, pageSize: 2)
		);
	}
	[Fact]
	public void GetPageWithPageLimitReturnsRequestedItems()
	{
		Assert.Equivalent(
		expected:
		new List<Product>
		{
			new Product { Name = "Steak Bake", Price = 1.2m },
			new Product { Name = "Yum Yum", Price = 0.7m },
		},
		actual:
		new ProductController(
		new Mock<ILogger<ProductController>>().Object,
		new ProductAccess(),
		new PriceConverter(new ConversionRates())
		).Get(pageStart: 1, pageSize: 2)
		); ;
	}
	[Fact]
	public void GetPageBelowZeroReturnsDefaultPage()
	{
		Assert.Equivalent(
		expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", Price = 1m },
					new Product { Name = "Vegan Sausage Roll", Price = 1.1m },
					new Product { Name = "Steak Bake", Price = 1.2m },
					new Product { Name = "Yum Yum", Price = 0.7m },
					new Product { Name = "Pink Jammie", Price = 0.5m },
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					new ProductAccess(),
					new PriceConverter(new ConversionRates())
				).Get(pageStart: -1)
		);
	}
}


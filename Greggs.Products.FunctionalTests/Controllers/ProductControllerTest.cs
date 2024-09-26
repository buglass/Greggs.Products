using System;
using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
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
	public void GetWithNoPagingReturnsFirstFiveItems()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", PriceInPounds = 1m },
					new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
					new Product { Name = "Steak Bake", PriceInPounds = 1.2m },
					new Product { Name = "Yum Yum", PriceInPounds = 0.7m },
					new Product { Name = "Pink Jammie", PriceInPounds = 0.5m },
				},
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, new ProductAccess(), new CurrencyPriceRepository())
				.Get()
		);
	}

	[Fact]
	public void GetWithPageLimitReturnsRequestedItems()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", PriceInPounds = 1m },
					new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
				},
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, new ProductAccess(), new CurrencyPriceRepository())
				.Get(pageStart: 0, pageSize: 2)
		);
	}

	[Fact]
	public void GetPageWithPageLimitReturnsRequestedItems()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Steak Bake", PriceInPounds = 1.2m },
					new Product { Name = "Yum Yum", PriceInPounds = 0.7m },
				},
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, new ProductAccess(), new CurrencyPriceRepository())
				.Get(pageStart: 1, pageSize: 2)
		); ;
	}

	[Fact]
	public void GetPageBelowZeroReturnsDefaultPage()
	{
		Assert.Equivalent(
			expected:
				new List<Product>
				{
					new Product { Name = "Sausage Roll", PriceInPounds = 1m },
					new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
					new Product { Name = "Steak Bake", PriceInPounds = 1.2m },
					new Product { Name = "Yum Yum", PriceInPounds = 0.7m },
					new Product { Name = "Pink Jammie", PriceInPounds = 0.5m },
				},
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, new ProductAccess(), new CurrencyPriceRepository())
				.Get(pageStart: -1)
		);
	}
}


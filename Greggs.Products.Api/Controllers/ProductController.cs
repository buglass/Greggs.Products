using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.PriceCalculation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _dataAccess;
    private readonly ICurrencyPriceConverter _priceCalculation;

    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> dataAccess, ICurrencyPriceConverter priceCalculation)
    {
        _logger = logger;
        _dataAccess = dataAccess;
        _priceCalculation = priceCalculation;
    }

    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5, string currencyCode = "GBP")
    {
        if (pageSize < 0) {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

		if (pageSize == 0) {
			return Enumerable.Empty<Product>();
		}

        if (pageStart < 0) {
            pageStart = 0;
        }

        pageStart = pageStart * pageSize;

        return this._dataAccess.List(pageStart: pageStart, pageSize: pageSize).Select(product =>
            new Product
            {
                Name = product.Name,
                PriceInPounds = _priceCalculation.GetPrice(currencyCode, product.PriceInPounds)
            }
        );
	}
}
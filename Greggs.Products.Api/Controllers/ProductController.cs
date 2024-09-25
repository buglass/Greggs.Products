using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _dataAccess;

    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
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

		return this._dataAccess.List(pageStart: pageStart, pageSize: pageSize);
	}
}
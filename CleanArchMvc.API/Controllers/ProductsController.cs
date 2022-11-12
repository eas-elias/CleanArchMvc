using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {


        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productService.GetProducts();
            if (products == null)
            {
                return NotFound("Products not found");
            }

            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int? id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO ProductDto)
        {
            if (ProductDto == null)
            {
                return BadRequest("Invalid Data");
            }

            await _productService.Add(ProductDto);

            return new CreatedAtRouteResult("GetProduct", new { id = ProductDto.Id });
        }

        [HttpPut]

        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO ProductDto)
        {
            if (id != ProductDto.Id)
            {
                return BadRequest("invalid Id");
            }

            if (ProductDto == null)
            {
                return BadRequest("Invalid Data");
            }

            await _productService.Update(ProductDto);

            return Ok(ProductDto);
        }


        [HttpDelete]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var Product = await _productService.GetById(id);

            if (Product == null)
            {
                return NotFound("Product not found");
            }

            await _productService.Remove(id);

            return Ok(Product);
        }


    }
}

using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Products.Commands;
using CleanArchMvc.Application.Products.Queries;
using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Application.Services
{
    public class ProductService : IProductService
    {

        private IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentException(nameof(mediator));

            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {

            var productsQuery = new GetProductsQuery();

            if (productsQuery == null)
                throw new ApplicationException($"Entity could not be loaded.");


            var result = await _mediator.Send(productsQuery);
            return _mapper.Map<IEnumerable<ProductDTO>>(result);

        }

        public async Task<ProductDTO> GetById(int? id)
        {
            var productsByIdQuery = new GetProductByIdQuery(id.Value);

            if (productsByIdQuery == null)
                throw new ApplicationException($"Entity could not be loaded.");


            var result = await _mediator.Send(productsByIdQuery);
            return _mapper.Map<ProductDTO>(result);
        }


        //public async Task<ProductDTO> GetProductCategory(int? id)
        //{
        //    var productsByIdQuery = new GetProductByIdQuery(id.Value);

        //    if (productsByIdQuery == null)
        //        throw new ApplicationException($"Entity could not be loaded.");


        //    var result = await _mediator.Send(productsByIdQuery);
        //    return _mapper.Map<ProductDTO>(result);
        //}


        public async Task Add(ProductDTO productDTO)
        {
            var productCreateCommand = _mapper.Map<ProductCreateCommand>(productDTO);
            await _mediator.Send(productCreateCommand);

        }

        public async Task Update(ProductDTO productDTO)
        {
            var productUpdateCommand = _mapper.Map<ProductUpdateCommand>(productDTO);
            await _mediator.Send(productUpdateCommand);
        }

        public async Task Remove(int? id)
        {
            var productRemoveCommand = new ProductRemoveCommand(id.Value);
            
            if (productRemoveCommand == null)
                throw new ApplicationException($"Entity could not be loaded.");


            await _mediator.Send(productRemoveCommand);
        }


        //private IProductRepository _productRepository;
        //private readonly IMapper _mapper;

        //public ProductService(IMapper mapper, IProductRepository productRepository)
        //{
        //    _productRepository = productRepository ??
        //        throw new ArgumentException(nameof(productRepository));

        //    _mapper = mapper;
        //}

        //public async Task<IEnumerable<ProductDTO>> GetProducts()
        //{
        //    var productsEntity = await _productRepository.GetProductsAsync();
        //    return _mapper.Map<IEnumerable<ProductDTO>>(productsEntity);
        //}

        //public async Task<ProductDTO> GetById(int? id)
        //{
        //    var productEntity = await _productRepository.GetByIdAsync(id);
        //    return _mapper.Map<ProductDTO>(productEntity);
        //}

        //public async Task<ProductDTO> GetProductCategory(int? id)
        //{
        //    var productEntity = await _productRepository.GetProductCategoryAsync(id);
        //    return _mapper.Map<ProductDTO>(productEntity);
        //}

        //public async Task Add(ProductDTO productDTO)
        //{
        //    var productEntity = _mapper.Map<Product>(productDTO);
        //    await _productRepository.CreateAsync(productEntity);
        //}

        //public async Task Update(ProductDTO productDTO)
        //{
        //    var productEntity = _mapper.Map<Product>(productDTO);
        //    await _productRepository.UpdateAsync(productEntity);
        //}

        //public async Task Remove(int? id)
        //{
        //    var productEntity = _productRepository.GetByIdAsync(id).Result;
        //    await _productRepository.RemoveAsync(productEntity);
        //}
    }
}

using System.Net;
using AspNet.CorrelationIdGenerator;
using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IMediator mediator, ILogger<BasketController> logger, IPublishEndpoint publishEndpoint,
        ICorrelationIdGenerator correlationIdGenerator, DiscountGrpcService discountGrpcService)
    {
        _mediator = mediator;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _correlationIdGenerator = correlationIdGenerator;
        _discountGrpcService = discountGrpcService;
        _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
    }
    
    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);
        return Ok(basket);
    }
    
    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
    {
        foreach (var item in createShoppingCartCommand.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        
        var basket = await _mediator.Send(createShoppingCartCommand);
        return Ok(basket);
    }
    
    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        var query = new DeleteBasketByUserNameQuery(userName); 
        await _mediator.Send(query);
        return Ok();
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        //Get existing basket with username
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        if (basket == null)
        {
            return BadRequest();
        }

        var eventMesg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMesg.TotalPrice = basket.TotalPrice;
        eventMesg.CorrelationId = _correlationIdGenerator.Get();
        await _publishEndpoint.Publish(eventMesg);
        
        var deleteQuery = new DeleteBasketByUserNameQuery(basketCheckout.UserName);
        await _mediator.Send(deleteQuery);
        return Accepted();
    } 
}
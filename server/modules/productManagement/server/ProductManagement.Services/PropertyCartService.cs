using Core.Database;
using Core.Exceptions;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts;
using System;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyCartService : BaseService<PropertyCart, PropertyCartDto>, IPropertyCartService
    {
        private readonly IMembership _membership;
        private readonly IUserService _userService;

        public PropertyCartService(IMappingService mappingService, IRepository repository, IMembership membership, IUserService userService) : base(mappingService, repository)
        {
            _membership = membership;
            _userService = userService;
        }

        public PropertyCart GetAvailableCartForUser(long userId)
        {
            return First(cart => cart.UserId == userId && !cart.CheckedOut && !cart.Closed);
        }

        public void ClosePropertyCart(long id)
        {
            var cart = GetCart(id);
            if (cart == null)
                throw new BaseNotFoundException<PropertyCart>();

            cart.Closed = true;
            Repository.Update(cart);
        }

        public void MarkCartAsCheckedOut(long id)
        {
            var cart = GetCart(id);
            if (cart == null)
                throw new BaseNotFoundException<PropertyCart>();

            cart.CheckedOut = true;
            cart.CheckedOutDate = DateTimeOffset.Now;
            Repository.Update(cart);
        }

        public void UpdateCartItem(long cartId, long propertyId, int quantity)
        {
            var cart = GetCart(cartId);

            var item = cart.Items.FirstOrDefault(cartItem => cartItem.PropertyId == propertyId);

            if (item == null)
            {
                throw new BaseNotFoundException<PropertyCartItem>();
            }

            item.Quantity = quantity;
            item.ModifiedDate = DateTimeOffset.Now;

            Repository.Update(item);
            Repository.Update(cart);
        }

        private PropertyCart GetCart(long cartId)
        {
            var cart = GetEntity(cartId);

            if (cart.UserId != _membership.UserId)
                throw new UnauthorizedAccessException();

            if (cart.CheckedOut || cart.Closed)
                throw new InvalidOperationException("Property Cart has been checked out or closed, cannot edit cart detail.");
            return cart;
        }

        public void RemovePropertyFromCart(long cartId, long propertyId)
        {
            var cart = GetCart(cartId);
            var item = cart.Items.FirstOrDefault(cartItem => cartItem.PropertyId == propertyId);

            if (item == null)
            {
                throw new BaseNotFoundException<PropertyCartItem>();
            }

            Repository.Delete(item);
            Repository.Update(cart);
        }

        public override PropertyCart PrepareForUpdating(PropertyCart entity, PropertyCartDto model)
        {
            entity = base.PrepareForUpdating(entity, model);

            foreach (var propertyCartItemDto in model.Items)
            {
                var cartItem = entity.Items.FirstOrDefault(x => x.PropertyId == propertyCartItemDto.PropertyId);
                if (cartItem == null)
                    continue;

                if (propertyCartItemDto.AppliedPrice.HasValue)
                    cartItem.AppliedPrice = propertyCartItemDto.AppliedPrice;
                if (propertyCartItemDto.AppliedUnitPrice.HasValue)
                    cartItem.AppliedUnitPrice = propertyCartItemDto.AppliedUnitPrice;
                cartItem.Quantity = propertyCartItemDto.Quantity;
            }

            return entity;
        }

        public override PropertyCartDto ToDto(PropertyCart entity)
        {
            var propertyCartDto = base.ToDto(entity);

            if (propertyCartDto.DelegateToUserId != null)
                propertyCartDto.DelegateToUser =
                    _userService.GetUserPersonalInformation(propertyCartDto.DelegateToUserId.Value);

            return propertyCartDto;
        }
    }
}
using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Services
{
    public interface IPropertyCartService : IBaseService<PropertyCart, PropertyCartDto>, IDependency
    {
        PropertyCart GetAvailableCartForUser(long userId);

        void ClosePropertyCart(long id);

        void MarkCartAsCheckedOut(long id);

        void UpdateCartItem(long cartId, long propertyId, int quantity);

        void RemovePropertyFromCart(long cartId, long propertyId);
    }
}
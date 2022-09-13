using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class PropertyCartEntityTypeConfig : EntityTypeConfiguration<PropertyCart>
    {
        public PropertyCartEntityTypeConfig()
        {
            HasKey(cart => cart.Id);
        }
    }

    public class PropertyCartItemEntityTypeConfig : EntityTypeConfiguration<PropertyCartItem>
    {
        public PropertyCartItemEntityTypeConfig()
        {
            HasKey(cartItem => new
            {
                cartItem.PropertyId,
                cartItem.PropertyCartId
            });

            HasRequired(cartItem => cartItem.PropertyCart)
                .WithMany(cart => cart.Items);

            HasRequired(cart => cart.Property);
        }
    }
}
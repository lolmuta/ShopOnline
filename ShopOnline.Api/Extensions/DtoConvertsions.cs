using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConvertsions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories)
        {
            return (from product in products
                    join productCategory in productCategories
                    on product.CategoryId equals productCategory.Id
                    select ConvertToDto(product, productCategory))
                    .ToList();
            //select new ProductDto
            //{
            //    Id = productCategory.Id,
            //    Name = productCategory.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Qty = product.Qty,
            //    CategoryId = product.CategoryId,
            //    CategoryName = productCategory.Name,
            //    ImageURL = product.ImageURL
            //}).ToList();
        }
        public static ProductDto ConvertToDto(this Product product,
           ProductCategory productCategory)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CategoryName = productCategory.Name,
                ImageURL = product.ImageURL
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
            IEnumerable<Product> products)
            => (from cartItem in cartItems 
                join product in products
                on cartItem.ProductId equals product.Id
                select ConvertToDto(cartItem, product))
                .ToList();

        public static CartItemDto ConvertToDto(this CartItem cartItem,
            Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                
                TotoalPrice = product.Price * cartItem.Qty,
            };
        }
    }
}

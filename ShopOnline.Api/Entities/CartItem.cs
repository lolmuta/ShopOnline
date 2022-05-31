namespace ShopOnline.Api.Entities
{
    /// <summary>
    /// 購物車上的東西
    /// </summary>
    public class CartItem
    {
        public int  Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }


    }
}

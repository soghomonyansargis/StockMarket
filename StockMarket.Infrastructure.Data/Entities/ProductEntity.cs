namespace StockMarket.Infrastructure.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsInStock { get; set; }
    }
}
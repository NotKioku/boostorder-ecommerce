using System.Text.Json.Serialization;

namespace boostorder_ecommerce.Models
{
    public class ProductResponseWrapper
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; } = string.Empty;

        [JsonPropertyName("regular_price")]
        public string RegularPrice { get; set; } = string.Empty;

        [JsonPropertyName("images")]
        public List<ProductImage> Images { get; set; } = new();

        [JsonPropertyName("attributes")]
        public List<ProductAttribute> Attributes { get; set; } = new();

        [JsonPropertyName("variations")]
        public List<ProductVariation> Variations { get; set; } = new();

        [JsonPropertyName("in_stock")]
        public bool InStock { get; set; }

        [JsonPropertyName("stock_quantity")]
        public int? StockQuantity { get; set; }
    }

    public class ProductAttribute
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public List<string> Options { get; set; } = new();
    }

    public class ProductVariation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; } = string.Empty;

        [JsonPropertyName("regular_price")]
        public string RegularPrice { get; set; } = string.Empty;

        [JsonPropertyName("sale_price")]
        public string? SalePrice { get; set; }

        [JsonPropertyName("in_stock")]
        public bool InStock { get; set; }

        [JsonPropertyName("stock_quantity")]
        public int? StockQuantity { get; set; }

        [JsonPropertyName("uom")]
        public string? Uom { get; set; }

        [JsonPropertyName("attributes")]
        public List<VariationAttribute> Attributes { get; set; } = new();

        [JsonPropertyName("inventory")]
        public List<ProductInventory> Inventory { get; set; } = new();

        public double TotalInventoryStock
        {
            get
            {
                if (Inventory != null && Inventory.Any())
                {
                    return Inventory.Where(i => i.StockQuantity.HasValue).Sum(i => i.StockQuantity!.Value);
                }
                return StockQuantity ?? 0;
            }
        }
    }

    public class ProductInventory
    {
        [JsonPropertyName("branch_id")]
        public int BranchId { get; set; }

        [JsonPropertyName("stock_quantity")]
        public double? StockQuantity { get; set; }
    }

    public class VariationAttribute
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("option")]
        public string Option { get; set; } = string.Empty;
    }

    public class ProductCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class ProductImage
    {
        [JsonPropertyName("src")]
        public string Src { get; set; } = string.Empty;

        [JsonPropertyName("src_small")]
        public string? SrcSmall { get; set; }

        [JsonPropertyName("src_medium")]
        public string? SrcMedium { get; set; }

        [JsonPropertyName("src_large")]
        public string? SrcLarge { get; set; }
    }
}

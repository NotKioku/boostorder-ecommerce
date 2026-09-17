namespace boostorder_ecommerce.GlobalStates
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Uom { get; set; } = "UNIT";
        public int StockQuantity { get; set; } = 100;
        public string ImageSrc { get; set; } = string.Empty;
    }

    public class CartState
    {
        public List<CartItem> Items { get; private set; } = new();

        public event Action? OnStateChanged;

        public int TotalCount => Items.Sum(i => i.Quantity);
        public int UniqueCount => Items.Count;

        public void AddItem(CartItem item)
        {
            var existing = Items.FirstOrDefault(i => i.Id == item.Id && string.Equals(i.Uom, item.Uom, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
                if (existing.Quantity <= 0)
                {
                    Items.Remove(existing);
                }
            }
            else if (item.Quantity > 0)
            {
                Items.Add(item);
            }
            NotifyStateChanged();
        }

        public void UpdateQuantity(CartItem item, int quantity)
        {
            if (quantity <= 0)
            {
                Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            NotifyStateChanged();
        }

        public void RemoveItem(CartItem item)
        {
            Items.Remove(item);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace InventoryManagementSystem
{
    // Product Class
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Product() { }

        public Product(int id, string name, int quantity, decimal price)
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public void DisplayProductInfo()
        {
            Console.WriteLine($"ID: {ID} | Name: {Name} | Quantity: {Quantity} | Price: {Price:C}");
        }
    }

    // Inventory Class
    public class Inventory
    {
        private List<Product> products;
        private const string DATA_FILE = "inventory.json";

        public Inventory()
        {
            products = new List<Product>();
            LoadFromFile();
        }

        public void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW PRODUCT ===");

            try
            {
                // Prompt the user for product details
                Console.Write("Enter Product ID: ");
                int id = int.Parse(Console.ReadLine());

                // Check if ID already exists
                if (products.Any(p => p.ID == id))
                {
                    Console.WriteLine("Product with this ID already exists!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter Product Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                Console.Write("Enter Price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                // Create a new Product object and add it to the inventory list
                Product newProduct = new Product(id, name, quantity, price);

                // Display product details for confirmation
                Console.WriteLine("\n=== PRODUCT DETAILS ===");
                newProduct.DisplayProductInfo();

                // Confirm the addition to the user
                Console.Write("\nDo you want to add this product? (y/n): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y" || confirmation == "yes")
                {
                    products.Add(newProduct);
                    SaveToFile();
                    Console.WriteLine("\nProduct added successfully to the inventory!");
                }
                else
                {
                    Console.WriteLine("\nProduct addition cancelled.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void UpdateProduct()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE PRODUCT ===");

            if (products.Count == 0)
            {
                Console.WriteLine("No products available to update.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                // Prompt the user for the product ID to update
                Console.Write("Enter Product ID to update: ");
                int id = int.Parse(Console.ReadLine());

                // Search for the product in the inventory list
                Product product = products.FirstOrDefault(p => p.ID == id);
                if (product == null)
                {
                    Console.WriteLine("Product not found in the inventory!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\n=== CURRENT PRODUCT INFORMATION ===");
                product.DisplayProductInfo();

                // Store original values for comparison
                string originalName = product.Name;
                int originalQuantity = product.Quantity;
                decimal originalPrice = product.Price;

                // If found, prompt for new details and update the product
                Console.WriteLine("\n=== ENTER NEW DETAILS (Press Enter to keep current value) ===");

                Console.Write($"Enter new Product Name (current: {product.Name}): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                    product.Name = newName;

                Console.Write($"Enter new Quantity (current: {product.Quantity}): ");
                string quantityInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(quantityInput))
                    product.Quantity = int.Parse(quantityInput);

                Console.Write($"Enter new Price (current: {product.Price:C}): ");
                string priceInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(priceInput))
                    product.Price = decimal.Parse(priceInput);

                // Show updated product details
                Console.WriteLine("\n=== UPDATED PRODUCT DETAILS ===");
                product.DisplayProductInfo();

                // Confirm the update to the user
                Console.Write("\nDo you want to save these changes? (y/n): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y" || confirmation == "yes")
                {
                    SaveToFile();
                    Console.WriteLine("\nProduct updated successfully!");
                }
                else
                {
                    // Restore original values
                    product.Name = originalName;
                    product.Quantity = originalQuantity;
                    product.Price = originalPrice;
                    Console.WriteLine("\nUpdate cancelled. Product remains unchanged.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE PRODUCT ===");

            if (products.Count == 0)
            {
                Console.WriteLine("No products available to delete.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                // Prompt the user for the product ID to delete
                Console.Write("Enter Product ID to delete: ");
                int id = int.Parse(Console.ReadLine());

                // Search for the product in the inventory list
                Product product = products.FirstOrDefault(p => p.ID == id);
                if (product == null)
                {
                    Console.WriteLine("Product not found in the inventory!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // If found, display product details
                Console.WriteLine("\n=== PRODUCT TO BE DELETED ===");
                product.DisplayProductInfo();

                // Confirm the deletion to the user
                Console.Write("\nAre you sure you want to delete this product? This action cannot be undone! (y/n): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y" || confirmation == "yes")
                {
                    // Remove the product from the list
                    products.Remove(product);
                    SaveToFile();
                    Console.WriteLine("\nProduct deleted successfully from the inventory!");
                }
                else
                {
                    Console.WriteLine("\nDeletion cancelled. Product remains in inventory.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void ViewProducts()
        {
            Console.Clear();
            Console.WriteLine("=== VIEW ALL PRODUCTS ===");

            // Display all products in the inventory list
            if (products.Count == 0)
            {
                Console.WriteLine("No products in inventory.");
            }
            else
            {
                Console.WriteLine($"\nTotal products in inventory: {products.Count}\n");

                // Include details like ID, Name, Quantity, Price
                Console.WriteLine("ID\t| Name\t\t\t| Quantity\t| Price");
                Console.WriteLine("".PadRight(60, '-'));

                foreach (Product product in products.OrderBy(p => p.ID))
                {
                    Console.WriteLine($"{product.ID}\t| {product.Name.PadRight(15)}\t| {product.Quantity}\t\t| {product.Price:C}");
                }

                Console.WriteLine("".PadRight(60, '-'));
                Console.WriteLine($"Total inventory value: {products.Sum(p => p.Quantity * p.Price):C}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void GenerateReport()
        {
            Console.Clear();
            Console.WriteLine("=== INVENTORY REPORT ===");

            if (products.Count == 0)
            {
                Console.WriteLine("No products in inventory to generate report.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            // Calculate total inventory value
            decimal totalValue = products.Sum(p => p.Quantity * p.Price);
            int totalItems = products.Sum(p => p.Quantity);
            var lowStockProducts = products.Where(p => p.Quantity < 10).ToList();

            string reportContent = GenerateReportContent(totalValue, totalItems, lowStockProducts);

            // Display the report
            Console.WriteLine(reportContent);

            // Optionally, save the report to a file
            Console.Write("\nWould you like to save this report to a file? (y/n): ");
            string saveChoice = Console.ReadLine().ToLower();

            if (saveChoice == "y" || saveChoice == "yes")
            {
                try
                {
                    string fileName = $"InventoryReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    File.WriteAllText(fileName, reportContent);
                    Console.WriteLine($"\nReport saved successfully as: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving report: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private string GenerateReportContent(decimal totalValue, int totalItems, List<Product> lowStockProducts)
        {
            var report = new System.Text.StringBuilder();

            report.AppendLine($"INVENTORY MANAGEMENT SYSTEM - REPORT");
            report.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine("".PadRight(60, '='));

            // Display a summary of inventory levels
            report.AppendLine($"Total number of product types: {products.Count}");
            report.AppendLine($"Total items in inventory: {totalItems}");
            report.AppendLine($"Total inventory value: {totalValue:C}");
            report.AppendLine($"Average product value: {(products.Count > 0 ? totalValue / products.Count : 0):C}");
            report.AppendLine();

            // Product details
            report.AppendLine("PRODUCT DETAILS:");
            report.AppendLine("".PadRight(60, '-'));
            report.AppendLine("ID\t| Name\t\t\t| Quantity\t| Price\t\t| Total Value");
            report.AppendLine("".PadRight(80, '-'));

            foreach (var product in products.OrderBy(p => p.ID))
            {
                decimal productValue = product.Quantity * product.Price;
                report.AppendLine($"{product.ID}\t| {product.Name.PadRight(15)}\t| {product.Quantity}\t\t| {product.Price:C}\t\t| {productValue:C}");
            }
            report.AppendLine();

            // Low stock alerts
            if (lowStockProducts.Any())
            {
                report.AppendLine("LOW STOCK ALERT (Quantity < 10):");
                report.AppendLine("".PadRight(40, '-'));
                foreach (var product in lowStockProducts)
                {
                    report.AppendLine($"- {product.Name} (ID: {product.ID}) - Only {product.Quantity} left");
                }
                report.AppendLine();
            }

            // Most valuable products
            report.AppendLine("TOP 5 MOST VALUABLE PRODUCTS:");
            report.AppendLine("".PadRight(40, '-'));
            var topProducts = products.OrderByDescending(p => p.Quantity * p.Price).Take(5);
            foreach (var product in topProducts)
            {
                report.AppendLine($"- {product.Name}: {(product.Quantity * product.Price):C}");
            }

            return report.ToString();
        }

        private void LoadFromFile()
        {
            try
            {
                if (File.Exists(DATA_FILE))
                {
                    string json = File.ReadAllText(DATA_FILE);
                    products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                products = new List<Product>();
            }
        }

        private void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(products, options);
                File.WriteAllText(DATA_FILE, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            bool running = true;

            while (running)
            {
                DisplayMainMenu();

                Console.Write("Select an option (1-6): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        inventory.AddProduct();
                        break;
                    case "2":
                        inventory.UpdateProduct();
                        break;
                    case "3":
                        inventory.DeleteProduct();
                        break;
                    case "4":
                        inventory.ViewProducts();
                        break;
                    case "5":
                        inventory.GenerateReport();
                        break;
                    case "6":
                        Console.WriteLine("Thank you for using the Inventory Management System!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option! Please try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║        INVENTORY MANAGEMENT SYSTEM     ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine("║  1. Add a Product                      ║");
            Console.WriteLine("║  2. Update a Product                   ║");
            Console.WriteLine("║  3. Delete a Product                   ║");
            Console.WriteLine("║  4. View All Products                  ║");
            Console.WriteLine("║  5. Generate Report                    ║");
            Console.WriteLine("║  6. Exit Application                   ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
        }
    }
}
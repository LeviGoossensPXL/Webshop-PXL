using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Webshop.Application.Repositories;
using Webshop.Domain.Entities;

namespace Webshop.Infrastructure.Data;

/// <summary>
/// contains resources used for seeding or initializing data
/// </summary>
public static class SeedData
{
    public static string DefaultImage => "/images/default.jpg";

    public static AppUser AdminUser => new AppUser
    {
        Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
        UserName = "admin@webshop.com",
        Email = "admin@webshop.com",
        EmailConfirmed = true
        //Password = "AdminPassword123!"
    };

    public static IEnumerable<IdentityRole> Roles =>
    [
        new IdentityRole("Admin"),
        new IdentityRole("Client")
    ];

    public static IEnumerable<StockItem> StockItems =>
    [
        new StockItem { ProductId = 1, Quantity = 100, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 2, Quantity = 110, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 3, Quantity = 120, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 4, Quantity = 130, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 5, Quantity = 140, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 6, Quantity = 150, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 7, Quantity = 160, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 8, Quantity = 170, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 9, Quantity = 180, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 10, Quantity = 190, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 11, Quantity = 200, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 12, Quantity = 210, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 13, Quantity = 220, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 14, Quantity = 230, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 15, Quantity = 240, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 16, Quantity = 250, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 17, Quantity = 260, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 18, Quantity = 270, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 19, Quantity = 280, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 20, Quantity = 290, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 21, Quantity = 300, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 22, Quantity = 290, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 23, Quantity = 280, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 24, Quantity = 270, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 25, Quantity = 260, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 26, Quantity = 250, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 27, Quantity = 240, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 28, Quantity = 230, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 29, Quantity = 210, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 30, Quantity = 200, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 31, Quantity = 190, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 32, Quantity = 180, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 33, Quantity = 170, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 34, Quantity = 160, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 35, Quantity = 150, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 36, Quantity = 140, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 37, Quantity = 130, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 38, Quantity = 120, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 39, Quantity = 110, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 40, Quantity = 100, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 41, Quantity = 90, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 42, Quantity = 80, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 43, Quantity = 70, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 44, Quantity = 60, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 45, Quantity = 50, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 46, Quantity = 40, WarehouseLocation = "Gent" },
        new StockItem { ProductId = 47, Quantity = 30, WarehouseLocation = "Luik" },
        new StockItem { ProductId = 48, Quantity = 20, WarehouseLocation = "Antwerpen" },
        new StockItem { ProductId = 49, Quantity = 10, WarehouseLocation = "Hasselt" },
        new StockItem { ProductId = 50, Quantity = 0, WarehouseLocation = "Gent" },
    ];

    public static IEnumerable<Category> Categories => (List<Category>)
    [
        new Category
        {
            CategoryId = 1, Name = "Tents & Shelters", Description = "Professional tents and tarps for all weather."
        },
        new Category
            { CategoryId = 2, Name = "Sleeping Gear", Description = "Bags, mats, and cots for outdoor comfort." },
        new Category
            { CategoryId = 3, Name = "Camp Kitchen", Description = "Stoves, coolers, and cookware for camp meals." },
        new Category
            { CategoryId = 4, Name = "Lighting & Power", Description = "Lanterns, headlamps, and solar power banks." },
        new Category
            { CategoryId = 5, Name = "Backpacks & Bags", Description = "Hiking backpacks and waterproof duffel bags." },
        new Category
            { CategoryId = 6, Name = "Tools & Accessories", Description = "Knives, pumps, ropes, and first aid kits." }
    ];

    public static IEnumerable<Product> Products = (List<Product>)
    [
        new Product
        {
            Name = "Ultralight Solo Tent", Description = "1-person tent for fast and light backpacking.",
            Price = 129.99m, Sku = "TNT-001", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Alpine Dome 2", Description = "2-person 4-season tent for mountain climbing.", Price = 299.50m,
            Sku = "TNT-002", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Family Cabin 6", Description = "Large 6-person tent with room dividers.", Price = 349.00m,
            Sku = "TNT-003", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Pop-up Beach Tent", Description = "Instant setup sun shelter with UV protection.", Price = 45.99m,
            Sku = "TNT-004", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Survival Bivy Sack", Description = "Emergency waterproof sleeping cover.", Price = 25.00m,
            Sku = "TNT-005", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Heavy Duty Tarp 3x3", Description = "Waterproof tarp shelter with reinforced corners.",
            Price = 39.90m, Sku = "TNT-006", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Car Camping Awning", Description = "Attaches to your car roof rack for instant shade.",
            Price = 199.00m, Sku = "TNT-007", ImageUrl = DefaultImage, CategoryId = 1
        },

        new Product
        {
            Name = "Hammock with Bug Net", Description = "Jungle hammock with integrated mosquito net.", Price = 65.50m,
            Sku = "TNT-008", ImageUrl = DefaultImage, CategoryId = 1
        },

        // Sleeping Gear

        new Product
        {
            Name = "Down Winter Bag -15C", Description = "Extreme cold weather mummy sleeping bag.", Price = 249.99m,
            Sku = "SLP-001", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Summer Envelope Bag", Description = "Lightweight rectangular bag for warm nights.", Price = 35.00m,
            Sku = "SLP-002", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Double Sleeping Bag", Description = "Queen size bag for couples, unzips into two.", Price = 89.99m,
            Sku = "SLP-003", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Self-Inflating Mat 5cm", Description = "Comfortable foam core sleeping pad.", Price = 49.50m,
            Sku = "SLP-004", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Ultralight Air Pad", Description = "Compact air mattress for hikers.", Price = 75.00m,
            Sku = "SLP-005", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Closed Cell Foam Mat", Description = "Basic, indestructible foam sleeping pad.", Price = 15.99m,
            Sku = "SLP-006", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Folding Camp Cot", Description = "Elevated sleeping bed with steel frame.", Price = 85.00m,
            Sku = "SLP-007", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Inflatable Camp Pillow", Description = "Soft and compact travel pillow.", Price = 18.50m,
            Sku = "SLP-008", ImageUrl = DefaultImage, CategoryId = 2
        },

        new Product
        {
            Name = "Fleece Bag Liner", Description = "Adds extra warmth to any sleeping bag.", Price = 22.00m,
            Sku = "SLP-009", ImageUrl = DefaultImage, CategoryId = 2
        },

        // Camp Kitchen

        new Product
        {
            Name = "Dual Burner Stove", Description = "Classic two-burner propane camp stove.", Price = 99.99m,
            Sku = "KIT-001", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Backpacking Mini Stove", Description = "Pocket-sized titanium gas burner.", Price = 24.50m,
            Sku = "KIT-002", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Hard Anodized Cookset", Description = "Pots, pans, and kettle for 4 people.", Price = 65.00m,
            Sku = "KIT-003", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Cast Iron Skillet", Description = "Heavy duty pan for cooking over open fire.", Price = 35.99m,
            Sku = "KIT-004", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Rotomolded Cooler 45L", Description = "Premium cooler keeps ice for 5 days.", Price = 199.00m,
            Sku = "KIT-005", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Soft Sided Cooler Bags", Description = "Light cooler for day trips and picnics.", Price = 29.99m,
            Sku = "KIT-006", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Stainless Steel Mug", Description = "Double wall insulated camp coffee mug.", Price = 14.50m,
            Sku = "KIT-007", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Titanium Spork", Description = "Spoon and fork combo, ultra lightweight.", Price = 9.99m,
            Sku = "KIT-008", ImageUrl = DefaultImage, CategoryId = 3
        },

        new Product
        {
            Name = "Water Filter Pump", Description = "Removes 99.9% of bacteria from wild water.", Price = 55.00m,
            Sku = "KIT-009", ImageUrl = DefaultImage, CategoryId = 3
        },

        // & Power

        new Product
        {
            Name = "Rechargeable LED Lantern", Description = "1000 lumen camp light with power bank feature.",
            Price = 45.00m, Sku = "LGT-001", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Propane Gas Lantern", Description = "Traditional bright light and warmth source.", Price = 39.99m,
            Sku = "LGT-002", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Pro Headlamp 400L", Description = "Waterproof headlamp with red light mode.", Price = 34.50m,
            Sku = "LGT-003", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Kids Camp Flashlight", Description = "Small, durable, and colorful flashlight.", Price = 12.00m,
            Sku = "LGT-004", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Solar String Fairy Lights", Description = "Warm white lights to decorate your camp.",
            Price = 22.99m, Sku = "LGT-005", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Portable Solar Panel 21W", Description = "Foldable panel to charge phones off-grid.",
            Price = 89.00m, Sku = "LGT-006", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Rugged Power Bank 20k", Description = "Water and dust resistant battery pack.", Price = 49.99m,
            Sku = "LGT-007", ImageUrl = DefaultImage, CategoryId = 4
        },

        new Product
        {
            Name = "Tent Ceiling Fan & Light", Description = "2-in-1 fan and LED light for hot summer nights.",
            Price = 28.50m, Sku = "LGT-008", ImageUrl = DefaultImage, CategoryId = 4
        },

        // & Bags

        new Product
        {
            Name = "Expedition Backpack 75L", Description = "Large multi-day trekking backpack.", Price = 159.99m,
            Sku = "BAG-001", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Weekend Hiking Pack 40L", Description = "Perfect size for 1-2 night trips.", Price = 95.00m,
            Sku = "BAG-002", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Daypack 20L", Description = "Lightweight bag for short daily hikes.", Price = 35.50m,
            Sku = "BAG-003", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Waterproof Duffel 60L", Description = "Keeps your gear dry on canoe trips.", Price = 79.90m,
            Sku = "BAG-004", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Hydration Pack 2L", Description = "Backpack with integrated water bladder.", Price = 45.00m,
            Sku = "BAG-005", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Dry Bag Set (3 Pack)", Description = "Roll-top dry bags to protect electronics.", Price = 24.99m,
            Sku = "BAG-006", ImageUrl = DefaultImage, CategoryId = 5
        },

        new Product
        {
            Name = "Compression Stuff Sack", Description = "Saves space by compressing sleeping bags.", Price = 18.00m,
            Sku = "BAG-007", ImageUrl = DefaultImage, CategoryId = 5
        },

        // & Accessories

        new Product
        {
            Name = "Survival Multi-Tool", Description = "Pliers, knife, saw, and screwdrivers in one.", Price = 59.99m,
            Sku = "ACC-001", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Folding Camp Saw", Description = "Cuts firewood quickly and safely.", Price = 24.50m,
            Sku = "ACC-002", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Camping Hatchet", Description = "Small axe for splitting kindling.", Price = 32.00m,
            Sku = "ACC-003", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Pro First Aid Kit", Description = "Comprehensive medical supplies for wilderness.", Price = 45.00m,
            Sku = "ACC-004", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Electric Air Pump 12V", Description = "Plugs into car to inflate beds fast.", Price = 22.99m,
            Sku = "ACC-005", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Double Action Hand Pump", Description = "Manual pump for high pressure inflatables.",
            Price = 19.50m, Sku = "ACC-006", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Paracord 30m", Description = "550lb strength nylon utility rope.", Price = 12.99m, Sku = "ACC-007",
            ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Heavy Duty Steel Pegs", Description = "Set of 10 rock pegs for hard ground.", Price = 14.00m,
            Sku = "ACC-008", ImageUrl = DefaultImage, CategoryId = 6
        },

        new Product
        {
            Name = "Duct Tape Roll", Description = "The ultimate emergency repair tool.", Price = 5.99m,
            Sku = "ACC-009", ImageUrl = DefaultImage, CategoryId = 6
        }
    ];
}
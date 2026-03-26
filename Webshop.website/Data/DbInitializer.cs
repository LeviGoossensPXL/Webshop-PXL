using Microsoft.AspNetCore.Identity;
using Webshop.Domain.Entities;
using Webshop.Application.Repositories;

namespace Webshop.website.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var categoryRepo = serviceProvider.GetRequiredService<ICategoryRepository>();
            var productRepo = serviceProvider.GetRequiredService<IProductRepository>();

            // Check if we already have products
            var existingProducts = await productRepo.GetAll();
            if (existingProducts.Any())
            {
                return; // Stop here if database is not empty
            }
            
            // 1. Create 6 main camping categories
            var categories = new List<Category>
            {
                new Category { Name = "Tents & Shelters", Description = "Professional tents and tarps for all weather." },
                new Category { Name = "Sleeping Gear", Description = "Bags, mats, and cots for outdoor comfort." },
                new Category { Name = "Camp Kitchen", Description = "Stoves, coolers, and cookware for camp meals." },
                new Category { Name = "Lighting & Power", Description = "Lanterns, headlamps, and solar power banks." },
                new Category { Name = "Backpacks & Bags", Description = "Hiking backpacks and waterproof duffel bags." },
                new Category { Name = "Tools & Accessories", Description = "Knives, pumps, ropes, and first aid kits." }
            };

            var existingCategories = await categoryRepo.GetAll();
            if (!existingCategories.Any())
            {
                foreach (var category in categories)
                {
                    await categoryRepo.Add(category);
                }
            }

            // Get the categories from the database to use their IDs
            var dbCategories = await categoryRepo.GetAll();
            var tentId = dbCategories.First(c => c.Name == "Tents & Shelters").CategoryId;
            var sleepId = dbCategories.First(c => c.Name == "Sleeping Gear").CategoryId;
            var kitchenId = dbCategories.First(c => c.Name == "Camp Kitchen").CategoryId;
            var lightId = dbCategories.First(c => c.Name == "Lighting & Power").CategoryId;
            var bagId = dbCategories.First(c => c.Name == "Backpacks & Bags").CategoryId;
            var toolId = dbCategories.First(c => c.Name == "Tools & Accessories").CategoryId;

            var defaultImage = "/images/default.jpg";

            // 2. Create 50 professional camping products
            var products = new List<Product>
            {
                // Tents & Shelters
                new Product { Name = "Ultralight Solo Tent", Description = "1-person tent for fast and light backpacking.", Price = 129.99m, Sku = "TNT-001", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Alpine Dome 2", Description = "2-person 4-season tent for mountain climbing.", Price = 299.50m, Sku = "TNT-002", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Family Cabin 6", Description = "Large 6-person tent with room dividers.", Price = 349.00m, Sku = "TNT-003", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Pop-up Beach Tent", Description = "Instant setup sun shelter with UV protection.", Price = 45.99m, Sku = "TNT-004", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Survival Bivy Sack", Description = "Emergency waterproof sleeping cover.", Price = 25.00m, Sku = "TNT-005", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Heavy Duty Tarp 3x3", Description = "Waterproof tarp shelter with reinforced corners.", Price = 39.90m, Sku = "TNT-006", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Car Camping Awning", Description = "Attaches to your car roof rack for instant shade.", Price = 199.00m, Sku = "TNT-007", ImageUrl = defaultImage, CategoryId = tentId },
                new Product { Name = "Hammock with Bug Net", Description = "Jungle hammock with integrated mosquito net.", Price = 65.50m, Sku = "TNT-008", ImageUrl = defaultImage, CategoryId = tentId },

                // Sleeping Gear
                new Product { Name = "Down Winter Bag -15C", Description = "Extreme cold weather mummy sleeping bag.", Price = 249.99m, Sku = "SLP-001", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Summer Envelope Bag", Description = "Lightweight rectangular bag for warm nights.", Price = 35.00m, Sku = "SLP-002", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Double Sleeping Bag", Description = "Queen size bag for couples, unzips into two.", Price = 89.99m, Sku = "SLP-003", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Self-Inflating Mat 5cm", Description = "Comfortable foam core sleeping pad.", Price = 49.50m, Sku = "SLP-004", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Ultralight Air Pad", Description = "Compact air mattress for hikers.", Price = 75.00m, Sku = "SLP-005", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Closed Cell Foam Mat", Description = "Basic, indestructible foam sleeping pad.", Price = 15.99m, Sku = "SLP-006", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Folding Camp Cot", Description = "Elevated sleeping bed with steel frame.", Price = 85.00m, Sku = "SLP-007", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Inflatable Camp Pillow", Description = "Soft and compact travel pillow.", Price = 18.50m, Sku = "SLP-008", ImageUrl = defaultImage, CategoryId = sleepId },
                new Product { Name = "Fleece Bag Liner", Description = "Adds extra warmth to any sleeping bag.", Price = 22.00m, Sku = "SLP-009", ImageUrl = defaultImage, CategoryId = sleepId },

                // Camp Kitchen
                new Product { Name = "Dual Burner Stove", Description = "Classic two-burner propane camp stove.", Price = 99.99m, Sku = "KIT-001", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Backpacking Mini Stove", Description = "Pocket-sized titanium gas burner.", Price = 24.50m, Sku = "KIT-002", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Hard Anodized Cookset", Description = "Pots, pans, and kettle for 4 people.", Price = 65.00m, Sku = "KIT-003", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Cast Iron Skillet", Description = "Heavy duty pan for cooking over open fire.", Price = 35.99m, Sku = "KIT-004", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Rotomolded Cooler 45L", Description = "Premium cooler keeps ice for 5 days.", Price = 199.00m, Sku = "KIT-005", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Soft Sided Cooler Bags", Description = "Light cooler for day trips and picnics.", Price = 29.99m, Sku = "KIT-006", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Stainless Steel Mug", Description = "Double wall insulated camp coffee mug.", Price = 14.50m, Sku = "KIT-007", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Titanium Spork", Description = "Spoon and fork combo, ultra lightweight.", Price = 9.99m, Sku = "KIT-008", ImageUrl = defaultImage, CategoryId = kitchenId },
                new Product { Name = "Water Filter Pump", Description = "Removes 99.9% of bacteria from wild water.", Price = 55.00m, Sku = "KIT-009", ImageUrl = defaultImage, CategoryId = kitchenId },

                // Lighting & Power
                new Product { Name = "Rechargeable LED Lantern", Description = "1000 lumen camp light with power bank feature.", Price = 45.00m, Sku = "LGT-001", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Propane Gas Lantern", Description = "Traditional bright light and warmth source.", Price = 39.99m, Sku = "LGT-002", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Pro Headlamp 400L", Description = "Waterproof headlamp with red light mode.", Price = 34.50m, Sku = "LGT-003", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Kids Camp Flashlight", Description = "Small, durable, and colorful flashlight.", Price = 12.00m, Sku = "LGT-004", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Solar String Fairy Lights", Description = "Warm white lights to decorate your camp.", Price = 22.99m, Sku = "LGT-005", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Portable Solar Panel 21W", Description = "Foldable panel to charge phones off-grid.", Price = 89.00m, Sku = "LGT-006", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Rugged Power Bank 20k", Description = "Water and dust resistant battery pack.", Price = 49.99m, Sku = "LGT-007", ImageUrl = defaultImage, CategoryId = lightId },
                new Product { Name = "Tent Ceiling Fan & Light", Description = "2-in-1 fan and LED light for hot summer nights.", Price = 28.50m, Sku = "LGT-008", ImageUrl = defaultImage, CategoryId = lightId },

                // Backpacks & Bags
                new Product { Name = "Expedition Backpack 75L", Description = "Large multi-day trekking backpack.", Price = 159.99m, Sku = "BAG-001", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Weekend Hiking Pack 40L", Description = "Perfect size for 1-2 night trips.", Price = 95.00m, Sku = "BAG-002", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Daypack 20L", Description = "Lightweight bag for short daily hikes.", Price = 35.50m, Sku = "BAG-003", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Waterproof Duffel 60L", Description = "Keeps your gear dry on canoe trips.", Price = 79.90m, Sku = "BAG-004", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Hydration Pack 2L", Description = "Backpack with integrated water bladder.", Price = 45.00m, Sku = "BAG-005", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Dry Bag Set (3 Pack)", Description = "Roll-top dry bags to protect electronics.", Price = 24.99m, Sku = "BAG-006", ImageUrl = defaultImage, CategoryId = bagId },
                new Product { Name = "Compression Stuff Sack", Description = "Saves space by compressing sleeping bags.", Price = 18.00m, Sku = "BAG-007", ImageUrl = defaultImage, CategoryId = bagId },

                // Tools & Accessories
                new Product { Name = "Survival Multi-Tool", Description = "Pliers, knife, saw, and screwdrivers in one.", Price = 59.99m, Sku = "ACC-001", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Folding Camp Saw", Description = "Cuts firewood quickly and safely.", Price = 24.50m, Sku = "ACC-002", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Camping Hatchet", Description = "Small axe for splitting kindling.", Price = 32.00m, Sku = "ACC-003", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Pro First Aid Kit", Description = "Comprehensive medical supplies for wilderness.", Price = 45.00m, Sku = "ACC-004", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Electric Air Pump 12V", Description = "Plugs into car to inflate beds fast.", Price = 22.99m, Sku = "ACC-005", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Double Action Hand Pump", Description = "Manual pump for high pressure inflatables.", Price = 19.50m, Sku = "ACC-006", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Paracord 30m", Description = "550lb strength nylon utility rope.", Price = 12.99m, Sku = "ACC-007", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Heavy Duty Steel Pegs", Description = "Set of 10 rock pegs for hard ground.", Price = 14.00m, Sku = "ACC-008", ImageUrl = defaultImage, CategoryId = toolId },
                new Product { Name = "Duct Tape Roll", Description = "The ultimate emergency repair tool.", Price = 5.99m, Sku = "ACC-009", ImageUrl = defaultImage, CategoryId = toolId }
            };

            foreach (var product in products)
            {
                await productRepo.Add(product);
            }
            
            // 3. create roles
            await SeedRolesAsync(serviceProvider);
        }

        private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] seedRoles = ["Admin", "Client"];

            foreach (var role in seedRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
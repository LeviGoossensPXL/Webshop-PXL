using Riok.Mapperly.Abstractions;
using Webshop.Domain.Entities;
using Webshop.website.ViewModels;

namespace Webshop.website.Mappings;

[Mapper]
public partial class StockMapper
{
    public partial StockListViewModel ToListViewModel(StockItem stockItem);
    public partial StockEditViewModel ToEditViewModel(StockItem stockItem);

    public partial StockItem ToStockItem(StockEditViewModel stockListViewModel);
}
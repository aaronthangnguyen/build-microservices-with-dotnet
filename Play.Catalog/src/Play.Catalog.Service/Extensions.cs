using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Extensions
{
    public static class Extensions
    {
        public static ItemDto ToDto(this Item item) => new ItemDto(
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.CreatedDate);
    }

}
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Catalog.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {

            return (await _itemsRepository.GetAllAsync())
                    .Select(item => item.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item.ToDto();

        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(PostItemDto postItemDto)
        {
            var item = new Item
            {
                Name = postItemDto.Name,
                Description = postItemDto.Description,
                Price = postItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.PostAsync(item);

            return CreatedAtAction(nameof(GetAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, PutItemDto putItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.Name = putItemDto.Name;
            existingItem.Description = putItemDto.Description;
            existingItem.Price = putItemDto.Price;

            await _itemsRepository.PutAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            await _itemsRepository.DeleteAsync(item.Id);

            return NoContent();
        }
    }
}